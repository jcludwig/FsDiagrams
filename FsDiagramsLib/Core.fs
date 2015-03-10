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

type Line = Line of Point * Point

type Rect = Rect of Point * Size

type Color =
    | NamedColor of string
    | HexRgb of string
    | Rgb of byte * byte * byte
    | RgbPercentage of decimal * decimal * decimal

module KnownColors =
    let Red = NamedColor "red"
    let Green = NamedColor "green"
    let Blue = NamedColor "blue"
    let Black = NamedColor "black"

type LineCap =
    | Round

type StrokeAttributes =
    {
    Color : Color option;
    Width : decimal option;
    DashArray : int list option;
    LineCap : LineCap option;
    }
    with
        static member DefaultWidth = 0M
        static member Default = 
            {
            Color = Some(KnownColors.Black);
            Width = Some(1M);
            DashArray = None;
            LineCap = None;
            }

type Path =
    | Line of Line
    | Rect of Rect
    // shapes, polyline, etc

type Diagram =
    | Path of Path
    | Text
    | Image
    | Use
    | Empty
    | Compound of CompoundDiagram
    
and CompoundDiagram =
    | Overlay of Diagram list
    
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

    let overlay diagrams =
        Overlay diagrams

module Paths =

    let path pathDef =
        { pathDef = pathDef; strokeAttrs = StrokeAttributes.Default }

    let line (source:Point) (dest:Point) =
        Line.Line (source, dest)

    let rect topleft size =
        Rect.Rect (topleft, size)

    let rectByPoints (topleft:Point) (bottomright:Point) =
        let size = bottomright - topleft
        rect topleft size

    let pathWithStrokeAttrs path attrs =
        new Path(attrs)

    let pathWidth w path =
        pathWithStrokeAttrs path { path.StrokeAttributes with Width = Some w }

    let pathColor color path =
        pathWithStrokeAttrs path { path.strokeAttrs with Color = Some color }

    let fillColor color closedPath =
        pathWithStrokeAttrs path { path.strokeAttrs with }

    let pathToDiagram path =
        Diagram.Path path

module Lines =

    let path line =
        Paths.path (LinePath line)

module Rects =
    
    let path rect = 
        Paths.path (RectPath rect)