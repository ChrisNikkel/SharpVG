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
        Tag.create "image" |> Tag.withAttributes ((Point.toAttributes image.UpperLeft) + (Area.toAttributes image.Size))

    let toString =
        toTag >> Tag.toString