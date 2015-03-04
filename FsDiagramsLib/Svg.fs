namespace FsDiagrams.Svg

open System.Xml.Linq
open System.IO

open FsDiagrams
open FsDiagrams.Core

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
        let (x1, y1) = coords p1
        let (x2, y2) = coords p2
        let attrs = [
            ("x1", string x1);
            ("y1", string y1);
            ("x2", string x2);
            ("y2", string y2);
            ]
        XElement(svgname "line", attrs |> makeAttrs)

    let writeRect (Rect (topleft, size) as rect) =
        let (x1, y1) = coords topleft
        let (w, h) = dims size
        let attrs = [
            ("x", string x1);
            ("y", string y1);
            ("width", string w);
            ("height", string h);
            ]
        XElement(svgname "rect", attrs |> makeAttrs)

    let writePathDef (pathDef:PathDefinition) =
        match pathDef with
        | LinePath l -> writeLine l
        | RectPath r -> writeRect r
        //| _ -> failwith "Path not supported."

    let writePath (xparent:XElement) (path:Path) =
        let xpath = writePathDef path.pathDef

        let strokeAttrs = path.strokeAttrs
        let strokeAttrs = [
            match strokeAttrs.Color with
                | Some stroke -> yield("stroke", colorString stroke)
                | None -> ()
            match strokeAttrs.DashArray with
                | Some dasharray -> yield("stroke-dasharray", dasharray |> Seq.map string |> String.concat ",")
                | None -> ()
            match strokeAttrs.LineCap with
                | Some linecap -> yield("stroke-linecap", linecapString linecap)
                | None -> ()
            ]
        
        strokeAttrs |> makeAttrs |> Seq.toArray |> xpath.Add
        xparent.Add(xpath)

    let rec writeElement xparent d =
        match d with
        | Path path -> writePath xparent path
        | Empty -> ()
        | Compound comp -> writeCompound xparent comp
        | _ -> failwith "Diagram type not supported."

    and writeCompound xparent comp =
        match comp with
        | Overlay diagrams -> 
            diagrams |> List.iter (writeElement xparent)

    let writeDiagram (diagram:Diagram) =
        let xdoc = new XDocument()
        let svgroot = new XElement(svgname "svg")
        xdoc.Add(svgroot)
        
        writeElement svgroot diagram
        xdoc