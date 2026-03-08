namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPolyline =

    [<Fact>]
    let ``create polyline from list`` () =
        let points = [ Point.ofInts (0, 0); Point.ofInts (10, 10); Point.ofInts (20, 0) ]
        Assert.Equal("<polyline points=\"0,0 10,10 20,0\"/>", Polyline.ofList points |> Polyline.toString)

    [<Fact>]
    let ``create polyline from seq`` () =
        let points = seq { yield Point.ofInts (0, 0); yield Point.ofInts (50, 50) }
        Assert.Equal("<polyline points=\"0,0 50,50\"/>", Polyline.ofSeq points |> Polyline.toString)

    [<Fact>]
    let ``create polyline from array`` () =
        let points = [| Point.ofInts (5, 5); Point.ofInts (15, 10); Point.ofInts (25, 5) |]
        Assert.Equal("<polyline points=\"5,5 15,10 25,5\"/>", Polyline.ofArray points |> Polyline.toString)

    [<Fact>]
    let ``create polyline using create`` () =
        let points = seq { yield Point.ofInts (1, 2); yield Point.ofInts (3, 4) }
        Assert.Equal("<polyline points=\"1,2 3,4\"/>", Polyline.create points |> Polyline.toString)

    [<Fact>]
    let ``polyline as element with style`` () =
        let points = [ Point.ofInts (0, 0); Point.ofInts (100, 100) ]
        let strokeColor = Color.ofName Colors.Red
        let penWidth = Length.ofInt 2
        let strokePen = Pen.createWithWidth strokeColor penWidth
        let style = Style.createWithPen strokePen
        let result = Polyline.ofList points |> Element.createWithStyle style |> Element.toString
        Assert.Contains("polyline", result)
        Assert.Contains("stroke=\"red\"", result)
        Assert.Contains("points=\"0,0 100,100\"", result)

    [<Fact>]
    let ``polyline in SVG`` () =
        let points = [ Point.ofInts (0, 0); Point.ofInts (50, 50); Point.ofInts (100, 0) ]
        let result = Polyline.ofList points |> Element.create |> Svg.ofElement |> Svg.toString
        Assert.Contains("<polyline", result)
        Assert.Contains("<svg", result)
