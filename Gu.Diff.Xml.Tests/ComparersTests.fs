module Gu.Diff.Xml.Tests.ComparersTests
open System.Xml.Linq

open Xunit
open Gu.Diff.Xml
open System.Collections.Generic

[<Fact>]
let ``test XDeclaration equal encoding``()=
    let comparer = CreateEqualityComparer.ForProperty <@ fun (x:XDeclaration) -> x.Encoding @>
    let d1 = XDeclaration("", "utf-8", "")
    let d2 = XDeclaration("", "utf-8", "")
    Assert.True(comparer.Equals(d1, d2))

[<Fact>]
let ``test XDeclaration equal encoding using factory``()=
    let comparer = CreateEqualityComparer.ForProperty <@ fun (x:XDeclaration) -> x.Encoding @>
    let d1 = XDeclaration("", "utf-8", "")
    let d2 = XDeclaration("", "utf-8", "")
    Assert.True(comparer.Equals(d1, d2))

[<Fact>]
let ``test XDeclaration not equal encoding``()=
    let comparer = CreateEqualityComparer.ForProperty <@ fun (x:XDeclaration) -> x.Encoding @>
    let d1 = XDeclaration("", "utf-8", "")
    let d2 = XDeclaration("", "utf-16", "")
    Assert.False(comparer.Equals(d1, d2))

[<Fact>]
let ``PropertyComparer PropertyInfo``()=
    let comparer = CreateEqualityComparer.ForProperty <@ fun (x:XDeclaration) -> x.Encoding @>
    let expected = typeof<XDeclaration>.GetProperty("Encoding")
    Assert.Equal(expected, comparer.PropertyInfo)
