namespace SharpVG

type Plot =
    {
        Elements: seq<Element>
        Minimum: Point
        Maximum: Point
        Title: string option
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Plot =

    let create minimum maximum elements =
        {Elements = elements; Minimum = minimum; Maximum = maximum; Title = None}

    let line values =
        let (minimum, maximum) =
            let (xValues, yValues) = values |> List.unzip
            ((xValues |> List.min, yValues |> List.min), (xValues |> List.max, yValues |> List.max))

        let elements = 
            let path = Path.empty |> Path.addAbsolute PathType.MoveTo (Point.ofFloats values.Head)
            values
            |> List.scan (fun acc p -> acc |> (Path.addAbsolute PathType.LineTo (Point.ofFloats p))) path
            |> List.last
            |> Element.ofPath

        create (Point.ofFloats minimum) (Point.ofFloats maximum) (Seq.singleton elements)

    let toGroup plot =
        let style = { Stroke = Some(Name Colors.Black); StrokeWidth = Some(Length.ofInt 1); Fill = Some(Name Colors.White); Opacity = None }
        let namedStyle = style |> NamedStyle.ofStyle "std"
        let diagonal = Line.create plot.Minimum plot.Maximum |> Element.ofLine |> Element.withNamedStyle namedStyle
        let origin = Circle.create (Point.ofFloats (0.0, 0.0)) (Length.ofFloat 2.0) |>  Element.ofCircle |> Element.withNamedStyle namedStyle
        let yOffset = match plot.Minimum.Y with | Pixel y -> Length.ofFloat (y * -1.0)
        plot.Elements
        |> Seq.map (Element.withNamedStyle namedStyle)
        |> Group.ofSeq
        |> Group.addElement diagonal
        |> Group.addElement origin
        |> Group.asCartesian yOffset plot.Maximum.Y

