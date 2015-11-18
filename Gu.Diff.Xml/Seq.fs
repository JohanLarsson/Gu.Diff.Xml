namespace Gu.Diff.Xml

open System.Collections
open System.Collections.Generic

module internal Seq = 
    let forever x = 
        seq { 
            while true do
                yield x
        }
    
    let asInfinite (s : seq<'t>) : seq<option<'t>> = 
        let somes = s |> Seq.map Some
        let nones = forever None
        Seq.append somes nones
    
    let map2All f (s1 : seq<'t>) (s2 : seq<'t>) : seq<'u> = 
        let s1 = s1 |> asInfinite
        let s2 = s2 |> asInfinite
        Seq.zip s1 s2
        |> Seq.takeWhile (fun (x, y) -> x.IsSome || y.IsSome)
        |> Seq.map (fun (x, y) -> f (x, y))
