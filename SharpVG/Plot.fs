namespace SharpVG

type Plot =
    {
        Elements: seq<Element>
        Minimum: Point
        Maximum: Point
        Title: string option
        Style: Style
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Plot =
    let defaultScale = 1000.0
    let defaultStyle = { Stroke = Some(Name Colors.Black); StrokeWidth = Some(Length.ofInt 1); Fill = Some(Name Colors.White); Opacity = None; Name = Some("DefaultPlotStyle") }

    let create minimum maximum elements =
        {Elements = elements; Minimum = minimum; Maximum = maximum; Title = None; Style = defaultStyle}

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

    let lineXY = line

    let lineX values =
        let naturalNumbers = [1.0 .. (List.length values |> float)]
        line (List.zip naturalNumbers values)

    let toGroup plot =
        let yOffset =  Length.ofFloat (((Length.toFloat plot.Maximum.Y) - (Length.toFloat plot.Minimum.Y)))
        let xAxis = Line.create (Point.create plot.Minimum.X Length.empty) (Point.create plot.Maximum.X Length.empty) |> Element.ofLine |> Element.withStyle plot.Style
        let yAxis = Line.create (Point.create Length.empty plot.Minimum.Y) (Point.create Length.empty plot.Maximum.Y) |> Element.ofLine |> Element.withStyle plot.Style

        plot.Elements
            |> Seq.map (Element.withStyle plot.Style)
            |> Group.ofSeq
            |> Group.withId "MyPlot"
            |> Group.addElement xAxis
            |> Group.addElement yAxis
            |> Group.asCartesian yOffset plot.Maximum.Y

