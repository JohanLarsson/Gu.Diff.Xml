namespace Gu.Diff.Xml

module Parsing =
    open System.Xml.Linq
    open System

    type XDocAndSource = 
        { Document : XDocument
          Xml : string }

    type XmlExcpetionSource = 
        { Excption : Exception
          Xml : string }

    let parse xml =
        try
           { XDocument.Parse xml, xml}
        with
            |x ->  { x , xml }

module Diff =
    let Documents first other =
        let firstDoc = parse first
        let otherDoc = parse first

//
//type Diff() = 
//    member this.X = "F#"
