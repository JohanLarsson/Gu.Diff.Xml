module internal Gu.Diff.Xml.Check

open System.Xml.Linq

let getDiffsFor (properties: list<IPropertyEqualityComparer<'t>>) (first: 't) (other: 't) =
    properties |> List.filter (fun x -> not (x.Equals(first, other)))
               |> List.map (fun x -> x.PropertyInfo)

let declarationProperties =
    [ 
        CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Version @>
        CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Encoding @>
        CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Standalone @>
    ]
             
let checkDeclaration (first: XDeclaration) (other: XDeclaration) = 
    getDiffsFor declarationProperties first other
    |> CreateDiff.forDeclaration first other
    
let nameProperties =
        [ 
            CreateEqualityComparer.ForProperty<XName> <@ fun x -> x.NamespaceName @>
            CreateEqualityComparer.ForProperty<XName> <@ fun x -> x.LocalName @>
        ]

let attributeProperties =
        [ 
            CreateEqualityComparer.ForProperty<XAttribute> <@ fun x -> x.Value @>
        ]

let checkAttribute (first: XAttribute) (other: XAttribute) = 
    let nameDiffs = getDiffsFor nameProperties first.Name other.Name
                    |> CreateDiff.forName first other <@ fun x -> x.Name @>
                    |> Option.map (fun x -> x:> IDiffNode)
    let propDiffs = getDiffsFor attributeProperties first other
    CreateDiff.forAttribute first other nameDiffs propDiffs

 
//let rec checkElement first other =
//    first.   
//
let checkDocument (first: XDocument) (other: XDocument) =
    { new IDocumentDiff with
          member x.First = first.ToString()
          member x.Other = other.ToString()
          member x.Diffs = Seq.empty
    }

