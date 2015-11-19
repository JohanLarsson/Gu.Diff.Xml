namespace Gu.Diff.Xml

open System.Collections.Generic

type IDiffNode = 
    abstract First : string
    abstract Other : string
    abstract Diffs : IEnumerable<IDiffNode>

type IPropertyDiff = 
    inherit IDiffNode
    abstract PropertyName : string

type IDeclarationDiff = 
    inherit IDiffNode

type INameDiff = 
    inherit IDiffNode

type IAttributeDiff = 
    inherit IDiffNode

type IElementDiff = 
    inherit IDiffNode

//    abstract member AttributeDiffs : IEnumerable<IAttributeDiff> 
//    abstract member ElementDiffs : IEnumerable<IElementDiff> 
type IDocumentDiff = 
    inherit IDiffNode

module internal CreateDiff = 
    open System.Reflection
    open System.Xml.Linq
    open Microsoft.FSharp.Quotations
    let inline stringOrEmpty o =
        match o with 
        |None -> System.String.Empty
        |Some x -> string x

    let inline forProperty first other (prop : PropertyInfo) = 
        { new IPropertyDiff with
          member __.First = stringOrEmpty first
          member __.Other = stringOrEmpty other
          member __.PropertyName = prop.Name
          member __.Diffs = Seq.empty }
    
    let inline forProperties first other (props : list<PropertyInfo>) : list<IPropertyDiff> = 
        props 
        |> List.map (fun x -> forProperty first other x)
    
    let forDeclaration (first : option<XDeclaration>) 
                       (other : option<XDeclaration>) 
                       (props : list<IPropertyDiff>)
                       : option<IDeclarationDiff> = 
        match props with
        | [] -> None
        | x -> 
            Some { new IDeclarationDiff with
                   member __.First = stringOrEmpty first
                   member __.Other = stringOrEmpty other
                   member __.Diffs = props |> Seq.cast<IDiffNode> }
    
    let forAttribute (first : option<XAttribute>) 
                     (other : option<XAttribute>) 
                     (nameDiff : option<IPropertyDiff>) 
                     (props : list<IPropertyDiff>)
                     : option<IAttributeDiff> = 
        let diffs = match nameDiff, props with
                    |None, [] -> []
                    |Some x, [] -> [x]
                    |Some x, list -> x :: list
                    |None, props -> props

        match diffs with
        |[] -> None
        |x -> Some { new IAttributeDiff with
                     member __.First = stringOrEmpty first
                     member __.Other = stringOrEmpty other
                     member __.Diffs = diffs |> Seq.cast<IDiffNode> }
    
    let forName (first : option<XName>) 
                (other : option<XName>) 
                (props : list<IPropertyDiff>)
                : option<IPropertyDiff> = 
        match props with
        |[] -> None
        |props -> Some { new IPropertyDiff with
                         member __.First = stringOrEmpty first
                         member __.Other = stringOrEmpty other 
                         member __.PropertyName = "Name"
                         member __.Diffs = props |> Seq.cast<IDiffNode> }

//    let For (first: XElement) (other: XElement) (prop: PropertyInfo) = 
//        { new IElementDiff with
//            member x.First = first |> stringOrEmpty
//            member x.Other = other |> stringOrEmpty
//            member x.PropertyName = prop.Name }
