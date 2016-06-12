namespace SharpVG

type Text =
    {
        UpperLeft: Point
        Body: string
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Text =
    let create upperLeft body =
        { UpperLeft = upperLeft; Body = body }

    let toTag text =
        {
            Name = "text";
            Attribute = Some(Point.toDescriptiveString text.UpperLeft)
            Body = Some(text.Body)
        }

    let toString text = text |> toTag |> Tag.toString