module public Gu.Diff.Xml.Compare

open System
open System.Reflection
open System.Xml
open System.Xml.Linq

type internal Doc = 
    | Document of Document : XDocument
    | Exception of Exception : Exception

type internal Docs = 
    | Valids of First : XDocument * Other : XDocument
    | ExAndDoc of Exception : Exception

module internal Parse = 
    let text xml = 
        try 
            Document(XDocument.Parse xml)
        with ex -> Exception(ex)
    
    let documents first other = (text first), (text other)

//let public Documents(first, other) = 
//    let docs = Parse.documents first other
//    match docs with
//    | (Document first, Document other) -> Check.all first other
//    | (_, _) -> failwith "fail"
