module internal Helpers
open Microsoft.FSharp.Quotations

let rec GetPropertyInfoFromExpression = function
    | Patterns.PropertyGet (_, propertyInfo, _) -> propertyInfo
    | Patterns.Lambda(_, expr) -> GetPropertyInfoFromExpression expr
    | x -> failwithf "Unexpected input: %s" (x.ToString())

