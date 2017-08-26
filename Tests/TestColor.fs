namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit

module TestColor =

    [<Fact>]
    let ``create with name`` () =
        Assert.Equal("snow", Color.ofName Colors.Snow |> Color.toString)

    [<Fact>]
    let ``create with small hex`` () =
        Assert.Equal("0x123", Color.ofSmallHex 0x123s |> Color.toString)

    [<Fact>]
    let ``create with hex`` () =
        Assert.Equal("0x112233", Color.ofHex 0x112233 |> Color.toString)

    [<Fact>]
    let ``create with percents`` () =
        Assert.Equal("(10%,10%,10%)", Color.ofPercents (10.0, 10.0, 10.0) |> Color.toString)

    [<Property>]
    let ``create with values`` (r, g, b) =
        Assert.Equal(sprintf "(%d,%d,%d)" r g b, Color.ofValues (r, g, b) |> Color.toString)
