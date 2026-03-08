namespace SharpVG.Tests

open SharpVG
open Xunit

module TestGraphic =

    [<Fact>]
    let ``create graphic`` () =
        let ellipse = Ellipse.create Point.origin Point.origin
        let graphic = Graphic.ofEllipse ellipse
        Assert.Equal("<ellipse cx=\"0\" cy=\"0\" rx=\"0\" ry=\"0\"/>",  graphic |> Graphic.toElement |> Element.toString)

    [<Fact>]
    let ``graphic ofCircle`` () =
        let center = Point.ofInts (10, 10)
        let radius = Length.ofInt 5
        let result = Circle.create center radius |> Graphic.ofCircle |> Graphic.toElement |> Element.toString
        Assert.Contains("<circle", result)
        Assert.Contains("r=\"5\"", result)

    [<Fact>]
    let ``graphic ofLine`` () =
        let startPoint = Point.ofInts (0, 0)
        let endPoint = Point.ofInts (100, 100)
        let result = Line.create startPoint endPoint |> Graphic.ofLine |> Graphic.toElement |> Element.toString
        Assert.Contains("<line", result)

    [<Fact>]
    let ``graphic ofRect`` () =
        let position = Point.ofInts (10, 10)
        let area = Area.ofInts (50, 30)
        let result = Rect.create position area |> Graphic.ofRect |> Graphic.toElement |> Element.toString
        Assert.Contains("<rect", result)
        Assert.Contains("width=\"50\"", result)

    [<Fact>]
    let ``graphic ofPolygon`` () =
        let points = [ Point.ofInts (0, 0); Point.ofInts (50, 0); Point.ofInts (25, 50) ]
        let result = Polygon.ofList points |> Graphic.ofPolygon |> Graphic.toElement |> Element.toString
        Assert.Contains("<polygon", result)
        Assert.Contains("points=", result)

    [<Fact>]
    let ``graphic ofPolyline`` () =
        let points = [ Point.ofInts (0, 0); Point.ofInts (50, 50); Point.ofInts (100, 0) ]
        let result = Polyline.ofList points |> Graphic.ofPolyline |> Graphic.toElement |> Element.toString
        Assert.Contains("<polyline", result)

    [<Fact>]
    let ``graphic ofText`` () =
        let position = Point.ofInts (10, 20)
        let result = Text.create position "Hello" |> Graphic.ofText |> Graphic.toElement |> Element.toString
        Assert.Contains("<text", result)
        Assert.Contains("Hello", result)

    [<Fact>]
    let ``graphic ofImage`` () =
        let position = Point.ofInts (0, 0)
        let size = Area.ofInts (100, 100)
        let result = Image.create position size "photo.jpg" |> Graphic.ofImage |> Graphic.toElement |> Element.toString
        Assert.Contains("<image", result)
        Assert.Contains("photo.jpg", result)

    [<Fact>]
    let ``graphic ofPath`` () =
        let startingPoint = Point.ofInts (10, 10)
        let result = Path.empty |> Path.addMoveTo Absolute startingPoint |> Graphic.ofPath |> Graphic.toElement |> Element.toString
        Assert.Contains("<path", result)
