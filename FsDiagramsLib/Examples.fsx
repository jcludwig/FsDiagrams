#r "System.Xml.Linq.dll"

#load "Core.fs"
#load "Svg.fs"

open FsDiagrams
open FsDiagrams.Core
open FsDiagrams.Svg

open FsDiagrams.Paths

(*
let doc = {
    Elements = [ GraphicsElement( Line( { LineData.Default with source = point (100, 10); dest = point (110, 120); stroke = Some( NamedColor( KnownColors.Red ) ) } ) ) ]
}
*)

let l = line (point 10.0 10.0) (point 100.0 100.0)

let d1 = 
    l
    |> Lines.path
    |> pathWidth 10M
    |> pathToDiagram

let d2 =
    match l with
    | Line (p1, p2) -> rectByPoints p1 p2
    |> Rects.path
    |> pathWidth 5M
    |> pathColor KnownColors.Green
    |> fillColor KnownColors.Red
    |> pathToDiagram
    
let d3 = overlay [d2; d1] |> Diagram.Compound
let xdoc = SvgWriter.writeDiagram d3
let svgFile = @"c:\tmp\test.svg" //System.IO.Path.GetTempFileName( ) + ".svg"
xdoc.Save( svgFile )

//System.Diagnostics.Process.Start(svgFile)
