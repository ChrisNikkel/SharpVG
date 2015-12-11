module Tests
  open SharpVG
  open Xunit
  open FsCheck.Xunit

  [<Fact>]
  let ``true is true`` () =
    true

  [<Property>]
  let ``true is true again`` () =
    true

  let isTagEnclosed (tag:string) =
    let trimmedTag = tag.Trim()
    trimmedTag.[0..0] = "<" && trimmedTag.[(trimmedTag.Length - 2)..(trimmedTag.Length - 1)] = "/>"

  [<Property>]
  let ``draw lines`` (x1, y1, x2, y2) =
    let point1 = {X = Size.Pixels(x1); Y = Size.Pixels(y1)}
    let point2 = {X = Size.Pixels(x2); Y = Size.Pixels(y2)}
    let lineTag = line {Stroke = (Hex(0xff0000)); StrokeWidth = Pixels(3); Fill = Color.Name(Colors.Red); } point1 point2
    isTagEnclosed lineTag

  [<Fact>]
  let ``do lots and don't fail`` () =
    // Test
    let points = seq {
      yield {X = Size.Pixels(1); Y = Size.Pixels(1)}
      yield {X = Size.Pixels(4); Y = Size.Pixels(4)}
      yield {X = Size.Pixels(10); Y = Size.Pixels(10)}
    }
    let point = {X = Size.Pixels(24); Y = Size.Pixels(15)}
    let size = {Height = Size.Pixels(30); Width = Size.Pixels(30)}
    let style1 = {Stroke = (Hex(0xff0000)); StrokeWidth = Pixels(3); Fill = Color.Name(Colors.Red); }
    let style2 = {Stroke = (SmallHex(0xf00s)); StrokeWidth = Pixels(6); Fill = Color.Name(Colors.Blue); }


    let graphics = seq {
      yield image point size "myimage.jpg"
      yield text style1 point "Hello World!"
      yield text style2 point "Hello World!"
      yield polygon style1 points
      yield polyline style2 points
      yield line style1 point point
      yield circle style2 point (Pixels(2))
      yield ellipse style1 point point
      yield rect style2 point size
      yield script """
      function circle_click(evt) {
        var circle = evt.target;
        var currentRadius = circle.getAttribute("r");
        if (currentRadius == 100)
          circle.setAttribute("r", currentRadius*2);
        else
          circle.setAttribute("r", currentRadius*0.5);
      }
    """
    }

    let styleBody = style
    let svgBody = graphics |> String.concat "\n  " |> (svg size)
    let body = seq {
      yield styleBody
      yield svgBody
    }
    body |> String.concat "\n" |> (html "SVG Demo") |> (printfn "%s")

    true
