module internal Gu.Diff.Xml.Check

open Microsoft.FSharp.Linq
open System
open System.Reflection
open System.Xml.Linq

let declarationProps =
        [| 
            fun (x:XDeclaration) -> box (x.Encoding)
            fun (x:XDeclaration) -> box (x.Version)
            fun (x:XDeclaration) -> box (x.Standalone)
        |]

let checkDeclarations first other =
    declarationProps
    // return  declarationProps.Where(x => x(first) != x(other))
    //                         .Select(x => new Diff(first, other, GetPropertyInfo(x))

let all first other =
    Seq.empty
//    |> Seq.append declarations first other