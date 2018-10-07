namespace SharpVG

type Axis =
    {
        Visible: bool * bool
        Labels: bool * bool
        Style: Style
        Minimum: Point
        Maximum: Point
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Axis =
    let defaultStyle = { Stroke = Some(Name Colors.DarkGrey); StrokeWidth = Some(Length.ofInt 1); Fill = None; Opacity = None; FillOpacity = None; Name = Some("DefaultAxisStyle") }

    let create minimum maximum =
        {
            Visible = (true, true);
            Labels = (true, true);
            Style = defaultStyle;
            Minimum = minimum;
            Maximum = maximum
        }

    let toElements axis =
        let xAxis = Line.create (Point.create Length.empty axis.Minimum.Y) (Point.create Length.empty axis.Maximum.Y) |> Element.ofLine |> Element.withStyle axis.Style
        let yAxis = Line.create (Point.create axis.Minimum.X Length.empty) (Point.create axis.Maximum.X Length.empty) |> Element.ofLine |> Element.withStyle axis.Style

        match axis.Visible with
            | true, true -> seq [xAxis; yAxis]
            | false, true -> seq [yAxis]
            | true, false -> seq [xAxis]
            | _ -> Seq.empty<Element>
