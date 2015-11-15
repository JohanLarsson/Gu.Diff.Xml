namespace Gu.Diff.Xml
open System.Collections.Generic
open System.Xml.Linq

type Diff<'a> = 
    { First : 'a
      Other : 'a
      Property : string }


type IPropertyComparer<'TSource,'TProperty> = inherit IEqualityComparer<'TSource>

type PropertyComparer<'TProperty when 'TProperty: equality>(prop: XDeclaration -> 'TProperty) = 
    let property = prop
    member this.PropertyInfo = Helpers.GetPropertyInfoFromExpression <@ prop @>
    interface IPropertyComparer<XDeclaration,'TProperty> with
        member __.Equals((x: XDeclaration), (y: XDeclaration)): bool = 
            match x, y with
            |(null, null) -> true
            |(_, null) -> false
            |(null, _) -> false
            |(x, y) -> obj.Equals(prop(x), prop(y))
        member __.GetHashCode(obj: XDeclaration): int = 
            match obj with
            |null -> raise(System.ArgumentNullException("obj"))
            |x -> prop(x).GetHashCode()