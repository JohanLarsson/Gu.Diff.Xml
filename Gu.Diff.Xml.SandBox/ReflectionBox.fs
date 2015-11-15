module Gu.Diff.Xml.SandBox.ReflectionBox
open Microsoft.FSharp.Quotations
open System.Reflection
open System.Linq
open System.Xml.Linq;
open Xunit
open System.Collections

type Diff<'a> = 
    { First : 'a
      Other : 'a
      Property : PropertyInfo }

let getProperty<'a> name =
    typeof<'a>.GetProperty(name)

let rec property = function
    | Patterns.PropertyGet (_, propertyInfo, _) -> propertyInfo
    | x -> failwithf "Unexpected input: %s" (x.ToString())

let diffProp<'t> (first:'t) (other:'t) (prop: PropertyInfo) =
    seq {
        if(prop.GetValue(first) <> prop.GetValue(other)) 
            then yield {First = first; Other= other; Property = prop}
    }

[<Fact>]
let ``diff strings``() = 
    let check = diffProp "ab" "b" 
    let diffs = check (property <@ "".Length @>)
    Assert.Equal("Length" , diffs.Single().Property.Name)

[<Fact>]
let ``get property``() = 
    let prop = getProperty<string> "Length"
    Assert.Equal(prop.Name, "Length")

//[<Fact>]
//let ``declaration``() = 
//    let d1 = XDeclaration("version", "encoding", "standalone")
//    let d2 = XDeclaration("version", "encoding", "standalone")
//    Assert.Equal(declaration.Name, "Length")
