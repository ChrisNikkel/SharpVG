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
        let yOffset =  Length.ofFloat (((Length.toFloat plot.Maximum.Y) - (Length.toFloat plot.Minimum.Y)))
        let xAxis = Line.create (Point.create plot.Minimum.X Length.empty) (Point.create plot.Maximum.X Length.empty) |> Element.ofLine |> Element.withNamedStyle namedStyle
        let yAxis = Line.create (Point.create Length.empty plot.Minimum.Y) (Point.create Length.empty plot.Maximum.Y) |> Element.ofLine |> Element.withNamedStyle namedStyle
        plot.Elements
        |> Seq.map (Element.withNamedStyle namedStyle)
        |> Group.ofSeq
        |> Group.addElement xAxis
        |> Group.addElement yAxis
        |> Group.asCartesian yOffset plot.Maximum.Y

