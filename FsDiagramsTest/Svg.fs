namespace FsDiagrams.Test.Svg

open FsUnit
open NUnit.Framework

open FsDiagrams.Svg

[<TestFixture>]
type SvgTests() =
    
    [<Test>]
    member this.``Basic document has svg root``() =
        let xdoc = (new SvgDocument()).GetDocument()
        xdoc.Root.Name.LocalName |> should equal "svg"
        xdoc.Root.Name.Namespace.NamespaceName |> should equal @"http://www.w3.org/2000/svg"