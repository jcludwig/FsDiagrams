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
    |> path
    |> lineWidth 10M
    |> diagram

let d2 =
    match l with
    | Line (p1, p2) -> rect p1 p2
    |> path
    |> lineWidth 5M
    |> diagram
    
let d3 = overlay [d1; d2] |> diagram
let xdoc = SvgWriter.writeDiagram d3
let svgFile = @"c:\tmp\test.svg" //System.IO.Path.GetTempFileName( ) + ".svg"
xdoc.Save( svgFile )

//System.Diagnostics.Process.Start(svgFile)
