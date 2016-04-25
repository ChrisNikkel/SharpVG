namespace SharpVG

type text =
    {
        upperLeft: point
        body: string
    }

module Text =
    let toString text =
        {
            name = "text";
            attribute = Some(Point.toDescriptiveString text.upperLeft)
            body = Some(text.body)
        }
        |> Tag.toString