namespace SharpVG

type Symbol =
    {
        ViewBox : ViewBox
        Size : Area option
        Position : Point option
        Reference : Point option
    }
with
    static member ToTag symbol =
        Tag.create "symbol"
        |> Tag.withAttributes (ViewBox.toAttributes symbol.ViewBox)
        |> Tag.addAttributes (match symbol.Size with Some(s) -> Area.toAttributes s | _ -> [])
        |> Tag.addAttributes (match symbol.Position with Some(p) -> Point.toAttributes p | _ -> [])
        |> Tag.addAttributes (match symbol.Reference with Some(r) -> Point.toAttributesWithModifier "r" "" r | _ -> [])

    override this.ToString() =
       this |> Symbol.ToTag |> Tag.toString

module Symbol =

    let create viewBox =
        { ViewBox = viewBox; Size = None; Reference = None; Position = None }

    let withSize size (symbol : Symbol) =
        { symbol with Size = size }

    let withPosition position (symbol : Symbol) =
        { symbol with Position = position }

    let withReference reference (symbol : Symbol) =
        { symbol with Reference = reference }

    let toTag =
        Symbol.ToTag

    let toString (symbol : Symbol) =
        symbol.ToString()