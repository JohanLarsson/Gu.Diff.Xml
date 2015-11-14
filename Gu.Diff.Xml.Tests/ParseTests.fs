module Gu.Diff.Xml.Tests.ParseTests
open System.Xml.Linq
open System
open Xunit
open Gu.Diff.Xml

[<Fact>]
let ``parse valid xml returns XDocAndText``() = 
    let result = Compare.Parse.text "<Foo />"
    match result with
    |Compare.XDocAndXml(Xml = xml) -> Assert.Equal(xml, "<Foo />")
    |_ -> Assert.True(false)
