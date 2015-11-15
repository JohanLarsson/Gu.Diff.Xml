module internal Gu.Diff.Xml.Check

open Microsoft.FSharp.Linq
open System
open System.Reflection
open System.Xml.Linq
open System.Collections.Generic

let checkDeclarations first other =
    let properties =
            [| 
                PropertyComparer<XDeclaration, string>(<@ fun x -> x.Version @>) :> IPropertyComparer<XDeclaration, string>
                PropertyComparer<XDeclaration, string>(<@ fun x -> x.Encoding @>) :> IPropertyComparer<XDeclaration, string>
                PropertyComparer<XDeclaration, string>(<@ fun x -> x.Standalone @>) :> IPropertyComparer<XDeclaration, string>
            |]
    properties 
    |> Seq.filter (fun x -> x.Equals(first, other))
    |> Seq.map (fun x -> {First= first; Other= other; Property= x.PropertyInfo})

let all first other =
    Seq.empty
//    |> Seq.append declarations first other