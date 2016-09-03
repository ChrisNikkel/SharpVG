namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

module TestText =

    [<Fact>]
    let ``create text`` () =
        test <| <@ Text.create Point.origin "Hello World!" |> Text.toString = "<text x=\"0\" y=\"0\">Hello World!</text>" @>