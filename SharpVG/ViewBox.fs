namespace SharpVG

type ViewBox =
    {
        Minimum: Point
        Size: Area
    }

module ViewBox =

    let create minimum size =
        { Minimum = minimum; Size = size }

    let toAttributes viewBox =
        [Attribute.createXML "viewBox" ((Point.toString viewBox.Minimum) + " " + (Area.toString viewBox.Size))]
