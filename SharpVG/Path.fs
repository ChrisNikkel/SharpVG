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

type EllipticalArcParameters =
    {
        Radius: Point
        RotationXAxis: int
        LargeArc: bool
        Sweep: bool
        Point: Point
    }

type PathPositioning =
    | Relative
    | Absolute

type PathPart =
    | LengthPart of PathPositioning*PathType*Point
    | EllipticalArc of PathPositioning*EllipticalArcParameters
    | ClosePath

type Path =
    {
        PathParts: seq<PathPart>
    }
with
    static member ToDataString path =
        let pointPathTypeToString pathType =
            match pathType with
                | MoveTo -> "M"
                | LineTo -> "L"
                | HorizontalLineTo -> "H"
                | VerticalLineTo -> "V"
                | CurveTo -> "C"
                | SmoothCurveTo -> "S"
                | QuadraticBezierCurveTo -> "Q"
                | SmoothQuadraticBezierCurveTo -> "T"

        let applyPositioning positioning (letter : string) =
            match positioning with
                | Absolute -> letter
                | Relative -> letter.ToLower()

        let flagToValue flag =
            if flag then "1" else "0"

        let pathPartToString pathPart =
            match pathPart with
                | LengthPart(positioning, pathType, point) ->
                    applyPositioning positioning (pointPathTypeToString pathType) + (Point.toStringWithSeparator " " point)
                | EllipticalArc(positioning, p) ->
                    (applyPositioning positioning "A") +
                    Point.toString p.Radius + " " +
                    string p.RotationXAxis + " " +
                    flagToValue p.LargeArc + "," +
                    flagToValue p.Sweep + " " +
                    Point.toString p.Point
                | ClosePath -> "Z"

        path.PathParts
            |> Seq.map pathPartToString
            |> String.concat " "

    static member ToTag path =
        Tag.create "path"
            |> Tag.withAttribute (Attribute.createXML "d" (path |> Path.ToDataString))

    override this.ToString() =
        this |> Path.ToTag |> Tag.toString

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

    let addEllipticalArc pathPositioning radius rotationXAxis largeArc sweep point path =
        let ellipticalArc = { Radius = radius; RotationXAxis = rotationXAxis; LargeArc = largeArc; Sweep = sweep; Point = point }
        { path with PathParts = Seq.append path.PathParts (Seq.singleton (EllipticalArc (pathPositioning, ellipticalArc))) }

    let addClosePath path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton ClosePath) }

    let toAttribute path =
        Attribute.createXML "path" (path |> Path.ToDataString)

    let toTag =
        Path.ToTag

    let toString (path : Path) =
        path.ToString()
