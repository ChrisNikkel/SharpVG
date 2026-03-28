namespace SharpVG

type Image =
    {
        Position: Point
        Size: Area
        Source: string
        PreserveAspectRatio: PreserveAspectRatio option
    }
with
    static member ToTag image =
        Tag.create "image"
        |> Tag.withAttributes (Point.toAttributes image.Position @ Area.toAttributes image.Size @ [Attribute.createXML "href" image.Source])
        |> (match image.PreserveAspectRatio with Some par -> Tag.addAttributes [PreserveAspectRatio.toAttribute par] | None -> id)

    override this.ToString() =
        this |> Image.ToTag |> Tag.toString

module Image =

    let create position size source =
        { Position = position; Size = size; Source = source; PreserveAspectRatio = None }

    let withPreserveAspectRatio par (image: Image) =
        { image with PreserveAspectRatio = Some par }

    let toTag =
        Image.ToTag

    let toString (image : Image) =
        image.ToString()
