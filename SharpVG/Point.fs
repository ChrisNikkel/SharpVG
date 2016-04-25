namespace SharpVG

type point =
    {
        x : size;
        y : size;
    }

module Point =
    let toDescriptiveStringWithModifier point pre post =
        pre + "x" + post + "=" + Tag.quote (Size.toString point.x) + " " +
        pre + "y" + post + "=" + Tag.quote (Size.toString point.y)

    let toDescriptiveString point =
        toDescriptiveStringWithModifier point "" ""

    let toString point =
        Size.toString point.x + "," + Size.toString point.y
