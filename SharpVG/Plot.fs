namespace SharpVG

type PlotElement =
    {
        Elements: seq<Element>
        Style: Style
    }

type Plot =
    {
        Minimum: float * float
        Maximum: float * float
        Title: string option
        Axis: Axis option

        PlotElements: seq<PlotElement>
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Plot =
    let defaultScale = 1000.0

    let defaultStyle = { Stroke = Some(Name Colors.Black); StrokeWidth = Some(Length.ofInt 1); Fill = None; Opacity = None; FillOpacity = Some(0.0); Name = None }

    let defaultColors = [ Colors.Blue; Colors.Red; Colors.Green; Colors.Orange; Colors.Purple; Colors.Yellow ]

    let getDefaultColor i = defaultColors.[i % List.length defaultColors]

    let getDefaultStyle i = { defaultStyle with Stroke = Some(Name (getDefaultColor i)); Name = Some ("DefaultPlotStyle" + string i)}

    let discoverSize values =
        let (xValues, yValues) = values |> List.unzip
        ((xValues |> List.min, yValues |> List.min), (xValues |> List.max, yValues |> List.max))

    let create minimum maximum elements=
        {PlotElements = Seq.singleton {Elements = elements; Style = getDefaultStyle 0};  Minimum = minimum; Maximum = maximum; Title = None; Axis = None}

    let addPlotElement element plot =
        {plot with PlotElements = Seq.append plot.PlotElements (Seq.singleton {Elements = element; Style = getDefaultStyle (Seq.length plot.PlotElements)})}

    let addPlot newPlot plot =
        let (min, max) = discoverSize [newPlot.Minimum; newPlot.Maximum; plot.Minimum; plot.Maximum]
        let newPlotElements = newPlot.PlotElements |> Seq.mapi (fun i pe -> {pe with Style = getDefaultStyle (i + Seq.length plot.PlotElements)})
        {plot with PlotElements = Seq.append newPlotElements plot.PlotElements; Minimum = min; Maximum = max }

    let withAxis axis plot =
        {plot with Axis = Some(axis)}

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
        let yOffset =  Length.ofFloat ((snd plot.Maximum) - (snd plot.Minimum))

        plot.Axis
            |> Option.fold (fun _ a -> Axis.toElements a) Seq.empty<Element>
            |> Group.ofSeq
            |> Group.addElements (plot.PlotElements |> Seq.collect (fun pe -> pe.Elements |> Seq.map (Element.withStyle pe.Style)))
            |> Group.withId "MyPlot"
            |> Group.asCartesian yOffset (Length.ofFloat (snd plot.Maximum))

