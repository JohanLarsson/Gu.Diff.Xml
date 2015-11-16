module internal Gu.Diff.Xml.Check

open System.Xml.Linq

let declarationProperties : IPropertyComparer<XDeclaration>[] =
        [| 
            Create.PropertyComparer <@ fun (x:XDeclaration) -> x.Version @>
            Create.PropertyComparer <@ fun (x:XDeclaration) -> x.Encoding @>
            Create.PropertyComparer <@ fun (x:XDeclaration) -> x.Standalone @>
        |]

let check (properties: seq<IPropertyComparer<'t>>) (first: 't) (other: 't) =
    properties 
    |> Seq.filter (fun x -> not (x.Equals(first, other)))
    |> Seq.map (fun x -> { First= first; Other= other; Property= x.PropertyInfo })
      
let checkDeclarations first other = check declarationProperties first other

let all first other =
    seq{
        yield checkDeclarations first other
    }
