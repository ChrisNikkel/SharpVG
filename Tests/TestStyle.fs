namespace SharpVG.Tests

open SharpVG
open Xunit

module TestStyle =

    [<Fact>]
    let ``create style with fill`` () =
        Assert.Equal(Style.empty |> Style.withFill (Name Colors.White) |> Style.toString, "fill:white")

    [<Fact>]
    let ``create style with stroke`` () =
        Assert.Equal(Style.empty |> Style.withStroke (Name Colors.White) |> Style.toString, "stroke:white")

    [<Fact>]
    let ``create style with stroke width`` () =
        Assert.Equal(Style.empty |> Style.withStroke (Name Colors.White) |> Style.withStrokeWidth Length.empty |> Style.toString, "stroke:white;stroke-width:0")

    [<Fact>]
    let ``create style with opacity`` () =
        Assert.Equal(Style.empty |> Style.withOpacity 0.5 |> Style.toString, "opacity:0.5")

