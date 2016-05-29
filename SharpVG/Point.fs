namespace SharpVG

type point =
    {
        x : length;
        y : length;
    }

module Point =
    let toDescriptiveStringWithModifier point pre post =
        pre + "x" + post + "=" + Tag.quote (Length.toString point.x) + " " +
        pre + "y" + post + "=" + Tag.quote (Length.toString point.y)

    let toDescriptiveString point =
        toDescriptiveStringWithModifier point "" ""

    let toStringWithSeparator separator point =
        Length.toString point.x + separator + Length.toString point.y

    let toString =
        toStringWithSeparator ","
