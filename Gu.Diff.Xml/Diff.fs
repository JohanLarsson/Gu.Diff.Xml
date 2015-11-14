namespace Gu.Diff.Xml

//module Parsing =
//    open System.Xml.Linq
//    open System
//
//    type DiffDocument = 
//        | XDocAndText of Document : XDocument *  Xml : string
//        | ExcpetionAndText of Excption : Exception * Xml : string
//
//    let parse xml =
//        try XDocAndText(XDocument.Parse xml, xml)
//        with ex -> ExcpetionAndText (ex, xml)

//module Diff =
//    let Documents first other =
//        let firstDoc = parse first
//        let otherDoc = parse first


