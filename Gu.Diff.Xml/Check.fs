module internal Gu.Diff.Xml.Check

open System.Xml.Linq


let check (properties: seq<IPropertyEqualityComparer<'t>>) (first: 't) (other: 't) =
    properties 
    |> Seq.filter (fun x -> not (x.Equals(first, other)))
    |> Seq.map (fun x -> CreateDiff.For first other x.PropertyInfo)

let declarationProperties : IPropertyEqualityComparer<XDeclaration>[] =
        [| 
            CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Version @>
            CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Encoding @>
            CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Standalone @>
        |]
      
let checkDeclaration first other = check declarationProperties first other

let attributeProperties : IPropertyEqualityComparer<XAttribute>[] =
        [| 
//            CreateEqualityComparer.ForProperty<XAttribute> <@ fun x -> x.Name @>
            CreateEqualityComparer.ForProperty<XAttribute> <@ fun x -> x.Value @>
        |]

let checkAttribute first other = check attributeProperties first other

//let checkAttributes first other =
//    first.
    

let all first other =
    seq{
        yield checkDeclaration first other
//        yield checkAttributes first other
    }
