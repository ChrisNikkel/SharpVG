namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

module TestCircle =

    [<Fact>]
    let ``create circle`` () =
        test <| <@ Circle.create Point.origin Length.empty |> Circle.toString = "<circle r=\"0\" cx=\"0\" cy=\"0\"/>" @>