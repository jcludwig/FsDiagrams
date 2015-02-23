#r "System.Xml.Linq.dll"

#load "Core.fs"
#load "Svg.fs"

open FsDiagrams
open FsDiagrams.Svg

let doc = {
    Elements = [ GraphicsElement( Line( { LineData.Default with x1 = 100; y1 = 10; x2 = 110; y2 = 120; stroke = Some( NamedColor( KnownColors.Red ) ) } ) ) ]
}

let writer = SvgWriter( )
writer.WriteDocument(doc)
let svgFile = @"c:\tmp\test.svg" //System.IO.Path.GetTempFileName( ) + ".svg"
writer.GetDocument( ).Save( svgFile )

//System.Diagnostics.Process.Start(svgFile)
