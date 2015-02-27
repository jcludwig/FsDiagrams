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

type IPathLike =
    abstract member AsPathDefinition : unit -> PathDefinition

and PathDefinition =
    | Line of Line
    | Rect of Rect
    with
    interface IPathLike with
        member this.AsPathDefinition() = this

and Line = Line of Point * Point
    with
    interface IPathLike with
        member this.AsPathDefinition() = PathDefinition.Line this

and Rect = Rect of Point * Point
    with
    interface IPathLike with
        member this.AsPathDefinition() = PathDefinition.Rect this

type LineCap =
    | Round

type IDiagramComponent =
    abstract member AsDiagramComponent : unit -> Diagram

and Diagram =
    | Path of Path
    | Text
    | Image
    | Use
    | Empty
    | Compound of CompoundDiagram
    with
    interface IDiagramComponent with
        member this.AsDiagramComponent() = this

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

and CompoundDiagram =
    | Overlay of Diagram list
    with
    interface IDiagramComponent with
        member this.AsDiagramComponent() = Compound this

module Core =

    let inline point x y =
        (float x, float y) : Point

    let inline diagram (cmp:IDiagramComponent) =
        cmp.AsDiagramComponent()

    let overlay diagrams =
        Overlay diagrams

module Paths =

    let path (def:IPathLike) = 
        {
        pathDef = def.AsPathDefinition()
        stroke = Some( NamedColor( KnownColors.Black ) );
        stroke_width = None;
        stroke_dasharray = None;
        stroke_linecap = None;
        }

    let line (source:Point) (dest:Point) =
        Line.Line (source, dest)

    let rect (topleft:Point) (bottomright:Point) =
        Rect.Rect (topleft, bottomright)

    let lineWidth w path =
        { path with stroke_width = Some(w) }
