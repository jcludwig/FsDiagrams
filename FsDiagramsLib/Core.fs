namespace FsDiagrams

module Core =

    type Node =
        | Point of float * float

    type Path =
        | Line of Node * Node
        | Curve of Node * Node

    let LineTo (source:Node) (dest:Node) : Path =
        Line (source, dest)

    let CurveTo (source:Node) (dest:Node) : Path =
        Curve (source, dest)