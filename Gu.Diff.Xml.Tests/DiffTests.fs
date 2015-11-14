module Gu.Diff.Xml.Tests.DiffTests
open System.Xml.Linq
open System
open Xunit
open Gu.Diff.Xml

type DiffDocument = 
    | XDocAndXml of Document : XDocument *  Xml : string
    | ExcpetionAndXml of Excption : Exception * Xml : string

let parse xml =
    try XDocAndXml(XDocument.Parse xml, xml)
    with ex -> ExcpetionAndXml (ex, xml)

[<Fact>]
let ``parse valid xml returns XDocAndText``() = 
    let result = parse "<Foo />"
    match result with
    |XDocAndXml(Xml = xml) -> Assert.Equal(xml, "<Foo />")
    |_ -> Assert.True(false)
