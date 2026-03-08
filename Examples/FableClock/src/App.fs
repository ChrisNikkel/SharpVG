module App

open SharpVG
open System

// ---------------------------------------------------------------------------
// Styles
// All clock shapes share a 200x200 coordinate space, centered at (100, 100).
// ---------------------------------------------------------------------------

let private center = Point.ofInts (100, 100)
let private pivot  = Length.ofInt 100

let private faceStyle =
    let strokeColor = Color.ofName Colors.Black
    let fillColor   = Color.ofName Colors.White
    let penWidth    = Length.ofInt 2
    let strokePen   = Pen.createWithOpacityAndWidth strokeColor 1.0 penWidth
    let fillPen     = Pen.create fillColor
    Style.createWithPen strokePen |> Style.withFillPen fillPen

// ---------------------------------------------------------------------------
// Static elements — built once, reused every render
// ---------------------------------------------------------------------------

// Twelve tick marks, each rotated about the center point.
let private hourMarkers =
    let strokeColor = Color.ofName Colors.Black
    let penWidth    = Length.ofInt 2
    let style       = Style.createWithStroke strokeColor |> Style.withStrokeWidth penWidth
    [ for i in 0 .. 11 do
        let angle  = float i * 30.0
        let inner  = Point.ofInts (100, 14)
        let outer  = Point.ofInts (100, 24)
        let marker =
            Line.create inner outer
            |> Element.createWithStyle style
            |> Element.withTransform (Transform.createRotate angle pivot pivot)
        yield marker ]

// ---------------------------------------------------------------------------
// Dynamic elements — recreated each second with the current time
// ---------------------------------------------------------------------------

// A clock hand is a line from the center rotated to the given angle.
// tipY controls length (smaller y = longer hand; SVG y increases downward).
let private hand tipY width color angle =
    let strokeColor = Color.ofName color
    let penWidth    = Length.ofInt width
    let strokePen   = Pen.createWithWidth strokeColor penWidth
    let style       = Style.createWithPen strokePen
    let line        = Line.create center (Point.ofInts (100, tipY))
    let element     = line |> Element.createWithStyle style
    element |> Element.withTransform (Transform.createRotate angle pivot pivot)

// ---------------------------------------------------------------------------
// Render loop
// ---------------------------------------------------------------------------

let private render () =
    let now          = DateTime.Now
    let secondAngle  = float now.Second * 6.0
    let minuteAngle  = float now.Minute * 6.0  + float now.Second * 0.1
    let hourAngle    = float (now.Hour % 12) * 30.0 + float now.Minute * 0.5

    let face       = Circle.create center (Length.ofInt 90) |> Element.createWithStyle faceStyle
    let hourHand   = hand 50 5 Colors.Black hourAngle
    let minuteHand = hand 35 3 Colors.Black minuteAngle
    let secondHand = hand 28 1 Colors.Red   secondAngle
    let dotStyle   = Style.createWithFill (Color.ofName Colors.Black)
    let centerDot  = Circle.create center (Length.ofInt 4) |> Element.createWithStyle dotStyle

    let size = Area.ofInts (200, 200)
    let svg =
        [ face ] @ hourMarkers @ [ hourHand; minuteHand; secondHand; centerDot ]
        |> Svg.ofList
        |> Svg.withSize size
        |> Svg.toString

    // The key Fable integration point: inject the SVG string into the DOM.
    // We qualify Browser.Dom explicitly to avoid shadowing SharpVG.Element.
    Browser.Dom.document.getElementById("app").innerHTML <- svg

do
    render ()
    Browser.Dom.window.setInterval((fun () -> render ()), 1000) |> ignore
