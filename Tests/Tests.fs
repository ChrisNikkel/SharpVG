open SharpVG
open Xunit
open FsCheck.Xunit

module Tests =  
  [<Fact>]
  let ``true is true`` () =
    true
    
  [<Property>]
  let ``true is true again`` () =
    true

  [<EntryPoint>]
  let main argv =
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
  
    0 // return an integer exit code