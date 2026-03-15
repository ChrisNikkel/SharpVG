namespace SharpVG

type AttributeType =
    | CSS = 1
    | XML = 2

type Attribute =
    {
        Name: string
        Value: string
        Type: AttributeType
    }
with
    override this.ToString() =
        let escaped = this.Value.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;")
        this.Name + "=" + "\"" + escaped + "\""

module Attribute =

    let create t name value =
        { Name = name; Value = value; Type = t }

    let createXML =
        create AttributeType.XML

    let createCSS =
        create AttributeType.CSS

    let toString (attribute : Attribute) =
        attribute.ToString()

module AttributeType =
    let toString attributeType =
        match attributeType with
        | AttributeType.CSS -> "CSS"
        | AttributeType.XML -> "XML"
        | _ -> "XML"
