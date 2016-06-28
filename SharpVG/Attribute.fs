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

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Attribute =

    let create t name value =
        { Name = name; Value = value; Type = t }

    let createXML =
        create AttributeType.XML

    let createCSS =
        create AttributeType.CSS

    let toString attribute =
        attribute.Name + "=" + "\"" + attribute.Value + "\""