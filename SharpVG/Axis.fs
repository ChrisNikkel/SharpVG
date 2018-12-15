namespace SharpVG

type Axis =
    {
        Visible: bool * bool
        Labels: bool * bool
        Style: Style
        Minimum: float * float
        Maximum: float * float
    }

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
        let yAxis = Line.create (Point.create Length.empty (Length.ofFloat (snd axis.Minimum))) (Point.create Length.empty (Length.ofFloat (snd axis.Maximum))) |> Element.ofLine |> Element.withStyle axis.Style
        let xAxis = Line.create (Point.create (Length.ofFloat (fst axis.Minimum)) Length.empty) (Point.create (Length.ofFloat (fst axis.Maximum)) Length.empty) |> Element.ofLine |> Element.withStyle axis.Style

        match axis.Visible with
            | true, true -> seq [xAxis; yAxis]
            | false, true -> seq [yAxis]
            | true, false -> seq [xAxis]
            | _ -> Seq.empty<Element>
