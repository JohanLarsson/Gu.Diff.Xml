module Gu.Diff.Xml.Tests.CheckTests

open System.Xml.Linq
open Xunit
open Gu.Diff.Xml
open System.Collections.Generic

[<Fact>]
let ``check equal declarations returns empty diffs``() = 
    let d1 = XDeclaration("1.0", "utf-8", "")
    let d2 = XDeclaration("1.0", "utf-8", "")
    let actual = Check.checkDeclarations d1 d2
    Assert.Empty(actual)

[<Fact>]
let ``check not equal declarations returns diffs``() = 
    let d1 = XDeclaration("1.0", "utf-8", "")
    let d2 = XDeclaration("1.0", "utf-16", "")
    let actual = Check.checkDeclarations d1 d2
    let expected = Create.Diff d1 d2 (typeof<XDeclaration>.GetProperty("Encoding"))
    Assert.Single(actual, expected)
