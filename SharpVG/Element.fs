namespace SharpVG

type BaseElement =
    | Line of Line
    | Text of Text
    | Image of Image
    | Circle of Circle
    | Ellipse of Ellipse
    | Rect of Rect
    | Polygon of seq<Point>
    | Polyline of seq<Point>
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
                    element.Id |> Option.map (Attribute.createCSS "id" >> List.singleton)
                    element.Class |> Option.map (Attribute.createCSS "class" >> List.singleton)
                    element.Style  |> Option.map Style.toAttributes
                    element.Transform |> Option.map (Transform.toAttribute >> List.singleton)
                ]
                |> List.choose id
                |> List.concat
            )
        |> match element.Animations with
            | Some(a) -> Tag.addBody (a |> Seq.map Animation.toString |> String.concat "")
            | None -> id

    let toString element = element |> toTag |> Tag.toString

    let setTo timing newElement element =
        let attributesDiff = ((newElement |> toTag).Attributes |> Set.ofList) - ((element |> toTag).Attributes |> Set.ofList) |> Set.toList
        element |> withAnimations (attributesDiff |> List.map (fun {Name = n; Value = v; Type = t} -> Animation.createSet timing t n v))

    // TODO: Add more animations such as transform, path, animate, etc.
