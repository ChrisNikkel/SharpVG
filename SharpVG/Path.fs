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
    | LengthPart of PathPositioning*PathType*Point
    | ClosePath

type Path = seq<PathPart>

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Path =

    let empty = Seq.empty

    let add pathPositioning pathType point path =
        Seq.append path (Seq.singleton (LengthPart(pathPositioning, pathType, point)))

    let addRelative pathType point path =
        add Relative pathType point path

    let addAbsolute pathType point path =
        add Absolute pathType point path
    
    let addClosePath path =
        Seq.append path (Seq.singleton ClosePath)

    let toDataString path =
        let pathTypeToString pathType =
            match pathType with
                    | LengthPart(positioning, style, point) ->
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
                        + (Point.toStringWithSeparator " " point)
                    | ClosePath -> "Z"
        path
        |> Seq.map pathTypeToString
        |> String.concat " "

    let toAttribute path =
        Attribute.createXML "path" (path |> toDataString)

    let toTag path =
        Tag.create "path"
        |> Tag.withAttribute (Attribute.createXML "d" (path |> toDataString))

    let toString path =
        path |> toTag |> Tag.toString
