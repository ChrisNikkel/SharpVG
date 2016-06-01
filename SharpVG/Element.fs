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

type element = {
        Id: string option;
        Class: string option;  // TODO: Auto fill this for named styles
        Element: baseElement;
        Style: style option;
        Transform: transform option
    }

module Element =
    let create id className style transform element =
        {
            Id = id;
            Class = className;
            Element = element;
            Style = Some style;
            Transform = Some transform
        }

    let createWithElement element =
        {
            Id = None;
            Class = None;
            Element = element;
            Style = None;
            Transform = None
        }

    let withStyle style (element:element) =
        { element with Style = Some style }

    let withTransform transform element =
        { element with Transform = Some transform }

    let withId id element =
        { element with Id = Some id }

    let withClass className element =
        { element with Class = Some className }

    let withNamedStyle namedStyle =
        withClass namedStyle.Name

    let ofLine line = createWithElement (Line line)
    let ofText text = createWithElement (Text text)
    let ofImage image = createWithElement (Image image)
    let ofCircle circle = createWithElement (Circle circle)
    let ofEllipse ellipse = createWithElement (Ellipse ellipse)
    let ofRect rect = createWithElement (Rect rect)
    let ofPolygon polygon = createWithElement (Polygon polygon)
    let ofPolyline polyline = createWithElement (Polyline polyline)

    let toTag element =
        let elementTag =
            match element.Element with
                | Line line -> line |> Line.toTag
                | Text text -> text |> Text.toTag
                | Image image -> image |> Image.toTag
                | Circle circle -> circle |> Circle.toTag
                | Ellipse ellipse -> ellipse |> Ellipse.toTag
                | Rect rect -> rect |> Rect.toTag
                | Polygon polygon -> polygon |> Polygon.toTag
                | Polyline polyline -> polyline |> Polyline.toTag
        let attribute =
            [
                element.Id |> Option.map (fun id -> "id=" + (Tag.quote id))
                element.Class |> Option.map (fun className -> "class=" + (Tag.quote className))
                element.Style  |> Option.map Style.toAttributeString
                element.Transform |> Option.map Transform.toString
            ] |> List.choose id |>  String.concat " "
        elementTag |> Tag.addAttribute attribute

    let toString element = element |> toTag |> Tag.toString
