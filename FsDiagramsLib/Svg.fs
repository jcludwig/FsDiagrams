namespace FsDiagrams.Svg

open System.Xml.Linq
open System.IO

open FsDiagrams

module Utils =
    let svgns = XNamespace.Get(@"http://www.w3.org/2000/svg")
    
    let xname (name:string) =
        XName.Get(name)

    let svgname (name:string) =
        svgns + name

open Utils

module SvgWriter =

    let colorString = 
        function
        | NamedColor c -> c
        | HexRgb c -> c
        | Rgb (r, g, b) -> sprintf "rgb(%i, %i, %i)" r g b
        | RgbPercentage (r, g, b) -> sprintf "rgb(%M%%, %M%%, %M%%)" r g b

    let linecapString =
        function
        | Round -> "round"

    let writePath (parent:XElement) (path:Path) =
        let attrs:seq<string * string> = 
            seq {
            match path.pathDef with
                | Line (p1, p2) -> 
                    let (x1, y1) = p1
                    let (x2, y2) = p2
                    yield ("x1", string x1)
                    yield ("y1", string y1)
                    yield ("x2", string x2)
                    yield ("y2", string y2)
                | _ -> failwith "Path not supported."

            match path.stroke with
                | Some stroke -> yield("stroke", colorString stroke)
                | None -> ()
            match path.stroke_dasharray with
                | Some dasharray -> yield("stroke-dasharray", dasharray |> Seq.map string |> String.concat ",")
                | None -> ()
            match path.stroke_linecap with
                | Some linecap -> yield("stroke-linecap", linecapString linecap)
                | None -> ()
            }
        let element =
            new XElement(
                svgname "line",
                attrs |> Seq.map (fun (name, value) -> new XAttribute(xname name, value)) |> Seq.toArray)
        parent.Add(element)

    let writeDiagram (diagram:Diagram) =
        let xdoc = new XDocument()
        let svgroot = new XElement(svgname "svg")
        xdoc.Add(svgroot)
        
        let rec writeElement d =
            match d with
            | Path path -> writePath svgroot path
            | Empty -> () 
            | _ -> failwith "Diagram type not supported."
        
        writeElement diagram
        xdoc