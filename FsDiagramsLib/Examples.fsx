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

let d = 
    line (point 10.0 10.0) (point 100.0 100.0)
    |> path
    |> lineWidth 10M
    |> diagram

let xdoc = SvgWriter.writeDiagram d
let svgFile = @"c:\tmp\test.svg" //System.IO.Path.GetTempFileName( ) + ".svg"
xdoc.Save( svgFile )

//System.Diagnostics.Process.Start(svgFile)
