namespace SharpVG

type PlotElement =
    {
        Elements: seq<Element>
        Style: Style
    }

type Plot =
    {
        Minimum: Point
        Maximum: Point
        Title: string option
        Axis: Axis option

        PlotElement: PlotElement
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Plot =
    let defaultScale = 1000.0
    let defaultStyle = { Stroke = Some(Name Colors.Black); StrokeWidth = Some(Length.ofInt 1); Fill = None; Opacity = None; FillOpacity = Some(0.0); Name = Some("DefaultPlotStyle") }

    let create minimum maximum elements =
        {PlotElement = {Elements = elements; Style = defaultStyle};  Minimum = minimum; Maximum = maximum; Title = None; Axis = None}

    let withAxis axis plot =
        {plot with Axis = Some(axis)}

    let discoverSize values =
        let (minimum, maximum) =
            let (xValues, yValues) = values |> List.unzip
            ((xValues |> List.min, yValues |> List.min), (xValues |> List.max, yValues |> List.max))

        (Point.ofFloats minimum, Point.ofFloats maximum)

    let plot values =
        let min, max = discoverSize values

        values
            |> List.map (fun p -> Circle.create (Point.ofFloats p) (Length.ofInt 1) |> Element.ofCircle)
            |> create min max
            |> withAxis (Axis.create min max)

    let plotXY = plot

    let plotX values =
        let naturalNumbers = [1.0 .. (List.length values |> float)]
        plot (List.zip naturalNumbers values)

    let line values =
        let min, max = discoverSize values
        let path = Path.empty |> Path.addAbsolute PathType.MoveTo (Point.ofFloats (List.head values))
        values
            |> List.scan (fun acc p -> acc |> Path.addAbsolute PathType.LineTo (Point.ofFloats p)) path
            |> List.last
            |> Element.ofPath
            |> Seq.singleton
            |> create min max
            |> withAxis (Axis.create min max)

    let lineXY = line

    let lineX values =
        let naturalNumbers = [1.0 .. (List.length values |> float)]
        line (List.zip naturalNumbers values)

    let toGroup plot =
        let yOffset =  Length.ofFloat (((Length.toFloat plot.Maximum.Y) - (Length.toFloat plot.Minimum.Y)))

        plot.Axis
            |> Option.fold (fun _ a -> Axis.toElements a) Seq.empty<Element>
            |> Group.ofSeq
            |> Group.addElements (plot.PlotElement.Elements |> Seq.map (Element.withStyle plot.PlotElement.Style))
            |> Group.withId "MyPlot"
            |> Group.asCartesian yOffset plot.Maximum.Y

