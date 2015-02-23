namespace FsDiagrams.Test.Svg

open FsUnit
open Microsoft.VisualStudio.TestTools.UnitTesting
open NUnit.Framework.Constraints
open System.Xml.Linq

open FsDiagrams.Svg
open FsDiagrams.Svg.Utils

module Utils =

    let attributeEquals (el:XElement) (name:string) (value:string) =
        match el.Attribute(xname name) with
            | null -> false
            | a -> a.Value = value

open Utils

module Seq =

    let all f sources =
        sources |> Seq.exists (fun x -> not (f x)) |> not

module FsUnitEx =

    type HasAttributesConstraint( attrs:(string * string) list ) =
        inherit Constraint()
        let mutable actual : obj = null

        override this.Matches( actual:obj ) : bool =
            this.actual <- actual

            match actual with
                | :? System.Xml.Linq.XElement as el ->
                    attrs |> Seq.all (fun (name, value) -> attributeEquals el name value)
                | _ -> false

        override this.WriteDescriptionTo( writer:MessageWriter ) =
            writer.Write( "attribute(s) {0}", attrs |> Seq.map (fun (name, value) -> sprintf "%s=%s" name value) |> String.concat ", " )
    
    let haveAttribute attr =
        AndConstraint( InstanceOfTypeConstraint( typeof<XElement>), HasAttributesConstraint( [ attr ] ) )

    let haveAttributes attrs =
        AndConstraint( InstanceOfTypeConstraint( typeof<XElement>), HasAttributesConstraint( attrs ) )

open FsUnitEx

[<TestClass>]
type SvgTests() =
    
    [<TestMethod>]
    member this.``Basic document has svg root``() =
        let xdoc = (new SvgWriter()).GetDocument()
        xdoc.Root.Name.LocalName |> should equal "svg"
        xdoc.Root.Name.Namespace.NamespaceName |> should equal @"http://www.w3.org/2000/svg"

    [<TestMethod>]
    member this.``Write Line``() =
        let writer = new SvgWriter()
        let doc = {
            Elements = [ GraphicsElement(Line({ LineData.Default with x1 = 1; y1 = 2; x2 = 3; y2 = 4 })) ]
        }
        writer.WriteDocument(doc)
        let xdoc = writer.GetDocument()
        let line = xdoc.Root.Element(xname "line")
        line |> should haveAttributes [ ("x1", "1"); ("y1", "2"); ("x2", "3"); ("y2", "4") ]

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