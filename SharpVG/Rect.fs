namespace SharpVG

type Rect =
    {
        Position: Point
        Size: Area
        CornerRadius: Point option
    }
with
    static member ToTag rect =
        Tag.create "rect"
        |> Tag.withAttributes (Point.toAttributes rect.Position)
        |> Tag.addAttributes (Area.toAttributes rect.Size)
        |> Tag.addAttributes (match rect.CornerRadius with Some(cr) -> Point.toAttributesWithModifier "r" "" cr | _ -> [])

    override this.ToString() =
       this |> Rect.ToTag |> Tag.toString

module Rect =
    let create position size =
        { Position = position; Size = size; CornerRadius = None }

    let withCornerRadius point rect =
        { rect with CornerRadius = Some(point) }

    let toTag =
        Rect.ToTag

    let toString (rect : Rect) =
        rect.ToString()