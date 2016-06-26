namespace SharpVG

type Attribute =
    {
        Name: string;
        Value: string
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Attribute =

    let create name value = 
        { Name = name; Value = value }

    let toString attribute =
        attribute.Name + "=" + "\"" + attribute.Value + "\""