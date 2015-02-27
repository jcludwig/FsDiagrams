namespace FsDiagrams

type Size = Size of float * float

type Point = Point of float * float
    with
    static member (+) (left:Point, right:Point) =
        let (Point (x1, y1)) = left
        let (Point (x2, y2)) = right
        Point (x1 + x2, y1 + y2)
    
    static member (-) (left:Point, right:Point) =
        let (Point (x1, y1)) = left
        let (Point (x2, y2)) = right
        Size (x1 - x2, y1 - y2)

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
    | LinePath of Line
    | RectPath of Rect
    with
    interface IPathLike with
        member this.AsPathDefinition() = this

and Line = Line of Point * Point
    with
    interface IPathLike with
        member this.AsPathDefinition() = LinePath this

and Rect = Rect of Point * Size
    with
    interface IPathLike with
        member this.AsPathDefinition() = RectPath this

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
        Point (float x, float y)

    let coords point =
        let (Point (x, y)) = point
        (x, y)

    let dims size =
        let (Size (w, h)) = size
        (w, h)

    let inline size w h =
        Size (float w, float h)

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

    let rect topleft size =
        Rect (topleft, size)

    let rectByPoints (topleft:Point) (bottomright:Point) =
        let size = bottomright - topleft
        rect topleft size

    let lineWidth w path =
        { path with stroke_width = Some(w) }
