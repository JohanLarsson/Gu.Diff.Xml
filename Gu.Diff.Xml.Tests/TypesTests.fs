module Gu.Diff.Xml.Tests.TypesTests
open System.Xml.Linq

open Xunit
open Gu.Diff.Xml
open System.Collections.Generic

[<Fact>]
let ``test XDeclaration equal encoding``()=
    let comparer = PropertyComparer<XDeclaration, string>(<@ fun x -> x.Encoding @>) :> IEqualityComparer<XDeclaration>
    let d1 = XDeclaration("", "utf-8", "")
    let d2 = XDeclaration("", "utf-8", "")
    Assert.True(comparer.Equals(d1, d2))

[<Fact>]
let ``test XDeclaration not equal encoding``()=
    let comparer = PropertyComparer<XDeclaration, string>(<@ fun x -> x.Encoding @>) :> IEqualityComparer<XDeclaration>
    let d1 = XDeclaration("", "utf-8", "")
    let d2 = XDeclaration("", "utf-16", "")
    Assert.False(comparer.Equals(d1, d2))

[<Fact>]
let ``PropertyComparer PropertyInfo``()=
    let comparer = PropertyComparer<XDeclaration, string>(<@ fun x -> x.Encoding @>) :> IPropertyComparer<XDeclaration,string>
    let expected = typeof<XDeclaration>.GetProperty("Encoding")
    Assert.Equal(expected, comparer.PropertyInfo)

