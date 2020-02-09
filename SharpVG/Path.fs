namespace SharpVG

type PathPositioning =
    | Relative
    | Absolute

type EllipticalArcPathParameters =
    {
        Radius: Point
        RotationXAxis: float
        LargeArc: bool
        Sweep: bool
        Point: Point
    }

type ParameterizedPathPart =
    | HorizontalLineTo of Length seq
    | VerticalLineTo of Length seq
    | MoveTo of Point seq
    | LineTo of Point seq
    | CubicBezierCurveTo of (Point * Point * Point) seq
    | SmoothCubicBezierCurveTo of (Point * Point) seq
    | QuadraticBezierCurveTo of (Point * Point) seq
    | SmoothQuadraticBezierCurveTo of Point seq
    | EllipticalArcCurveTo of EllipticalArcPathParameters seq

type PathPart =
    | RelativePathPart of PathPositioning * ParameterizedPathPart
    | ClosePath

type Path =
    {
        PathParts: PathPart seq
    }
with
    static member ToDataString path =
        let getPathType pathPart  =
            match pathPart with
                | RelativePathPart(_, MoveTo(_)) -> "M"
                | RelativePathPart(_, LineTo(_)) -> "L"
                | RelativePathPart(_, HorizontalLineTo(_)) -> "H"
                | RelativePathPart(_, VerticalLineTo(_)) -> "V"
                | RelativePathPart(_, CubicBezierCurveTo(_)) -> "C"
                | RelativePathPart(_, SmoothCubicBezierCurveTo(_)) -> "S"
                | RelativePathPart(_, QuadraticBezierCurveTo(_)) -> "Q"
                | RelativePathPart(_, SmoothQuadraticBezierCurveTo(_)) -> "T"
                | RelativePathPart(_, EllipticalArcCurveTo(_)) -> "A"
                | ClosePath -> "Z"

        let applyPositioning positioning (letter : string) =
            match positioning with
                | Absolute -> letter
                | Relative -> letter.ToLower()

        let flagToValue flag =
            if flag then "1" else "0"

        let pathParametersToString parameterizedPathPart =
            match parameterizedPathPart with
                | HorizontalLineTo(lengths) | VerticalLineTo(lengths) -> lengths |> Seq.map Length.toString
                | MoveTo(points) | LineTo(points) | SmoothQuadraticBezierCurveTo(points) -> points |> Seq.map Point.toString
                | QuadraticBezierCurveTo(pointPairs) | SmoothCubicBezierCurveTo(pointPairs) -> pointPairs |> Seq.map (fun (p1, p2) -> Point.toString p1 + " " + Point.toString p2)
                | CubicBezierCurveTo(pointTriplets) -> pointTriplets |> Seq.map (fun (p1, p2, p3) -> Point.toString p1 + " " + Point.toString p2 + " " + Point.toString p3)
                | EllipticalArcCurveTo(arcParams) -> arcParams |> Seq.map (fun ap -> Point.toString ap.Radius + " " + string ap.RotationXAxis + " " + flagToValue ap.LargeArc + "," + flagToValue ap.Sweep + " " + Point.toString ap.Point)
            |> String.concat " "

        path.PathParts
            |> Seq.map (fun part ->
                let pathType = getPathType part
                match part with
                | RelativePathPart(positioning, parameters) -> applyPositioning positioning pathType + pathParametersToString parameters
                | ClosePath -> pathType)
            |> String.concat " "

    static member ToTag path =
        Tag.create "path"
            |> Tag.withAttribute (Attribute.createXML "d" (path |> Path.ToDataString))

    override this.ToString() =
        this |> Path.ToTag |> Tag.toString

module Path =

    let empty = { PathParts = Seq.empty }

    let addHorizontalLinesTo pathPositioning lengths path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton (RelativePathPart(pathPositioning, HorizontalLineTo(lengths)))) }

    let addHorizontalLineTo pathPositioning length path =
        addHorizontalLinesTo pathPositioning (Seq.singleton length) path

    let addVerticalLinesTo pathPositioning lengths path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton (RelativePathPart(pathPositioning, VerticalLineTo(lengths)))) }

    let addVerticalLineTo pathPositioning length path =
        addVerticalLinesTo pathPositioning (Seq.singleton length) path

    let addMovesTo pathPositioning points path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton (RelativePathPart(pathPositioning, MoveTo(points)))) }

    let addMoveTo pathPositioning point path =
        addMovesTo pathPositioning (Seq.singleton point) path

    let addLinesTo pathPositioning points path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton (RelativePathPart(pathPositioning, LineTo(points)))) }

    let addLineTo pathPositioning point path =
        addLinesTo pathPositioning (Seq.singleton point) path

    let addSmoothCubicBezierCurvesTo pathPositioning pointPairs path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton (RelativePathPart(pathPositioning, SmoothCubicBezierCurveTo(pointPairs)))) }

    let addSmoothCubicBezierCurveTo pathPositioning point1 point2 path =
        addSmoothCubicBezierCurvesTo pathPositioning (Seq.singleton (point1, point2)) path

    let addCubicBezierCurvesTo pathPositioning pointTriplets path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton (RelativePathPart(pathPositioning, CubicBezierCurveTo(pointTriplets)))) }

    let addCubicBezierCurveTo pathPositioning point1 point2 point3 path =
        addCubicBezierCurvesTo pathPositioning (Seq.singleton (point1, point2, point3)) path

    let addQuadraticBezierCurvesTo pathPositioning pointPairs path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton (RelativePathPart(pathPositioning, QuadraticBezierCurveTo(pointPairs)))) }

    let addQuadraticBezierCurveTo pathPositioning point1 point2 path =
        addQuadraticBezierCurvesTo pathPositioning (Seq.singleton (point1, point2)) path

    let addSmoothQuadraticBezierCurvesTo pathPositioning points path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton (RelativePathPart(pathPositioning, SmoothQuadraticBezierCurveTo(points)))) }

    let addSmoothQuadraticBezierCurveTo pathPositioning point path =
        addSmoothQuadraticBezierCurvesTo pathPositioning (Seq.singleton point) path

    let addEllipticalArcCurvesTo pathPositioning ellipticalArcPathParameters path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton (RelativePathPart(pathPositioning, EllipticalArcCurveTo(ellipticalArcPathParameters)))) }

    let addEllipticalArcCurveTo pathPositioning radius rotationXAxis largeArc sweep point path =
        addEllipticalArcCurvesTo pathPositioning (Seq.singleton { Radius = radius; RotationXAxis = rotationXAxis; LargeArc = largeArc; Sweep = sweep; Point = point }) path

    let addClosePath path =
        { path with PathParts = Seq.append path.PathParts (Seq.singleton ClosePath) }

    let toAttribute path =
        Attribute.createXML "path" (path |> Path.ToDataString)

    let toTag =
        Path.ToTag

    let toString (path : Path) =
        path.ToString()
