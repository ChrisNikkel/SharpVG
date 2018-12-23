namespace SharpVG

type Rect =
    {
        UpperLeft: Point
        Size: Area
        CornerRadius: Point option
    }
with
    static member ToTag rect =
        Tag.create "rect"
        |> Tag.withAttributes (Point.toAttributes rect.UpperLeft)
        |> Tag.addAttributes (Area.toAttributes rect.Size)
        |> Tag.addAttributes (match rect.CornerRadius with Some(cr) -> Point.toAttributesWithModifier "r" "" cr | _ -> [])

    override this.ToString() =
       this |> Rect.ToTag |> Tag.toString

module Rect =
    let create upperLeft size =
        { UpperLeft = upperLeft; Size = size; CornerRadius = None }

    let withCornerRadius point rect =
        { rect with CornerRadius = Some(point) }

    let toTag =
        Rect.ToTag

    let toString (rect : Rect) =
        rect.ToString()