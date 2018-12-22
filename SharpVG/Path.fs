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

type Path =
    {
        PathParts: seq<PathPart>
    }
with
    static member ToDataString path =
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
        path.PathParts
        |> Seq.map pathTypeToString
        |> String.concat " "
    static member ToTag path =
        Tag.create "path"
            |> Tag.withAttribute (Attribute.createXML "d" (path |> Path.ToDataString))

module Path =

    let empty = { PathParts = Seq.empty }

// TODO: Add helper functions for seq, list, array to path
(*
    let ofSeq pathType parts =
        { PathParts = parts }

    let ofList pathType parts =
        { PathParts = parts |> Seq.ofList }

    let ofArray pathType parts =
        { PathParts = parts |> Seq.ofArray }
*)
    let add pathPositioning pathType point path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton (LengthPart(pathPositioning, pathType, point))) }

    let addRelative pathType point path =
        add Relative pathType point path

    let addAbsolute pathType point path =
        add Absolute pathType point path

    let addClosePath path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton ClosePath) }

    let toAttribute path =
        Attribute.createXML "path" (path |> Path.ToDataString)

    let toTag =
        Path.ToTag

    let toString path =
        path |> toTag |> Tag.toString
