namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

module TestRect =

    [<Fact>]
    let ``create rect`` () =
        test <| <@ Rect.create Point.origin Area.full |> Rect.toString = "<rect x=\"0\" y=\"0\" height=\"100%\" width=\"100%\"/>" @>