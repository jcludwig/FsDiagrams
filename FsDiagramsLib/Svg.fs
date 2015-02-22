namespace FsDiagrams.Svg

open System.Xml.Linq
open System.IO

open FsDiagrams

type SvgDocument() =
    let xdoc = new XDocument()
    let svgns = XNamespace.Get(@"http://www.w3.org/2000/svg")
    let svgroot = xdoc.Add(new XElement(svgns + "svg"))

    member public this.GetDocument() =
        xdoc