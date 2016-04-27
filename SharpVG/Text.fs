namespace SharpVG

type text =
    {
        upperLeft: point
        body: string
    }

module Text =

    let toTag text =
        {
            name = "text";
            attribute = Some(Point.toDescriptiveString text.upperLeft)
            body = Some(text.body)
        }

    let toString text = text |> toTag |> Tag.toString