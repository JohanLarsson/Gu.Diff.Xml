namespace Gu.Diff.Xml

open Microsoft.FSharp.Quotations;
open System.Collections.Generic
open System.Xml.Linq

type Diff<'a> = 
    { First : 'a
      Other : 'a
      Property : string }

type IPropertyComparer<'TSource,'TProperty> = inherit IEqualityComparer<'TSource>

type PropertyComparer<'TSource, 'TProperty when 'TSource: null and 'TProperty: equality>(prop: Expr<'TSource -> 'TProperty>) =
    let property = Helpers.GetPropertyInfoFromExpression prop
    member this.PropertyInfo = property

    interface IPropertyComparer<'TSource,'TProperty> with
        member __.Equals((x: 'TSource), (y: 'TSource)): bool = 
            match x, y with
            |(null, null) -> true
            |(_, null) -> false
            |(null, _) -> false
            |(x, y) -> obj.Equals(property.GetValue(x), property.GetValue(y))
        member __.GetHashCode(obj: 'TSource): int = 
            match obj with
            |null -> raise(System.ArgumentNullException("obj"))
            |x -> property.GetValue(x).GetHashCode()