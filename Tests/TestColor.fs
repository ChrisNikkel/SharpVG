namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

module TestColor =

    [<Fact>]
    let ``create with name`` () =
        test <| <@ Color.createWithName Colors.Snow |> Color.toString = "snow" @>

    [<Fact>]
    let ``create with small hex`` () =
        test <| <@ Color.createWithSmallHex 0x123s |> Color.toString = "0x123" @>

    [<Fact>]
    let ``create with hex`` () =
        test <| <@ Color.createWithHex 0x112233 |> Color.toString = "0x112233" @>

    [<Fact>]
    let ``create with percents`` () =
        test <| <@ Color.createWithPercents (10.0, 10.0, 10.0) |> Color.toString = "(10%,10%,10%)" @>

    [<Property>]
    let ``create with values`` (r, g, b) =
        test <| <@ Color.createWithValues (r, g, b) |> Color.toString = sprintf "(%d,%d,%d)" r g b @>
