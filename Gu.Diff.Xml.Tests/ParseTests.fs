module Gu.Diff.Xml.Tests.ParseTests
open System.Xml.Linq
open System
open System.Xml;
open Xunit
open Gu.Diff.Xml
let fail =
    Assert.True(false)

//[<Fact>]
//let ``parse valid xml returns XDocument``() = 
//    match Compare.Parse.text "<Foo />" with
//    |Compare.Doc(Document = doc) -> Assert.Equal(doc.Document.ToString(), "<Foo />")
//    |_ -> fail
//
//[<Fact>]
//let ``parse invalid xml returns Exception``() = 
//    match Compare.Parse.text "<Foo>" with
//    |Compare.Ex(Exception = ex) -> Assert.Equal("", ex.Message)
//    |_ -> fail
