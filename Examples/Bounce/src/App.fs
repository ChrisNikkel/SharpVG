module Bounce

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open SharpVG

let init() =
    let content = Browser.document.getElementById "content"
    let area = Area.ofInts (55, 50)
    let rect1 = Rect.create (Point.ofInts (10, 10)) area
    let rect2 = Rect.create (Point.ofInts (30, 30)) area
    let style1 = Style.createWithFill (Color.ofValues (200uy, 0uy, 0uy))
    let style2 =
      Style.createWithFill (Color.ofValues (0uy, 0uy, 200uy))
        |> Style.withOpacity 0.5
    let element1 = Element.ofRect rect1 |> Element.withStyle style1
    let element2 = Element.ofRect rect2 |> Element.withStyle style2

    let svg =
      [element1; element2]
        |> Group.ofList
        |> Svg.ofGroup
        |> Svg.toString

    content.innerHTML <- svg
init()