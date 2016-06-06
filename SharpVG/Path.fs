namespace SharpVG

type PathType =
    | MoveTo
    | LineTo
    | HorizontalLineTo
    | VerticalLineTo
    | CurveTo
    | SmoothCurveTo
    | QuadraticBezierCurveTo
    | SmoothQuadraticBezierCurveTo
    | EllipticalArc

type PathPositioning =
    | Relative
    | Absolute

type PathPart =
    | LengthPart of PathType*PathPositioning*point
    | ClosePath

type path = seq<PathPart>

module Path =
    let toTag path =
        let pathTypeToString pathType =
            match pathType with
                    | LengthPart(style, positioning, point) ->
                        let letter =
                            match style with
                                | MoveTo -> "M"
                                | LineTo -> "L"
                                | HorizontalLineTo -> "H"
                                | VerticalLineTo -> "V"
                                | CurveTo -> "C"
                                | SmoothCurveTo -> "S"
                                | QuadraticBezierCurveTo -> "Q"
                                | SmoothQuadraticBezierCurveTo -> "T"
                                | EllipticalArc -> "A"
                        match positioning with
                            | Absolute -> letter
                            | Relative -> letter.ToLower()
                        + " " + (Point.toString point)
                    | ClosePath -> "Z"
        {
            Name = "path";
            Attribute = Some("d=" + Tag.quote (path |> Seq.map pathTypeToString |> String.concat " "))
            Body = None
        }

    let toString path = path |> toTag |> Tag.toString