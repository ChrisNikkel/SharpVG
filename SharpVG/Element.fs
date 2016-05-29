namespace SharpVG

type baseElement =
    | Line of line
    | Text of text
    | Image of image
    | Circle of circle
    | Ellipse of ellipse
    | Rect of rect
    | Polygon of points
    | Polyline of points

type element =
    | StyledElement of baseElement * style
    | PlainElement of baseElement

module Element =
    let ofLine line = PlainElement(Line(line))
    let ofText text = PlainElement(Text(text))
    let ofImage image = PlainElement(Image(image))
    let ofCircle circle = PlainElement(Circle(circle))
    let ofEllipse ellipse = PlainElement(Ellipse(ellipse))
    let ofRect rect = PlainElement(Rect(rect))
    let ofPolygon polygon = PlainElement(Polygon(polygon))
    let ofPolyline polyline = PlainElement(Polyline(polyline))

    let withStyle style element =
        match element with
            | StyledElement(element, _) -> StyledElement(element, style)
            | PlainElement(element) -> StyledElement(element, style)

    let toTag element =
        let baseElementToTag baseElement =
            match baseElement with
                | Line(line) -> line |> Line.toTag
                | Text(text) -> text |> Text.toTag
                | Image(image) -> image |> Image.toTag
                | Circle(circle) -> circle |> Circle.toTag
                | Ellipse(ellipse) -> ellipse |> Ellipse.toTag
                | Rect(rect) -> rect |> Rect.toTag
                | Polygon(polygon) -> polygon |> Polygon.toTag
                | Polyline(polyline) -> polyline |> Polyline.toTag
        match element with
            | StyledElement(element, style) ->
                let elementTag = baseElementToTag element
                let newAttribute oldAttribute =
                    match oldAttribute with
                        | None -> Some(style |> Style.toString)
                        | Some(attribute) -> Some(attribute + " " + (style |> Style.toString))
                { elementTag with Attribute = newAttribute elementTag.Attribute }
            | PlainElement(element) -> baseElementToTag element

    let toString element = element |> toTag |> Tag.toString
