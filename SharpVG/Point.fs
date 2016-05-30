namespace SharpVG

type point =
    {
        X : length;
        Y : length;
    }

module Point =
    let create x y =
        { X = x; Y = y }

    let toFloats point =
        (Length.toFloat point.X, Length.toFloat point.Y)

    let toDoubles point =
        (Length.toDouble point.X, Length.toDouble point.Y)

    let toDescriptiveStringWithModifier point pre post =
        pre + "x" + post + "=" + Tag.quote (Length.toString point.X) + " " +
        pre + "y" + post + "=" + Tag.quote (Length.toString point.Y)

    let toDescriptiveString point =
        toDescriptiveStringWithModifier point "" ""

    let toStringWithSeparator separator point =
        Length.toString point.X + separator + Length.toString point.Y

    let toString =
        toStringWithSeparator ","
