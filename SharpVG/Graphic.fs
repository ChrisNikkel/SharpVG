namespace SharpVG

type Graphic =
    | Path of Path
    | Polygon of Polygon
    | Polyline of Polyline
    | Image of Image
    | Circle of Circle
    | Ellipse of Ellipse
    | Line of Line
    | Rect of Rect
    | Text of Text
    | Use of Use

module Graphic =
    let ofPath path =
        Path(path)

    let ofPolygon polygon =
        Polygon(polygon)

    let ofPolyline polyline =
        Polyline(polyline)

    let ofImage image =
        Image(image)

    let ofCircle circle =
        Circle(circle)

    let ofEllipse ellipse =
        Ellipse(ellipse)

    let ofLine line =
        Line(line)

    let ofRect rect =
        Rect(rect)

    let ofText text =
        Text(text)

    let ofUse u =
        Use(u)

    let toElement graphic =
        match graphic with
            | Path(p) -> Element.create p
            | Polygon(p) -> Element.create p
            | Polyline(p) -> Element.create p
            | Image(i) -> Element.create i
            | Circle(c) -> Element.create c
            | Ellipse(e) -> Element.create e
            | Line(l) -> Element.create l
            | Rect(r) -> Element.create r
            | Text(t) -> Element.create t
            | Use(u) -> Element.create u
