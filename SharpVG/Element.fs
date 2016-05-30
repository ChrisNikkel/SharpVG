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
            seq {
                yield match element.Id with | Some id -> Some("id=" + (Tag.quote element.Id)) | None -> None;
                yield match element.Class with | Some className -> Some("class=" + (Tag.quote element.Class)) | None -> None;
                yield match element.Style with | Some style -> Some(style |> Style.toString) | None -> None;
                yield match element.Transform with | Some style -> Some(style |> Transform.toString) | None -> None
            } |> Seq.choose id |>  String.concat " "
        elementTag |> Tag.addAttribute attribute

    let toString element = element |> toTag |> Tag.toString
