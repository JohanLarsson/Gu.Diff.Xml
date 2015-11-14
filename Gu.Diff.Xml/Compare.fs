module public Gu.Diff.Xml.Compare
open System
open System.Xml.Linq

type internal DiffDocument = 
    | XDocAndXml of Document : XDocument *  Xml : string
    | ExcpetionAndXml of Excption : Exception * Xml : string

module internal Parse =
    let text xml =
        try XDocAndXml(XDocument.Parse xml, xml)
        with ex -> ExcpetionAndXml (ex, xml)


