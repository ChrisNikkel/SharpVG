namespace SharpVG.Draw

open SharpVG
open System.Drawing

module Draw =
    // TODO: develop way to translate between the various coordinate systems
    let lengthToScreenInt length =
        match length with
        | UserSpace(f) -> int f
        | Pixels(i) -> i
        | Em(f) -> int f
        | Percent(f) -> int f

    let lengthToScreenFloat length =
        match length with
        | UserSpace(f) -> float32 f
        | Pixels(i) -> float32 i
        | Em(f) -> float32 f
        | Percent(f) -> float32 f

    let areaToXYInt area =
        (lengthToScreenInt area.Width, lengthToScreenInt area.Height)

    let areaToXYFloat area =
        (lengthToScreenFloat area.Width, lengthToScreenFloat area.Height)

    let pointToXYInt position =
        (lengthToScreenInt position.X, lengthToScreenInt position.Y)

    let pointToXYFloat position =
        (lengthToScreenFloat position.X, lengthToScreenFloat position.Y)

    let pointToPointInt position =
        let x, y = pointToXYInt position
        Point(x, y)

    let pointToPointFloat position =
        let x, y = pointToXYFloat position
        PointF(x, y)


    // TODO: Fix converstions to actually map to the right colors
    let colorToDrawingColor color =
        match color with
        | Name name -> Color.FromName(name.ToString())
        | SmallHex smallHex -> Color.FromArgb(int smallHex)
        | Hex hex -> Color.FromArgb(hex)
        | Values (r, g, b) -> Color.FromArgb(int r, int g, int b)
        | Percents (r, g, b) -> Color.FromArgb(int r, int g, int b)

    // TODO: Add opacity
    let penToDrawPen pen =
        new Pen((colorToDrawingColor pen.Color), (lengthToScreenFloat pen.Width))

    let drawPolyline pen (polyline : Polyline) (graphics : System.Drawing.Graphics) =
        let points =
            polyline.Points
            |> Seq.map pointToPointFloat
            |> Seq.toArray

        graphics.DrawLines((penToDrawPen pen), points)
        graphics

    let drawPolygon pen polygon (graphics : System.Drawing.Graphics) =
        drawPolyline pen (Polyline.ofPolygon polygon) graphics

    let drawRectangle pen (rect : SharpVG.Rect) (graphics : System.Drawing.Graphics) =
        let x, y = pointToXYInt rect.Position
        let width, height = areaToXYInt rect.Size
        let rectangleToDraw = Rectangle(x, y, width, height)

        graphics.DrawRectangle((penToDrawPen pen), rectangleToDraw)
        graphics

    let drawImage image (graphics : System.Drawing.Graphics) =
        let imageToDraw = Image.FromFile(image.Source)
        let x, y = pointToXYInt image.Position
        let width, height = areaToXYInt image.Size

        graphics.DrawImage(imageToDraw, x, y, width, height)
        graphics

    let drawEllipse pen (ellipse : SharpVG.Ellipse) (graphics : System.Drawing.Graphics) =
        let cx, cy = pointToXYInt ellipse.Center
        let rx, ry = pointToXYInt ellipse.Radius
        let x, y = cx - rx, cy - ry
        let width, height = 2 * rx, 2 * ry

        graphics.DrawEllipse((penToDrawPen pen), x, y, width, height)
        graphics

    let drawCircle pen circle (graphics : System.Drawing.Graphics) =
        drawEllipse pen (Ellipse.ofCircle circle) graphics

    let drawLine pen line (graphics : System.Drawing.Graphics) =
        let x1, y1 = pointToXYInt line.Point1
        let x2, y2 = pointToXYInt line.Point2

        graphics.DrawLine((penToDrawPen pen), x1, y1, x2, y2)
        graphics


