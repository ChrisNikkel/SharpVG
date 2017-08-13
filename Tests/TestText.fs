namespace SharpVG.Tests

open SharpVG
open Xunit

module TestText =

    [<Fact>]
    let ``create text`` () =
        Assert.Equal(Text.create Point.origin "Hello World!" |> Text.toString, "<text x=\"0\" y=\"0\">Hello World!</text>")