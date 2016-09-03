namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

module TestImage =

    [<Fact>]
    let ``create image`` () =
        test <| <@ Image.create Point.origin Area.full "test.jpg" |> Image.toString = "<image x=\"0\" y=\"0\" height=\"100%\" width=\"100%\"/>" @>