namespace SharpVG

type Image =
    {
        UpperLeft: Point
        Size: Area
        Source: string
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Image =

    let create upperLeft size source =
        { UpperLeft = upperLeft; Size = size; Source = source }

    let toTag image =
        {
            Name = "image";
            Attribute = Some((Point.toDescriptiveString image.UpperLeft) + " " + Area.toDescriptiveString image.Size);
            Body = None
        }

    let toString = toTag >> Tag.toString