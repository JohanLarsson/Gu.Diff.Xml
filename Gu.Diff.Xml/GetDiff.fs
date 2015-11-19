module internal Gu.Diff.Xml.GetDiff

open System.Xml.Linq

module internal Properties = 
    let forDeclaration = 
        [ CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Version @>
          CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Encoding @>
          CreateEqualityComparer.ForProperty<XDeclaration> <@ fun x -> x.Standalone @> ]
    
    let forName = 
        [ CreateEqualityComparer.ForProperty<XName> <@ fun x -> x.NamespaceName @>
          CreateEqualityComparer.ForProperty<XName> <@ fun x -> x.LocalName @> ]
    
    let forAttribute = [ CreateEqualityComparer.ForProperty<XAttribute> <@ fun x -> x.Value @> ]
    let forElement = [ CreateEqualityComparer.ForProperty<XElement> <@ fun x -> x.Value @> ]

let forProperties (properties : list<IPropertyEqualityComparer<'t>>) (first : option<'t>) (other : option<'t>) = 
    let asValueOrNull x = 
        match x with
        | Some x -> x
        | None -> null
    properties
    |> List.filter (fun x -> not (x.Equals(first |> asValueOrNull, other |> asValueOrNull)))
    |> List.map (fun x -> x.PropertyInfo)

let forNameProperty first other f = 
    let firstName = first |> Option.map f
    let otherName = other |> Option.map f
    forProperties Properties.forName firstName otherName
    |> CreateDiff.forProperties first other
    |> CreateDiff.forName firstName otherName

let forDeclaration (first : option<XDeclaration>) (other : option<XDeclaration>) = 
    forProperties Properties.forDeclaration first other 
    |> List.map (CreateDiff.forProperty first other)
    |> CreateDiff.forDeclaration first other

let forAttribute (first : option<XAttribute>) (other : option<XAttribute>) : option<IAttributeDiff>= 
    let nameDiff = forNameProperty first other (fun x -> x.Name)
    let propDiffs = forProperties Properties.forAttribute first other
                    |> List.map (fun x -> CreateDiff.forProperty first other x)
    CreateDiff.forAttribute first other nameDiff propDiffs

let forAttributes (first : option<XElement>) (other : option<XElement>) = 
    let getAttributes (e : option<XElement>) =
        match e with
        |None -> Seq.empty
        |Some e -> e.Attributes()

    Seq.map2All forAttribute (getAttributes first) (getAttributes other)

//let rec forElement (first : option<XElement>) (other : option<XElement>) = 
//    let nameDiffs = forNameProperty first other (fun x -> x.Name)
//    
//    let attributeDiffs = forAttributes first other
//    // check if both are leaf elements, if so compare value
//    let propDiffs = forProperties Properties.forElement first other
//    CreateDiff.forElement first other nameDiffs propDiffs
//
//let forDocument (first : XDocument) (other : XDocument) = 
//    { new IDocumentDiff with
//          member x.First = first.ToString()
//          member x.Other = other.ToString()
//          member x.Diffs = Seq.empty }
