namespace FsDiagrams.Svg

open System.Xml.Linq
open System.IO

open FsDiagrams

module Utils =
    let public xname (name:string) =
        XName.Get(name)

open Utils

module KnownColors =
    let Red = "red"
    let Green = "green"
    let Blue = "blue"
    let Black = "black"

type Color =
    | NamedColor of string
    | HexRgb of string
    | Rgb of byte * byte * byte
    | RgbPercentage of decimal * decimal * decimal
    with
    override this.ToString() =
        match this with
            | NamedColor c -> c
            | HexRgb c -> c
            | Rgb (r, g, b) -> sprintf "rgb(%i, %i, %i)" r g b
            | RgbPercentage (r, g, b) -> sprintf "rgb(%M%%, %M%%, %M%%)" r g b

type LineCap =
    | Round
    with
    override this.ToString() =
        match this with
            | Round -> "round"
    
type LineData = {
    x1 : int;
    y1 : int;
    x2 : int;
    y2 : int;
    stroke : Color option;
    stroke_width : float option;
    stroke_dasharray : int list option;
    stroke_linecap : LineCap option;
    }
    with
    static member Default = {
        x1 = 0; y1 = 0;
        x2 = 0; y2 = 0;
        stroke = Some( NamedColor( KnownColors.Black ) );
        stroke_width = None;
        stroke_dasharray = None;
        stroke_linecap = None;
    }

type GraphicsElement =
    | Path
    | Text
    | Rect
    | Circle
    | Ellipse
    | Line of LineData
    //| Polyline
    //| Polygon
    | Image
    | Use

type Element =
    | GraphicsElement of GraphicsElement

type Document = {
    Elements : Element list
    }

type SvgWriter() =
    let xdoc = new XDocument()
    let svgns = XNamespace.Get(@"http://www.w3.org/2000/svg")
    let svgroot = xdoc.Add(new XElement(svgns + "svg"))

    let WriteLine (data:LineData) (parent:XElement) =
        let attrs:seq<string * string> = seq {
            yield ("x1", string data.x1)
            yield ("y1", string data.y1)
            yield ("x2", string data.x2)
            yield ("y2", string data.y2)
            match data.stroke with
                | Some stroke -> yield("stroke", string stroke)
                | None -> ()
            match data.stroke_dasharray with
                | Some dasharray -> yield("stroke-dasharray", dasharray |> Seq.map string |> String.concat ",")
                | None -> ()
            match data.stroke_linecap with
                | Some linecap -> yield("stroke-linecap", string linecap)
                | None -> ()
        }
        let element =
            new XElement(
                xname "line",
                attrs |> Seq.map (fun (name, value) -> new XAttribute(xname name, value)) |> Seq.toArray)
        parent.Add(element)

    let WriteGraphicsElement (el:GraphicsElement) (parent:XElement) =
        match el with
            | Line data -> WriteLine data parent
            | _ ->  failwith "Element not supported."

    let WriteElement (el:Element) (parent:XElement) =
        match el with
            | GraphicsElement e -> WriteGraphicsElement e parent

    member public this.GetDocument() =
        xdoc

    member public this.WriteDocument(doc:Document) =
        doc.Elements
            |> List.iter (fun el -> WriteElement el xdoc.Root)