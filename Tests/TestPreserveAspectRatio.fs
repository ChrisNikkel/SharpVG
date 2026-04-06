namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

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

    [<Property>]
    let ``create with any AspectRatioAlign always produces preserveAspectRatio attribute`` (align: AspectRatioAlign) =
        let result = PreserveAspectRatio.create align |> PreserveAspectRatio.toAttribute |> Attribute.toString
        result.StartsWith("preserveAspectRatio=\"") && result.EndsWith("\"")

    [<Property>]
    let ``createWithScale with any align and scale always produces valid attribute`` (align: AspectRatioAlign, scale: AspectRatioScale) =
        let result = PreserveAspectRatio.createWithScale align scale |> PreserveAspectRatio.toAttribute |> Attribute.toString
        result.Contains(align.ToString()) && result.Contains(scale.ToString())

    [<Property>]
    let ``toString always contains the align value`` (align: AspectRatioAlign) =
        let result = PreserveAspectRatio.create align |> PreserveAspectRatio.toString
        result = align.ToString()

    [<Property>]
    let ``Uniform preserveAspectRatio is never equal to none`` (align: AspectRatioAlign) =
        let result = PreserveAspectRatio.create align |> PreserveAspectRatio.toString
        result <> "none"

    // Wiki: PreserveAspectRatio page — string values table
    [<Fact>]
    let ``PreserveAspectRatio wiki - create XMidYMid produces xMidYMid`` () =
        let par = PreserveAspectRatio.create XMidYMid
        Assert.Equal("xMidYMid", PreserveAspectRatio.toString par)

    [<Fact>]
    let ``PreserveAspectRatio wiki - createWithScale XMidYMid Slice produces xMidYMid slice`` () =
        let parSlice = PreserveAspectRatio.createWithScale XMidYMid Slice
        Assert.Equal("xMidYMid slice", PreserveAspectRatio.toString parSlice)

    [<Fact>]
    let ``PreserveAspectRatio wiki - none produces none`` () =
        Assert.Equal("none", PreserveAspectRatio.toString PreserveAspectRatio.none)

    // Wiki: PreserveAspectRatio page — symbol with preserveAspectRatio
    [<Fact>]
    let ``PreserveAspectRatio wiki - symbol with par renders attribute`` () =
        let par = PreserveAspectRatio.create XMidYMid
        let viewBox = ViewBox.create Point.origin (Area.ofInts (100, 100))
        let symbol = Symbol.create viewBox |> Symbol.withPreserveAspectRatio par
        let result = Symbol.toString symbol
        Assert.Contains("preserveAspectRatio=\"xMidYMid\"", result)
