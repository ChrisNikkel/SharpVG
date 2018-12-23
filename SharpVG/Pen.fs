namespace SharpVG

type Pen =
    {
        Color: Color;
        Opacity: float option;
        Width: Length option;
    }

module Pen =
    let create color =
        { Color = color; Opacity = None; Width = None }

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

    let createWithOpacity color opacity =
        { Color = color; Opacity = Some(opacity); Width = None }

    let createWithWidth color width =
        { Color = color; Opacity = None; Width = Some(width) }

    let createWithOpacityAndWidth color opacity width =
        { Color = color; Opacity = Some(opacity); Width = Some(width) }

    let withOpacity opacity pen =
        { pen with Opacity = Some(opacity) }

    let withWidth width (pen : Pen) =
        { pen with Width = Some(width) }