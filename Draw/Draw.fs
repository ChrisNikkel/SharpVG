namespace SharpVG.Draw
open SharpVG
open System.Drawing

module Draw =
    let lengthToScreen (length : SharpVG.Length) =
        match length with
        | UserSpace(f) -> int f
        | Pixels(i) -> i
        | Em(f) -> int f
        | Percent(f) -> int f


    let areaToScreenXY area =
        (lengthToScreen area.Width, lengthToScreen area.Height)

    let positionToScreenXY position =
        (lengthToScreen position.X, lengthToScreen position.Y)

    let drawRectangle (graphics : System.Drawing.Graphics) (rect : SharpVG.Rect) () =
        let x, y = positionToScreenXY rect.Position
        let width, height = areaToScreenXY rect.Size
        let rectangleToDraw = System.Drawing.Rectangle(x, y, width, height)
        let pen = new System.Drawing.Pen(Color.ForestGreen, 4.0F)

        graphics.DrawRectangle(pen, rectangleToDraw)
