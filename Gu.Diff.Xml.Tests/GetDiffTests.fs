module Gu.Diff.Xml.Tests.GetDiff

open System.Xml.Linq
open Xunit
open Gu.Diff.Xml
open System.Collections.Generic

[<Fact>]
let ``check equal declarations returns empty diffs``() = 
    let d1 = XDeclaration("1.0", "utf-8", "")
    let d2 = XDeclaration("1.0", "utf-8", "")
    let actual = GetDiff.forDeclaration d1 d2
    Assert.True(actual.IsNone)

[<Fact>]
let ``check not equal declarations returns diffs``() = 
    let d1 = XDeclaration("1.0", "utf-8", null)
    let d2 = XDeclaration("1.0", "utf-16", null)
    let actual = GetDiff.forDeclaration d1 d2
    Assert.True(actual.IsSome)
    let actual = actual.Value.Diffs
                 |> Seq.exactlyOne |>(fun x -> x:?>IPropertyDiff)
    Assert.Equal(d1.ToString(), actual.First)
    Assert.Equal(d2.ToString(), actual.Other)
    Assert.Equal("Encoding", actual.PropertyName)

[<Fact>]
let ``check equal attributes returns empty diffs``() = 
    let a1 = XAttribute(XName.Get("Value", "x"), 1)
    let a2 = XAttribute(XName.Get("Value", "x"), 1)
    let actual = GetDiff.forAttribute a1 a2
    Assert.True(actual.IsNone)

[<Fact>]
let ``check attributes with different values returns diffs``() = 
    let a1 = XAttribute(XName.Get("Value", "x"), 1)
    let a2 = XAttribute(XName.Get("Value", "x"), 2)
    let actual = GetDiff.forAttribute a1 a2
    Assert.True(actual.IsSome)
    let actual = actual.Value.Diffs
                 |> Seq.exactlyOne |>(fun x -> x:?>IPropertyDiff)
    Assert.Equal(a1.ToString(), actual.First)
    Assert.Equal(a2.ToString(), actual.Other)
    Assert.Equal("Value", actual.PropertyName)

[<Fact>]
let ``attributes with different names returns diffs``() = 
    let a1 = XAttribute(XName.Get("Name1", "x"), 1)
    let a2 = XAttribute(XName.Get("Name2", "x"), 1)
    let actual = GetDiff.forAttribute a1 a2
    Assert.True(actual.IsSome)
    let actual = actual.Value.Diffs
                 |> Seq.exactlyOne |>(fun x -> x:?>IPropertyDiff)
    Assert.Equal(a1.ToString(), actual.First)
    Assert.Equal(a2.ToString(), actual.Other)
    Assert.Equal("Name", actual.PropertyName)
    Assert.Equal("LocalName", actual.Diffs |>Seq.exactlyOne |> (fun x -> (x:?> IPropertyDiff).PropertyName))