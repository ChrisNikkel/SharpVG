namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

module TestStyle =

    [<Fact>]
    let ``create style with fill`` () =
        test <| <@ Style.empty |> Style.withFill (Name Colors.White) |> Style.toString = "fill:white" @>

    [<Fact>]
    let ``create style with stroke`` () =
        test <| <@ Style.empty |> Style.withStroke (Name Colors.White) |> Style.toString = "stroke:white" @>

    [<Fact>]
    let ``create style with stroke width`` () =
        test <| <@ Style.empty |> Style.withStroke (Name Colors.White) |> Style.withStrokeWidth Length.empty |> Style.toString = "stroke:white;stroke-width:0" @>

    [<Fact>]
    let ``create style with opacity`` () =
        test <| <@ Style.empty |> Style.withOpacity 0.5 |> Style.toString = "opacity:0.5" @>

