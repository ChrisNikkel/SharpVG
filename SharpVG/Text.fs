namespace SharpVG

type text =
    {
        UpperLeft: point
        Body: string
    }

module Text =
    let init upperLeft body =
        { UpperLeft = upperLeft; Body = body }

    let toTag text =
        {
            Name = "text";
            Attribute = Some(Point.toDescriptiveString text.UpperLeft)
            Body = Some(text.Body)
        }

    let toString text = text |> toTag |> Tag.toString