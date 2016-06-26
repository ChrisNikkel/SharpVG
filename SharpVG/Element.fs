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

    let create element =
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

    let ofLine line = create (Line line)
    let ofText text = create (Text text)
    let ofImage image = create (Image image)
    let ofCircle circle = create (Circle circle)
    let ofEllipse ellipse = create (Ellipse ellipse)
    let ofRect rect = create (Rect rect)
    let ofPolygon polygon = create (Polygon polygon)
    let ofPolyline polyline = create (Polyline polyline)
    let ofPath path = create (Path path)

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
        |> Tag.addAttributes
            (
                [
                    element.Id |> Option.map (Attribute.create "id" >> Set.singleton)
                    element.Class |> Option.map (Attribute.create "class" >> Set.singleton)
                    element.Style  |> Option.map Style.toAttributes
                    element.Transform |> Option.map (Transform.toAttribute >> Set.singleton)
                    Some Set.empty // To prevent an empty list for the reduce
                ]
                |> List.choose id |> List.reduce (+)
            )
        |> match element.Animations with
            | Some(a) -> Tag.addBody (a |> Seq.map Animation.toString |> String.concat "")
            | None -> id

    let toString element = element |> toTag |> Tag.toString
