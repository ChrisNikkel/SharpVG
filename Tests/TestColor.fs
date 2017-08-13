namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit

module TestColor =

    [<Fact>]
    let ``create with name`` () =
        Assert.Equal(Color.createWithName Colors.Snow |> Color.toString, "snow")

    [<Fact>]
    let ``create with small hex`` () =
        Assert.Equal(Color.createWithSmallHex 0x123s |> Color.toString, "0x123")

    [<Fact>]
    let ``create with hex`` () =
        Assert.Equal(Color.createWithHex 0x112233 |> Color.toString, "0x112233")

    [<Fact>]
    let ``create with percents`` () =
        Assert.Equal(Color.createWithPercents (10.0, 10.0, 10.0) |> Color.toString, "(10%,10%,10%)")

    [<Property>]
    let ``create with values`` (r, g, b) =
        Assert.Equal(Color.createWithValues (r, g, b) |> Color.toString, sprintf "(%d,%d,%d)" r g b)
