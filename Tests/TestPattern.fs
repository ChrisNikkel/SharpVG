namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPattern =

    [<Fact>]
    let ``Pattern create empty`` () =
        let result = Pattern.create "myPattern" |> Pattern.toString
        Assert.Equal("<pattern id=\"myPattern\"></pattern>", result)

    [<Fact>]
    let ``Pattern create has correct id`` () =
        let result = Pattern.create "testPattern" |> Pattern.toString
        Assert.Contains("id=\"testPattern\"", result)

    [<Fact>]
    let ``Pattern ofElement creates with element and size`` () =
        let circle = Circle.create (Point.ofInts (10, 10)) (Length.ofInt 5) |> Element.create
        let size = Area.ofInts (20, 20)
        let result = Pattern.ofElement "myPattern" size circle |> Pattern.toString
        Assert.Contains("id=\"myPattern\"", result)
        Assert.Contains("width=\"20\"", result)
        Assert.Contains("height=\"20\"", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``Pattern withPosition adds x y attributes`` () =
        let result = Pattern.create "myPattern" |> Pattern.withPosition (Point.ofInts (5, 10)) |> Pattern.toString
        Assert.Contains("x=\"5\"", result)
        Assert.Contains("y=\"10\"", result)

    [<Fact>]
    let ``Pattern withPatternUnits userSpaceOnUse`` () =
        let result = Pattern.create "myPattern" |> Pattern.withPatternUnits UserSpaceOnUse |> Pattern.toString
        Assert.Contains("patternUnits=\"userSpaceOnUse\"", result)

    [<Fact>]
    let ``Pattern withPatternUnits objectBoundingBox`` () =
        let result = Pattern.create "myPattern" |> Pattern.withPatternUnits ObjectBoundingBox |> Pattern.toString
        Assert.Contains("patternUnits=\"objectBoundingBox\"", result)

    [<Fact>]
    let ``Pattern withPatternContentUnits objectBoundingBox`` () =
        let result = Pattern.create "myPattern" |> Pattern.withPatternContentUnits ObjectBoundingBox |> Pattern.toString
        Assert.Contains("patternContentUnits=\"objectBoundingBox\"", result)

    [<Fact>]
    let ``Pattern withPatternContentUnits userSpaceOnUse`` () =
        let result = Pattern.create "myPattern" |> Pattern.withPatternContentUnits UserSpaceOnUse |> Pattern.toString
        Assert.Contains("patternContentUnits=\"userSpaceOnUse\"", result)

    [<Fact>]
    let ``Pattern withPatternTransform`` () =
        let transform = Transform.createRotate 45.0 Length.empty Length.empty
        let result = Pattern.create "myPattern" |> Pattern.withPatternTransform transform |> Pattern.toString
        Assert.Contains("patternTransform=", result)

    [<Fact>]
    let ``Pattern withViewBox`` () =
        let viewBox = ViewBox.create Point.origin (Area.ofInts (100, 100))
        let result = Pattern.create "myPattern" |> Pattern.withViewBox viewBox |> Pattern.toString
        Assert.Contains("viewBox=", result)

    [<Fact>]
    let ``Pattern addElement adds an element`` () =
        let rect = Rect.create Point.origin (Area.ofInts (10, 10)) |> Element.create
        let result = Pattern.create "myPattern" |> Pattern.addElement rect |> Pattern.toString
        Assert.Contains("<rect", result)

    [<Fact>]
    let ``Pattern addElements adds multiple elements`` () =
        let rect = Rect.create Point.origin Area.full |> Element.create
        let circle = Circle.create (Point.ofInts (5, 5)) (Length.ofInt 3) |> Element.create
        let result = Pattern.create "myPattern" |> Pattern.addElements [rect; circle] |> Pattern.toString
        Assert.Contains("<rect", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``Pattern ofElements creates with size and multiple elements`` () =
        let rect = Rect.create Point.origin Area.full |> Element.create
        let circle = Circle.create (Point.ofInts (5, 5)) (Length.ofInt 3) |> Element.create
        let size = Area.ofInts (20, 20)
        let result = Pattern.ofElements "myPattern" size [rect; circle] |> Pattern.toString
        Assert.Contains("id=\"myPattern\"", result)
        Assert.Contains("width=\"20\"", result)
        Assert.Contains("height=\"20\"", result)
        Assert.Contains("<rect", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``Pattern toString produces valid pattern tag`` () =
        let rect = Rect.create Point.origin (Area.ofInts (10, 10)) |> Element.create
        let result = Pattern.ofElement "pat1" (Area.ofInts (10, 10)) rect |> Pattern.toString
        Assert.StartsWith("<pattern", result)
        Assert.EndsWith("</pattern>", result)
