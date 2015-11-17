module Gu.Diff.Xml.Tests.CheckTests

open System.Xml.Linq
open Xunit
open Gu.Diff.Xml
open System.Collections.Generic

[<Fact>]
let ``check equal declarations returns empty diffs``() = 
    let d1 = XDeclaration("1.0", "utf-8", "")
    let d2 = XDeclaration("1.0", "utf-8", "")
    let actual = Check.checkDeclaration d1 d2
    Assert.True(actual.IsNone)

[<Fact>]
let ``check not equal declarations returns diffs``() = 
    let d1 = XDeclaration("1.0", "utf-8", null)
    let d2 = XDeclaration("1.0", "utf-16", null)
    let actual = Check.checkDeclaration d1 d2
    Assert.True(actual.IsSome)
    let actual = actual.Value.Diffs |> Seq.cast<IPropertyDiff> |> Seq.exactlyOne
    Assert.Equal(d1.ToString(), actual.First)
    Assert.Equal(d2.ToString(), actual.Other)
    Assert.Equal("Encoding", actual.PropertyName)
