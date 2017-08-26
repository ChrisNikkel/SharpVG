namespace SharpVG.Tests

open SharpVG
open Xunit

module TestStyle =

    [<Fact>]
    let ``create style with fill`` () =
        Assert.Equal("fill:white", Style.empty |> Style.withFill (Name Colors.White) |> Style.toString)

    [<Fact>]
    let ``create style with stroke`` () =
        Assert.Equal("stroke:white", Style.empty |> Style.withStroke (Name Colors.White) |> Style.toString)

    [<Fact>]
    let ``create style with stroke width`` () =
        Assert.Equal("stroke:white;stroke-width:0", Style.empty |> Style.withStroke (Name Colors.White) |> Style.withStrokeWidth Length.empty |> Style.toString)

    [<Fact>]
    let ``create style with opacity`` () =
        Assert.Equal("opacity:0.5", Style.empty |> Style.withOpacity 0.5 |> Style.toString)

