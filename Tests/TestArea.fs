namespace SharpVG.Tests

open SharpVG
open Xunit

module TestArea =

    [<Fact>]
    let ``create area`` () =
        Assert.Equal("0,0", Area.create Length.empty Length.empty |> Area.toString)

    [<Fact>]
    let ``create area of floats`` () =
        Assert.Equal("0,0", Area.ofFloats (0.0,0.0) |> Area.toString)

    [<Fact>]
    let ``create area of ints`` () =
        Assert.Equal("0,0", Area.ofInts (0,0) |> Area.toString)

    [<Fact>]
    let ``create area from points`` () =
        Assert.Equal("0,0", Area.fromPoints Point.origin Point.origin |> Area.toString)

    [<Fact>]
    let ``transform point to attribute`` () =
        Assert.Equal( "width=\"0\" height=\"0\"", Area.create Length.empty Length.empty |> Area.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create full area`` () =
        Assert.Equal("100%,100%", Area.full |> Area.toString)

