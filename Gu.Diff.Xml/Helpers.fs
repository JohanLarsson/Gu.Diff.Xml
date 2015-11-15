module internal Helpers
open Microsoft.FSharp.Quotations

let rec GetPropertyInfoFromExpression expression = 
    match expression with
    | Patterns.PropertyGet (_, propertyInfo, _) -> propertyInfo
    | Patterns.ValueWithName(:? Expr as expr, _, _) -> GetPropertyInfoFromExpression expr
    | Patterns.Lambda(_, expr) -> GetPropertyInfoFromExpression expr
    | x -> failwithf "Unexpected input: %s" (x.ToString())
