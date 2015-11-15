namespace Gu.Diff.Xml.SandBox
module NameOf = 
    open Microsoft.FSharp.Quotations
    let rec Method = function
        | Patterns.Call(None, methodInfo, _) -> methodInfo.Name
        | Patterns.Lambda(_, expr) -> Method expr
        | x -> failwithf "Unexpected input: %s" (x.ToString())
    
    let rec Property = function
        | Patterns.PropertyGet (_, propertyInfo, _) -> propertyInfo.Name
        | Patterns.Lambda(_, expr) -> Property expr
        | x -> failwithf "Unexpected input: %s" (x.ToString())

module NameOf_Tests =
    open Xunit
    type Dummy = { Value: int; Name: string }
    let DummyMethod () = ()

    [<Fact>]
    let ``name of method``()= 
        Assert.Equal("DummyMethod", NameOf.Method <@ DummyMethod @>)
        Assert.Equal("DummyMethod", NameOf.Method <@fun () -> DummyMethod @>)
    
    let props = 
        [| 
            fun (x:Dummy) -> box (x.Name)
            fun (x:Dummy) -> box (x.Value)
        |]

    [<Fact>]
    let ``name of property``()= 
        let dummy = { Value = 1; Name = "dk" }
        Assert.Equal("Value", NameOf.Property <@ dummy.Value @>)
        Assert.Equal("Length", NameOf.Property <@ dummy.Name.Length @>)
