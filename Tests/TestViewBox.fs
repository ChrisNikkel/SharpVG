namespace SharpVG.Tests

open SharpVG
open Xunit

module TestViewBox =

    [<Fact>]
    let ``create and toAttributes`` () =
        let vb = ViewBox.create Point.origin Area.full
        let result = vb |> ViewBox.toAttributes |> List.map Attribute.toString |> String.concat " "
        Assert.Equal("viewBox=\"0,0 100%,100%\"", result)

    [<Fact>]
    let ``create with specific values`` () =
        let vb = ViewBox.create (Point.ofInts (10, 20)) (Area.ofInts (200, 100))
        let result = vb |> ViewBox.toAttributes |> List.map Attribute.toString |> String.concat " "
        Assert.Equal("viewBox=\"10,20 200,100\"", result)

    [<Fact>]
    let ``toAttributes returns single attribute`` () =
        let vb = ViewBox.create Point.origin (Area.ofInts (800, 600))
        Assert.Equal(1, vb |> ViewBox.toAttributes |> List.length)
