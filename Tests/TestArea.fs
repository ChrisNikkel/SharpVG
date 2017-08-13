namespace SharpVG.Tests

open SharpVG
open Xunit

module TestArea =

    [<Fact>]
    let ``create area`` () =
        Assert.Equal(Area.create Length.empty Length.empty |> Area.toString, "0,0")

    [<Fact>]
    let ``create area of floats`` () =
        Assert.Equal(Area.ofFloats (0.0,0.0) |> Area.toString, "0,0")

    [<Fact>]
    let ``create area of ints`` () =
        Assert.Equal(Area.ofInts (0,0) |> Area.toString, "0,0")

    [<Fact>]
    let ``create area from points`` () =
        Assert.Equal(Area.fromPoints Point.origin Point.origin |> Area.toString, "0,0")

    [<Fact>]
    let ``transform point to attribute`` () =
        Assert.Equal(Area.create Length.empty Length.empty |> Area.toAttributes |> List.map Attribute.toString |> String.concat " ", "height=\"0\" width=\"0\"")

    [<Fact>]
    let ``create full area`` () =
        Assert.Equal(Area.full |> Area.toString, "100%,100%")

