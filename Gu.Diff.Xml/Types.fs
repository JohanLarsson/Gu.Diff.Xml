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
    
    let forProperty first other (prop : PropertyInfo) = 
        { new IPropertyDiff with
              member __.First = first.ToString()
              member __.Other = other.ToString()
              member __.PropertyName = prop.Name
              member __.Diffs = Seq.empty }
    
    let forProperties first other (props : seq<PropertyInfo>) : seq<IPropertyDiff> = 
        props |> Seq.map (fun x -> forProperty first other x)
    
    let forDeclaration first other props = 
        match props with
        | [] -> None
        | x -> 
            Some { new IDeclarationDiff with
                       member __.First = first.ToString()
                       member __.Other = other.ToString()
                       member __.Diffs = (forProperties first other props) |> Seq.cast<IDiffNode> }
    
    let forAttribute first other nameDiffs props = 
        match nameDiffs, props with
        | None, [] -> None
        | Some nameDiff, [] -> 
            Some { new IAttributeDiff with
                       member __.First = first.ToString()
                       member __.Other = other.ToString()
                       member __.Diffs = Seq.singleton nameDiff }
        | Some nameDiff, props -> 
            Some 
                { new IAttributeDiff with
                      member __.First = first.ToString()
                      member __.Other = other.ToString()
                      member __.Diffs = Seq.singleton nameDiff 
                                        |> Seq.append(forProperties first other props 
                                                      |> Seq.cast<IDiffNode>) }
        | None, props -> 
            Some 
                { new IAttributeDiff with
                      member __.First = first.ToString()
                      member __.Other = other.ToString()
                      member __.Diffs = forProperties first other props |> Seq.cast<IDiffNode> }
    
    let forName<'t> (first : 't) (other : 't) (prop : Expr<'t -> XName>) (props : seq<PropertyInfo>) = 
        if Seq.isEmpty props then None
        else 
            Some { new IPropertyDiff with
                       member __.First = first.ToString()
                       member __.Other = other.ToString()
                       member __.PropertyName = PropertyInfo.FromExpression(prop).Name
                       member __.Diffs = (forProperties first other props) |> Seq.cast<IDiffNode> }
//    let For (first: XElement) (other: XElement) (prop: PropertyInfo) = 
//        { new IElementDiff with
//            member x.First = first.ToString()
//            member x.Other = other.ToString()
//            member x.PropertyName = prop.Name }
