namespace SharpVG

type Area =
    {
        Width : Length;
        Height : Length;
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Area =
    let create width height =
        { Width = width; Height = height }

    let ofFloats (x, y) =
        create (Pixel x) (Pixel y)

    let ofInts (x, y) =
        ofFloats (float x, float y)

    let full =
        create (Percent 100.0) (Percent 100.0)

    let toAttributes area =
        set[Attribute.createXML "height" <| Length.toString area.Height; Attribute.createXML "width" <| Length.toString area.Width]

    let toString area =
        Length.toString area.Height + "," + Length.toString area.Width
