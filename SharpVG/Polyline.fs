namespace SharpVG

type Polyline =
    {
        Points: seq<Point>
    }
with
    static member ToTag polyline =
        Tag.create "polyline" |> Tag.withAttribute (Attribute.createXML "points" <| Points.toString polyline.Points)

    override this.ToString() =
        this |> Polyline.ToTag |> Tag.toString

module Polyline =

    let create points =
        { Points = points }

    let ofSeq points =
        { Points = points }

    let ofList points =
        { Points = points |> Seq.ofList }

    let ofArray points =
        { Points = points |> Seq.ofArray }

    let toTag =
        Polyline.ToTag

    let toString (polyline : Polyline) =
        polyline.ToString()