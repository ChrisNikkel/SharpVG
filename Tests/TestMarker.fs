namespace SharpVG.Tests

open SharpVG
open Xunit

module TestMarker =

    [<Fact>]
    let ``Marker create empty`` () =
        let result = Marker.create "arrow" |> Marker.toString
        Assert.Equal("<marker id=\"arrow\"></marker>", result)

    [<Fact>]
    let ``Marker create has correct id`` () =
        let result = Marker.create "myMarker" |> Marker.toString
        Assert.Contains("id=\"myMarker\"", result)

    [<Fact>]
    let ``Marker withViewBox`` () =
        let viewBox = ViewBox.create Point.origin (Area.ofInts (10, 10))
        let result = Marker.create "arrow" |> Marker.withViewBox viewBox |> Marker.toString
        Assert.Contains("viewBox=", result)

    [<Fact>]
    let ``Marker withRefPoint`` () =
        let result = Marker.create "arrow" |> Marker.withRefPoint (Point.ofInts (5, 5)) |> Marker.toString
        Assert.Contains("refX=\"5\"", result)
        Assert.Contains("refY=\"5\"", result)

    [<Fact>]
    let ``Marker withSize`` () =
        let result = Marker.create "arrow" |> Marker.withSize (Area.ofInts (10, 10)) |> Marker.toString
        Assert.Contains("width=\"10\"", result)
        Assert.Contains("height=\"10\"", result)

    [<Fact>]
    let ``Marker withUnits strokeWidth`` () =
        let result = Marker.create "arrow" |> Marker.withUnits StrokeWidthUnits |> Marker.toString
        Assert.Contains("markerUnits=\"strokeWidth\"", result)

    [<Fact>]
    let ``Marker withUnits userSpaceOnUse`` () =
        let result = Marker.create "arrow" |> Marker.withUnits UserSpaceOnUseUnits |> Marker.toString
        Assert.Contains("markerUnits=\"userSpaceOnUse\"", result)

    [<Fact>]
    let ``Marker withOrient auto`` () =
        let result = Marker.create "arrow" |> Marker.withOrient AutoOrient |> Marker.toString
        Assert.Contains("orient=\"auto\"", result)

    [<Fact>]
    let ``Marker withOrient angle`` () =
        let result = Marker.create "arrow" |> Marker.withOrient (AngleOrient 45.0) |> Marker.toString
        Assert.Contains("orient=\"45\"", result)

    [<Fact>]
    let ``Marker addElement adds body`` () =
        let path = Path.empty |> Path.addMoveTo Absolute (Point.ofInts (0, 0)) |> Element.create
        let result = Marker.create "arrow" |> Marker.addElement path |> Marker.toString
        Assert.Contains("<path", result)

    [<Fact>]
    let ``Marker addElements adds multiple elements`` () =
        let rect = Rect.create Point.origin (Area.ofInts (5, 5)) |> Element.create
        let circle = Circle.create (Point.ofInts (2, 2)) (Length.ofInt 2) |> Element.create
        let result = Marker.create "marker" |> Marker.addElements [rect; circle] |> Marker.toString
        Assert.Contains("<rect", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``Marker toString produces valid marker tag`` () =
        let result = Marker.create "arrow" |> Marker.toString
        Assert.StartsWith("<marker", result)
        Assert.EndsWith("</marker>", result)

    [<Fact>]
    let ``Marker withRefPoint zero origin`` () =
        let result = Marker.create "arrow" |> Marker.withRefPoint Point.origin |> Marker.toString
        Assert.Contains("refX=\"0\"", result)
        Assert.Contains("refY=\"0\"", result)

    [<Fact>]
    let ``MarkerUnits StrokeWidthUnits toString`` () =
        Assert.Equal("strokeWidth", StrokeWidthUnits.ToString())

    [<Fact>]
    let ``MarkerUnits UserSpaceOnUseUnits toString`` () =
        Assert.Equal("userSpaceOnUse", UserSpaceOnUseUnits.ToString())

    [<Fact>]
    let ``MarkerOrient AutoOrient toString`` () =
        Assert.Equal("auto", AutoOrient.ToString())

    [<Fact>]
    let ``MarkerOrient AngleOrient toString`` () =
        Assert.Equal("90", (AngleOrient 90.0).ToString())
