namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

module TestEllipse =

    [<Fact>]
    let ``create ellipse`` () =
        test <| <@ Ellipse.create Point.origin Point.origin |> Ellipse.toString = "<ellipse cx=\"0\" cy=\"0\" rx=\"0\" ry=\"0\"/>" @>