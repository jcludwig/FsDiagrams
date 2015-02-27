namespace FsDiagrams

type Point = float * float

// TODO -- angles, unit of measurement? deg, rad

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

type PathDefinition =
    | Line of Point * Point
    | Rect of Point * Point

type LineCap =
    | Round

type Diagram =
    | Path of Path
    | Text
    | Image
    | Use
    | Empty

and IDiagramComponent =
    abstract member AsDiagramComponent : unit -> Diagram

and Path = 
    {
    pathDef : PathDefinition;
    stroke : Color option;
    stroke_width : decimal option;
    stroke_dasharray : int list option;
    stroke_linecap : LineCap option;
    }
    with
        interface IDiagramComponent with
            member this.AsDiagramComponent() = Path this

module Core =

    let inline point x y =
        (float x, float y)

    let inline diagram (cmp:IDiagramComponent) =
        cmp.AsDiagramComponent()

module Paths =

    let path def = 
        {
        pathDef = def
        stroke = Some( NamedColor( KnownColors.Black ) );
        stroke_width = None;
        stroke_dasharray = None;
        stroke_linecap = None;
        }

    let line (source:Point) (dest:Point) =
        PathDefinition.Line(source, dest)

    let lineWidth w path =
        { path with stroke_width = Some(w) }
