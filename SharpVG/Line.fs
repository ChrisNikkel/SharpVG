namespace SharpVG

type Line =
    {
        Point1: Point
        Point2: Point
    }
with
    static member this.ToTag line =
        Tag.create "line"
            |> Tag.withAttributes (Point.toAttributesWithModifier "" "1" line.Point1)
            |> Tag.addAttributes (Point.toAttributesWithModifier "" "2" line.Point2)

    static member ToHtmlCanvas line =
        line.ToTag line |> (Tag.withHtmlCanvasCode "ctx.moveTo(" + this.Point.toString Point1 + ");ctx.lineTo(" + Point.toString Point2 + ");ctx.stroke();") List.empty

    override this.ToString() =
        Line.ToTag this |> Tag.toString

module Line =
    let create point1 point2 =
        { Point1 = point1; Point2 = point2 }

    let toTag line =
        Line.ToTag line

    let toString (line : Line) =
        line.ToString()