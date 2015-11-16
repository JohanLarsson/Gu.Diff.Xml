namespace Gu.Diff.Xml

open Microsoft.FSharp.Quotations;
open System.Collections.Generic

module internal PropertyInfo =
    open Microsoft.FSharp.Quotations
    let rec FromExpression expression = 
        match expression with
        | Patterns.PropertyGet (_, propertyInfo, _) -> propertyInfo
        | Patterns.ValueWithName(:? Expr as expr, _, _) -> FromExpression expr
        | Patterns.Lambda(_, expr) -> FromExpression expr
        | x -> failwithf "Unexpected input: %s" (x.ToString())

type IPropertyEqualityComparer<'TSource> = 
    abstract member PropertyInfo : System.Reflection.PropertyInfo
    inherit IEqualityComparer<'TSource>

type PropertyEqualityComparer<'TSource when 'TSource: null>(prop, equals, getHashCode) =
    let property = prop
    let equals = equals
    let getHashCode = getHashCode

    interface IPropertyEqualityComparer<'TSource> with
        member this.PropertyInfo = property

        member __.Equals((x: 'TSource), (y: 'TSource)): bool = 
            match x, y with
            |(null, null) -> true
            |(_, null) -> false
            |(null, _) -> false
            |(x, y) -> equals(property.GetValue(x), property.GetValue(y))

        member __.GetHashCode(obj: 'TSource): int = 
            match obj with
            |null -> raise(System.ArgumentNullException("obj"))
            |obj -> getHashCode(property.GetValue(obj))


module internal CreateEqualityComparer =
    let ForProperty<'TSource when 'TSource: null> (prop: Expr<'TSource -> string>) =
        let property = PropertyInfo.FromExpression prop
        PropertyEqualityComparer(property, (fun (x, y) -> x = y), (fun x -> x.GetHashCode())) :> IPropertyEqualityComparer<'TSource>

//    let ForXName (prop: Expr<'TSource -> System.Xml.Linq.XName>) =
//        PropertyEqualityComparer(prop) :> IPropertyEqualityComparer<'TSource>



