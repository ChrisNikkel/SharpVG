namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPreserveAspectRatio =

    [<Fact>]
    let ``create uniform`` () =
        let result = PreserveAspectRatio.create XMidYMid |> PreserveAspectRatio.toString
        Assert.Equal("xMidYMid", result)

    [<Fact>]
    let ``create with meet scale`` () =
        let result = PreserveAspectRatio.createWithScale XMinYMin Meet |> PreserveAspectRatio.toString
        Assert.Equal("xMinYMin meet", result)

    [<Fact>]
    let ``create with slice scale`` () =
        let result = PreserveAspectRatio.createWithScale XMaxYMax Slice |> PreserveAspectRatio.toString
        Assert.Equal("xMaxYMax slice", result)

    [<Fact>]
    let ``none`` () =
        let result = PreserveAspectRatio.none |> PreserveAspectRatio.toString
        Assert.Equal("none", result)

    [<Fact>]
    let ``toAttribute`` () =
        let result = PreserveAspectRatio.create XMidYMid |> PreserveAspectRatio.toAttribute |> Attribute.toString
        Assert.Equal("preserveAspectRatio=\"xMidYMid\"", result)

    [<Fact>]
    let ``Image withPreserveAspectRatio`` () =
        let result =
            Image.create Point.origin Area.full "test.jpg"
            |> Image.withPreserveAspectRatio (PreserveAspectRatio.create XMidYMid)
            |> Image.toString
        Assert.Contains("preserveAspectRatio=\"xMidYMid\"", result)

    [<Fact>]
    let ``Symbol withPreserveAspectRatio`` () =
        let result =
            Symbol.create (ViewBox.create Point.origin Area.full)
            |> Symbol.withPreserveAspectRatio (PreserveAspectRatio.createWithScale XMidYMid Meet)
            |> Symbol.toString
        Assert.Contains("preserveAspectRatio=\"xMidYMid meet\"", result)

    [<Fact>]
    let ``Pattern withPreserveAspectRatio`` () =
        let result =
            Pattern.create "pat1"
            |> Pattern.withPreserveAspectRatio (PreserveAspectRatio.create XMinYMin)
            |> Pattern.toString
        Assert.Contains("preserveAspectRatio=\"xMinYMin\"", result)

    [<Fact>]
    let ``Marker withPreserveAspectRatio`` () =
        let result =
            Marker.create "arrow"
            |> Marker.withPreserveAspectRatio PreserveAspectRatio.none
            |> Marker.toString
        Assert.Contains("preserveAspectRatio=\"none\"", result)

    [<Fact>]
    let ``Svg withPreserveAspectRatio`` () =
        let result =
            Element.create (Circle.create Point.origin (Length.ofInt 10))
            |> Svg.ofElement
            |> Svg.withPreserveAspectRatio (PreserveAspectRatio.createWithScale XMidYMid Slice)
            |> Svg.toString
        Assert.Contains("preserveAspectRatio=\"xMidYMid slice\"", result)
