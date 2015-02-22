namespace FsDiagrams.Svg

open System.Xml.Linq
open System.IO

open FsDiagrams

module KnownColors =
    let Red = "red"
    let Green = "green"
    let Blue = "blue"

type Color =
    | NamedColor of string
    | HexRgb of string
    | Rgb of byte * byte * byte
    | RgbPercentage of float * float * float

type LineCap =
    | Round
    
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

type SvgDocument() =
    let xdoc = new XDocument()
    let svgns = XNamespace.Get(@"http://www.w3.org/2000/svg")
    let svgroot = xdoc.Add(new XElement(svgns + "svg"))

    member public this.GetDocument() =
        xdoc