module Bounce

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open SharpVG

let createRect p c o =
    let area = Area.ofInts (55, 50)
    let style = Style.createWithFill c |> Style.withOpacity 0.5
    let rect = Rect.create (Point.ofInts p) area
    Element.ofRect rect |> Element.withStyle style

let render pos1 pos2 =
  let rect1 = createRect pos1 <| Color.ofValues (200uy, 0uy, 0uy) <| 1.0
  let rect2 = createRect pos2 <| Color.ofValues (0uy, 0uy, 200uy) <| 0.5
  [rect1; rect2]
    |> Group.ofList
    |> Svg.ofGroup
    |> Svg.toString

let rec update position () =
  let (pos1, pos2) = position
  (Browser.document.getElementById "content").innerHTML <- render pos1 pos2

let init() =
    let position = ((10, 10), (30, 30))
    update position ()


init()