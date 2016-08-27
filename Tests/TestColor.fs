namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks
open Swensen.Unquote

module TestArea =

    [<Fact>]
    let ``create with name`` () =
        test <| <@ Color.createWithName Colors.Snow |> Color.toString = "snow" @>

    [<Fact>]
    let ``create with percents`` () =
        test <| <@ Color.createWithPercents (10.0, 10.0, 10.0) |> Color.toString = "(10%,10%,10%)" @>
