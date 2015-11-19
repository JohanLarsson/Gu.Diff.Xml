module Gu.Diff.Xml.Tests.GetDiff

open System.Xml.Linq
open Xunit
open Gu.Diff.Xml
open System.Collections.Generic

[<Fact>]
let ``check equal declarations returns empty diffs``() = 
    let d1 = Some(XDeclaration("1.0", "utf-8", ""))
    let d2 = Some(XDeclaration("1.0", "utf-8", ""))
    let actual = GetDiff.forDeclaration d1 d2
    Assert.True(actual.IsNone)

[<Fact>]
let ``check not equal declarations returns diffs``() = 
    let d1 = Some(XDeclaration("1.0", "utf-8", null))
    let d2 = Some(XDeclaration("1.0", "utf-16", null))
    let actual = GetDiff.forDeclaration d1 d2
    Assert.True(actual.IsSome)
    let actual = 
        actual.Value.Diffs
        |> Seq.exactlyOne
        |> (fun x -> x :?> IPropertyDiff)
    Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?>", actual.First)
    Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-16\"?>", actual.Other)
    Assert.Equal("Encoding", actual.PropertyName)

[<Fact>]
let ``check equal attributes returns empty diffs``() = 
    let a1 = Some(XAttribute(XName.Get("Value", "x"), 1))
    let a2 = Some(XAttribute(XName.Get("Value", "x"), 1))
    let actual = GetDiff.forAttribute a1 a2
    Assert.True(actual.IsNone)

[<Fact>]
let ``check attributes with different values returns diffs``() =
    let a1 = Some(XAttribute(XName.Get("Value", "x"), 1))
    let a2 = Some(XAttribute(XName.Get("Value", "x"), 2))
    let actual = GetDiff.forAttribute a1 a2
    Assert.True(actual.IsSome)
    let actual = 
        actual.Value.Diffs
        |> Seq.exactlyOne
        |> (fun x -> x :?> IPropertyDiff)
    Assert.Equal("p0:Value=\"1\"", actual.First)
    Assert.Equal("p0:Value=\"2\"", actual.Other)
    Assert.Equal("Value", actual.PropertyName)

[<Fact>]
let ``attributes with different names returns diffs``() = 
    let a1 = Some(XAttribute(XName.Get("Name1"), 1))
    let a2 = Some(XAttribute(XName.Get("Name2"), 1))
    let actual = GetDiff.forAttribute a1 a2
    Assert.True(actual.IsSome)
    let actual = 
        actual.Value.Diffs
        |> Seq.exactlyOne
        |> (fun x -> x :?> IPropertyDiff)
    Assert.Equal("Name1", actual.First)
    Assert.Equal("Name2", actual.Other)
    Assert.Equal("Name", actual.PropertyName)
    Assert.Equal("LocalName", 
                 actual.Diffs
                 |> Seq.exactlyOne
                 |> (fun x -> (x :?> IPropertyDiff).PropertyName))
