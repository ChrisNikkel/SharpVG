namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

module TestLine =

    [<Fact>]
    let ``create line`` () =
        test <| <@ Line.create Point.origin Point.origin |> Line.toString = "<line x1=\"0\" y1=\"0\" x2=\"0\" y2=\"0\"/>" @>