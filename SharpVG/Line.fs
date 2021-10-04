namespace SharpVG

type Line =
    {
        Point1: Point
        Point2: Point
    }
with
    static member ToTag line =
        Tag.create "line"
            |> Tag.withAttributes (Point.toAttributesWithModifier "" "1" line.Point1)
            |> Tag.addAttributes (Point.toAttributesWithModifier "" git checkout -b  "2" line.Point2)

    static member ToHtmlCanvas =
        "ctx.moveTo(" + Point.toString Point1 + ");ctx.lineTo(" + Point.toString Point2 + ");ctx.stroke();"

    static member ToHtmlCanvas (line : Line) =
        let parameters = [(Point.toString line.Point1); (Point.toString line.Point2)]
        Tag.withHtmlCanvasCode "ctx.moveTo(%1%);ctx.lineTo(%2%);" parameters (Line.ToTag line)

    override this.ToString() =
        Line.ToTag this |> Tag.toString

module Line =
    let create point1 point2 =
        { Point1 = point1; Point2 = point2 }

    let toTag line =
        Line.ToTag line

    let toString (line : Line) =
        line.ToString()