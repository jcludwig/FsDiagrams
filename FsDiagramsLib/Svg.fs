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

    let makeAttr ((name, value):string*string) =
        XAttribute(xname name, value)

    let makeAttrs =
        Seq.map makeAttr

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
        
    let writeLine (Line (p1, p2) as line) =
        let (x1, y1) = p1
        let (x2, y2) = p2
        let attrs = [
            ("x1", string x1);
            ("y1", string y1);
            ("x2", string x2);
            ("y2", string y2);
            ]
        XElement(svgname "line", attrs |> makeAttrs)

    let writePathDef (pathDef:PathDefinition) =
        match pathDef with
        | PathDefinition.Line l -> writeLine l
        | _ -> failwith "Path not supported."

    let writePath (parent:XElement) (path:Path) =
        let xpath = writePathDef path.pathDef

        let strokeAttrs = [
            match path.stroke with
                | Some stroke -> yield("stroke", colorString stroke)
                | None -> ()
            match path.stroke_dasharray with
                | Some dasharray -> yield("stroke-dasharray", dasharray |> Seq.map string |> String.concat ",")
                | None -> ()
            match path.stroke_linecap with
                | Some linecap -> yield("stroke-linecap", linecapString linecap)
                | None -> ()
            ]
        
        strokeAttrs |> makeAttrs |> Seq.toArray |> xpath.Add
        parent.Add(xpath)

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