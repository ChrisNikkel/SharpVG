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
        Id: string option
        Classes: seq<string>
        Element: BaseElement
        Style: Style option
        Transform: Transform option
        Animations: seq<Animation>
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Element =

    let create element =
        {
            Id = None
            Classes = Seq.empty
            Element = element
            Style = None
            Transform = None
            Animations = Seq.empty
        }

    let withStyle style (element:Element) =
        { element with Style = Some style }

    let withTransform transform element =
        { element with Transform = Some transform }

    let withAnimation animation element =
        { element with Animations = Seq.singleton animation }

    let withAnimations animations element =
        { element with Animations = animations }

    let addAnimation animation element =
        element.Animations |> Seq.append (Seq.singleton animation)

    let withId id element =
        { element with Id = Some id }

    let withClass className element =
        { element with Classes = Seq.singleton className }

    let withClasses classes element =
        { element with Classes = classes }

    let addClass className element =
        element.Classes |> Seq.append (Seq.singleton className)

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
        let styleClass =
            match element.Style with
                | Some s -> s.Name
                | None -> None

        let classes =
            match styleClass with
                | Some n -> element.Classes |> Seq.append (Seq.singleton n)
                | None -> element.Classes

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
        |> Tag.insertAttributes
            (
                [
                    element.Id |> Option.map (Attribute.createCSS "id" >> List.singleton)
                    classes |> Seq.map (Attribute.createCSS "class") |> Seq.toList |> Option.Some
                    element.Style |> Option.filter (not << Style.isNamed) |> Option.map Style.toAttributes
                    element.Transform |> Option.map (Transform.toAttribute >> List.singleton)
                ]
                |> List.choose id
                |> List.concat
            )
        |> if element.Animations |> Seq.isEmpty then id else Tag.addBody (element.Animations |> Seq.map Animation.toString |> String.concat "")

    let toString element = element |> toTag |> Tag.toString

    let setTo timing newElement element =
        let attributesDiff = ((newElement |> toTag).Attributes |> Set.ofList) - ((element |> toTag).Attributes |> Set.ofList) |> Set.toList
        element |> withAnimations (attributesDiff |> List.map (fun {Name = n; Value = v; Type = t} -> Animation.createSet timing t n v))

    // TODO: Add more animations such as transform, path, animate, etc.
