namespace SharpVG.Tests
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks
open Swensen.Unquote

module TestLength =

    [<Fact>]
    let ``create empty user space`` () =
        test <| <@ (Length.empty |> Length.toString) = "0" @>

    [<Fact>]
    let ``create full percent`` () =
        test <| <@ (Length.full |> Length.toString) = "100%" @>

    [<Fact>]
    let ``create user space`` () =
        test <| <@ 2.0 |> Length.ofUserSpace |> Length.toString = "2" @>
        test <| <@ 2.1 |> Length.ofUserSpace |> Length.toString = "2.1" @>

    [<Fact>]
    let ``create pixels`` () =
        test <| <@ 2 |> Length.ofPixels |> Length.toString = "2px" @>

    [<Fact>]
    let ``create em`` () =
        test <| <@ 2.0 |> Length.ofEm |> Length.toString = "2em" @>
        test <| <@ 2.1 |> Length.ofEm |> Length.toString = "2.1em" @>

    [<Fact>]
    let ``create percent`` () =
        test <| <@ 2.0 |> Length.ofPercent |> Length.toString = "2%" @>
        test <| <@ 2.1 |> Length.ofPercent |> Length.toString = "2.1%" @>

    [<SvgProperty>]
    let ``what goes in must come out`` (x) =
        test <| <@ x |> Length.ofUserSpace |> Length.toFloat = x @>
        test <| <@ x |> Length.ofEm |> Length.toFloat = x @>
        test <| <@ x |> Length.ofPercent |> Length.toFloat = x @>
        test <| <@ x |> int |> Length.ofPixels |> Length.toFloat |> int = int x @>
