namespace SharpVG.Tests

open SharpVG
open Xunit

module TestClipPath =

    [<Fact>]
    let ``ClipPath create empty`` () =
        let result = ClipPath.create "myClip" |> ClipPath.toString
        Assert.Contains("<clipPath", result)
        Assert.Contains("id=\"myClip\"", result)

    [<Fact>]
    let ``ClipPath create empty has no children`` () =
        let result = ClipPath.create "myClip" |> ClipPath.toString
        Assert.Equal("<clipPath id=\"myClip\"></clipPath>", result)

    [<Fact>]
    let ``ClipPath ofElement contains element`` () =
        let circle = Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30) |> Element.create
        let result = ClipPath.ofElement "myClip" circle |> ClipPath.toString
        Assert.Contains("id=\"myClip\"", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``ClipPath withClipPathUnits userSpaceOnUse`` () =
        let result = ClipPath.create "myClip" |> ClipPath.withClipPathUnits UserSpaceOnUse |> ClipPath.toString
        Assert.Contains("clipPathUnits=\"userSpaceOnUse\"", result)

    [<Fact>]
    let ``ClipPath withClipPathUnits objectBoundingBox`` () =
        let result = ClipPath.create "myClip" |> ClipPath.withClipPathUnits ObjectBoundingBox |> ClipPath.toString
        Assert.Contains("clipPathUnits=\"objectBoundingBox\"", result)

    [<Fact>]
    let ``ClipPath addElement adds single element`` () =
        let rect = Rect.create Point.origin Area.full |> Element.create
        let result = ClipPath.create "myClip" |> ClipPath.addElement rect |> ClipPath.toString
        Assert.Contains("<rect", result)

    [<Fact>]
    let ``ClipPath addElements adds multiple elements`` () =
        let rect = Rect.create Point.origin Area.full |> Element.create
        let circle = Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30) |> Element.create
        let result = ClipPath.create "myClip" |> ClipPath.addElements [rect; circle] |> ClipPath.toString
        Assert.Contains("<rect", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``ClipPath ofElements creates with multiple elements`` () =
        let rect = Rect.create Point.origin Area.full |> Element.create
        let circle = Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30) |> Element.create
        let result = ClipPath.ofElements "myClip" [rect; circle] |> ClipPath.toString
        Assert.Contains("id=\"myClip\"", result)
        Assert.Contains("<rect", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``ClipPath toString produces valid clipPath`` () =
        let rect = Rect.create Point.origin (Area.ofInts (100, 100)) |> Element.create
        let result = ClipPath.ofElement "clip1" rect |> ClipPath.toString
        Assert.StartsWith("<clipPath", result)
        Assert.EndsWith("</clipPath>", result)
