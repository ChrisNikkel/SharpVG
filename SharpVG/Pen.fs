namespace SharpVG

type Pen =
    {
        Stroke: Color;
        Opacity: float option;
        Width: Length option;
    }

module Pen =
    let create stroke =
        { Stroke = stroke; Opacity = None; Width = None }

    let red =
        create (Name Colors.Red)

    let green =
        create (Name Colors.Green)

    let blue =
        create (Name Colors.Blue)

    let black =
        create (Name Colors.Black)

    let gray =
        create (Name Colors.Gray)
    let yellow =
        create (Name Colors.Yellow)

    let purple =
        create (Name Colors.Purple)

    let orange =
        create (Name Colors.Orange)

    let white =
        create (Name Colors.White)

    let createWithOpacity stroke opacity =
        { Stroke = stroke; Opacity = Some(opacity); Width = None }

    let createWithWidth stroke width =
        { Stroke = stroke; Opacity = None; Width = Some(width) }

    let createWithOpacityAndWidth stroke opacity width =
        { Stroke = stroke; Opacity = Some(opacity); Width = Some(width) }

    let withOpacity opacity pen =
        { pen with Opacity = Some(opacity) }

    let withWidth width (pen : Pen) =
        { pen with Width = Some(width) }