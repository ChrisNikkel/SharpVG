namespace SharpVG

type Point =
    {
        X : Length;
        Y : Length;
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Point =
    let create x y =
        { X = x; Y = y }

    let ofFloats (x, y) =
        create (UserSpace x) (UserSpace y)

    let ofInts (x, y) =
        ofFloats (float x, float y)

    let origin =
        ofInts (0, 0)

    let toFloats point =
        (Length.toFloat point.X, Length.toFloat point.Y)

    let toAttributesWithModifier pre post point =
        [Attribute.createXML (pre + "x" + post) (Length.toString point.X); Attribute.createXML (pre + "y" + post) (Length.toString point.Y)]

    let toAttributes point =
        toAttributesWithModifier "" "" point

    let toStringWithSeparator separator point =
        Length.toString point.X + separator + Length.toString point.Y

    let toString =
        toStringWithSeparator ","

module Points =
    let toString (points:seq<Point>) =
        points
        |> Seq.map Point.toString
        |> String.concat " "