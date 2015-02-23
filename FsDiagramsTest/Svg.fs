namespace FsDiagrams.Test.Svg

open FsUnit
open Microsoft.VisualStudio.TestTools.UnitTesting

open FsDiagrams.Svg

[<TestClass>]
type SvgTests() =
    
    [<TestMethod>]
    member this.``Basic document has svg root``() =
        let xdoc = (new SvgWriter()).GetDocument()
        xdoc.Root.Name.LocalName |> should equal "svg"
        xdoc.Root.Name.Namespace.NamespaceName |> should equal @"http://www.w3.org/2000/svg"

[<TestClass>]
type ColorTests() =

    [<TestMethod>]
    member this.``NamedColor ToString``() =
        let color = Color.NamedColor("red")
        color.ToString() |> should equal "red"
        
    [<TestMethod>]
    member this.``HexRgb ToString``() =
        let color = Color.HexRgb("#ff00aa")
        color.ToString() |> should equal "#ff00aa"

    [<TestMethod>]
    member this.``Rgb ToString``() =
        let color = Color.Rgb(255uy, 0uy, 128uy)
        color.ToString() |> should equal "rgb(255, 0, 128)"

    [<TestMethod>]
    member this.``RgbPercentage ToString``() =
        let color = Color.RgbPercentage(100M, 0.0M, 50.501M)
        color.ToString() |> should equal "rgb(100%, 0.0%, 50.501%)"

[<TestClass>]
type LineCapTests() =

    [<TestMethod>]
    member this.``Round ToString``() =
        let linecap = LineCap.Round
        linecap.ToString() |> should equal "round"