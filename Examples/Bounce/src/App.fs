module Bounce

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open SharpVG

let sizeToArea size = Area.ofInts (size, size)

let fieldSize = 500
let fieldArea = sizeToArea fieldSize

let boxSize = 50
let boxArea = sizeToArea boxSize

let outerBounds = fieldSize - boxSize - 1

let red, blue = Color.ofValues (200uy, 0uy, 0uy), Color.ofValues (0uy, 0uy, 200uy)

let opaque, translucent = 1.0, 0.5

let createRect p c o =
    let style = Style.createWithFill c |> Style.withOpacity 0.5
    let rect = Rect.create (Point.ofInts p) boxArea
    Element.ofRect rect |> Element.withStyle style

let render position =
    let pos1, pos2 = position
    let rect1 = createRect pos1 red opaque
    let rect2 = createRect pos2 blue translucent
    [rect1; rect2]
        |> Group.ofList
        |> Svg.ofGroup
        |> Svg.withSize fieldArea
        |> Svg.toString

let getElement(name) = Browser.document.getElementById name

let getDirection position (dx, dy) =
    match position with
        | (x, y) when (x <= 0 || x >= outerBounds) && (y <= 0 || y >= outerBounds) -> (-1 * dx, -1 * dy)
        | (x, _) when (x <= 0 || x >= outerBounds) -> (-1 * dx, dy)
        | (_, y) when (y <= 0 || y >= outerBounds) -> (dx, -1 * dy)
        | (_, _) -> (dx, dy)


let add (x1, y1) (x2, y2) =
    (x1 + x2, y1 + y2)

let move (position, direction) =
    let point1, point2 = position
    let direction1, direction2 = direction
    let (newDirection1, newDirection2) = (getDirection point1 direction1, getDirection point2 direction2)

    ((add point1 newDirection1, add point2 newDirection2), (newDirection1, newDirection2))

let rec update position direction () =
    let (newPosition, newDirection) = move(position, direction)
    getElement("content").innerHTML <- render newPosition
    window.setTimeout(update newPosition newDirection, 1000. / 60.) |> ignore

let init() =
    let position = ((10, 10), (30, 30))
    let direction = ((1, -1), (-1, 1))
    update position direction ()

init()