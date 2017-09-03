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
        create (UserSpace x) (UserSpace y)

    let ofInts (x, y) =
        ofFloats (float x, float y)

    let fromPoints p1 p2 =
        create
            (Length.ofFloat (abs ((Length.toFloat p1.X) - (Length.toFloat p2.X))))
            (Length.ofFloat (abs ((Length.toFloat p1.Y) - (Length.toFloat p2.Y))))

    let toFloats area =
        (Length.toFloat area.Width, Length.toFloat area.Height)

    let full =
        create Length.full Length.full

    let half =
        create Length.half Length.half

    let toAttributes area =
        [Attribute.createXML "width" <| Length.toString area.Width; Attribute.createXML "height" <| Length.toString area.Height]

    let toString area =
        Length.toString area.Width + "," + Length.toString area.Height
