namespace SharpVG

type Image =
    {
        UpperLeft: Point
        Size: Area
        Source: string
    }
with
    static member ToTag image =
        Tag.create "image" |> Tag.withAttributes ((Point.toAttributes image.UpperLeft) @ (Area.toAttributes image.Size) @ [Attribute.createXML "xlink:href" image.Source])

    override this.ToString() =
        this |> Image.ToTag |> Tag.toString

module Image =

    let create upperLeft size source =
        { UpperLeft = upperLeft; Size = size; Source = source }

    let toTag =
        Image.ToTag

    let toString (image : Image) =
        image.ToString()