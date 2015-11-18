module internal Gu.Diff.Xml.GetDiff

open System.Xml.Linq

module internal Properties =
    let forDeclaration =
        [ 
            CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Version @>
            CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Encoding @>
            CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Standalone @>
        ]

    let forName =
            [ 
                CreateEqualityComparer.ForProperty<XName> <@ fun x -> x.NamespaceName @>
                CreateEqualityComparer.ForProperty<XName> <@ fun x -> x.LocalName @>
            ]

    let forAttribute =
            [ 
                CreateEqualityComparer.ForProperty<XAttribute> <@ fun x -> x.Value @>
            ]

let forProperties (properties: list<IPropertyEqualityComparer<'t>>) (first: 't) (other: 't) =
    properties |> List.filter (fun x -> not (x.Equals(first, other)))
               |> List.map (fun x -> x.PropertyInfo)
             
let forDeclaration (first: XDeclaration) (other: XDeclaration) = 
    forProperties Properties.forDeclaration first other
    |> CreateDiff.forDeclaration first other

let forAttribute (first: XAttribute) (other: XAttribute) = 
    let nameDiffs = forProperties Properties.forName first.Name other.Name
                    |> CreateDiff.forName first other <@ fun x -> x.Name @>
                    |> Option.map (fun x -> x:> IDiffNode)
    let propDiffs = forProperties Properties.forAttribute first other
    CreateDiff.forAttribute first other nameDiffs propDiffs

 
//let rec checkElement first other =
//    first.   
//
let forDocument (first: XDocument) (other: XDocument) =
    { new IDocumentDiff with
          member x.First = first.ToString()
          member x.Other = other.ToString()
          member x.Diffs = Seq.empty
    }

