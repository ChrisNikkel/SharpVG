namespace SharpVG.Tests

open SharpVG
open Xunit

module TestSvg =

    [<Fact>]
    let ``create empty SVG`` () =
        Assert.Equal("<!DOCTYPE html>\n<html>\n<head>\n<title>empty</title>\n</head>\n<body>\n<svg></svg>\n</body>\n</html>\n", ([] |> Svg.ofList |> Svg.toHtml "empty"))
