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
        Class: string option;
        Element: baseElement;
        Style: style option;
        Transform: transform option
    }

module Element =
    let init id className style transform element =
        {
            Id = id;
            Class = className;
            Element = element;
            Style = Some style;
            Transform = Some transform
        }

    let initWithElement element =
        {
            Id = None;
            Class = None;
            Element = element;
            Style = None;
            Transform = None
        }

    let withStyle style element =
        { element with Style = Some style }

    let withTransform transform element =
        { element with Transform = Some transform }

    let withId id element =
        { element with Id = Some id }

    let withClass className element =
        { element with Class = Some className }

    let ofLine line = initWithElement (Line line)
    let ofText text = initWithElement (Text text)
    let ofImage image = initWithElement (Image image)
    let ofCircle circle = initWithElement (Circle circle)
    let ofEllipse ellipse = initWithElement (Ellipse ellipse)
    let ofRect rect = initWithElement (Rect rect)
    let ofPolygon polygon = initWithElement (Polygon polygon)
    let ofPolyline polyline = initWithElement (Polyline polyline)

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
                element.Style  |> Option.map Style.toString
                element.Transform |> Option.map Transform.toString
            ] |> List.choose id |>  String.concat " "
        elementTag |> Tag.addAttribute attribute

    let toString element = element |> toTag |> Tag.toString
