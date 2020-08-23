namespace SharpVG

type Polygon =
    {
        Points: seq<Point>
    }
with
    static member ToTag polygon =
        Tag.create "polygon" |> Tag.withAttribute (Attribute.createXML "points" <| Points.toString polygon.Points)

    override this.ToString() =
        this |> Polygon.ToTag |> Tag.toString

module Polygon =

    let create points =
        { Points = points }

    let ofSeq points =
        { Points = points }

    let ofList points =
        { Points = points |> Seq.ofList }

    let ofArray points =
        { Points = points |> Seq.ofArray }

    let toTag =
        Polygon.ToTag

    let toString (polygon : Polygon) =
        polygon.ToString()
