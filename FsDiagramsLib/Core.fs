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

type Path =
    | Line of Point * Point
    | Rect of Point * Point

type LineCap =
    | Round

type DecoratedPath = 
    {
    path : Path;
    stroke : Color option;
    stroke_width : decimal option;
    stroke_dasharray : int list option;
    stroke_linecap : LineCap option;
    }

type GraphicsElement =
    | Path of DecoratedPath
    | Text
    | Image
    | Use

type Element =
    | GraphicsElement of GraphicsElement

type Diagram = 
    {
    Elements : Element list
    }
    static member (+) (left:Diagram, right:Diagram) =
        ()

module Core =

    let inline point x y =
        (float x, float y)

    let toPath path = 
        {
        path = path
        stroke = Some( NamedColor( KnownColors.Black ) );
        stroke_width = None;
        stroke_dasharray = None;
        stroke_linecap = None;
        }

    let line (source:Point) (dest:Point) =
        Path.Line(source, dest)

    let lineWidth w line =
        { line with stroke_width = Some(w) }

    let diagram elements =
        { Elements = elements } : Diagram

    let appendElement diagram element =
        { diagram with Elements = element :: diagram.Elements }

