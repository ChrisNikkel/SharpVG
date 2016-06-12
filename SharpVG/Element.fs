namespace SharpVG

type BaseElement =
    | Line of Line
    | Text of Text
    | Image of Image
    | Circle of Circle
    | Ellipse of Ellipse
    | Rect of Rect
    | Polygon of Points
    | Polyline of Points
    | Path of Path

type Element = {
        Id: string option;
        Class: string option;  // TODO: Auto fill this for named styles
        Element: BaseElement;
        Style: Style option;
        Transform: Transform option;
        Animations: seq<Animation> option
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Element =
    let create id className element =
        {
            Id = id;
            Class = className;
            Element = element;
            Style = None;
            Transform = None;
            Animations = None;
        }

    let createWithElement element =
        {
            Id = None;
            Class = None;
            Element = element;
            Style = None;
            Transform = None;
            Animations = None
        }

    let withStyle style (element:Element) =
        { element with Style = Some style }

    let withTransform transform element =
        { element with Transform = Some transform }

    let withAnimation animation element =
        { element with Animations = Some (Seq.singleton animation) }

    let withAnimations animations element =
        { element with Animations = Some animations }

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
        match element.Element with
            | Line line -> line |> Line.toTag
            | Text text -> text |> Text.toTag
            | Image image -> image |> Image.toTag
            | Circle circle -> circle |> Circle.toTag
            | Ellipse ellipse -> ellipse |> Ellipse.toTag
            | Rect rect -> rect |> Rect.toTag
            | Polygon polygon -> polygon |> Polygon.toTag
            | Polyline polyline -> polyline |> Polyline.toTag
            | Path path -> path |> Path.toTag
        |> Tag.addAttribute
            (
                [
                    element.Id |> Option.map (fun id -> "id=" + (Tag.quote id))
                    element.Class |> Option.map (fun className -> "class=" + (Tag.quote className))
                    element.Style  |> Option.map Style.toAttributeString
                    element.Transform |> Option.map Transform.toString
                ]
                |> List.choose id |>  String.concat " "
            )
        |> match element.Animations with
            | Some(a) -> Tag.addBody (a |> Seq.map Animation.toString |> String.concat "")
            | None -> id

    let toString element = element |> toTag |> Tag.toString
