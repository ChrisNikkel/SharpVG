namespace SharpVG.Tests
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

module TestLength =

    [<Fact>]
    let ``create empty user space`` () =
        Assert.Equal(Length.empty |> Length.toString, "0")

    [<Fact>]
    let ``create full percent`` () =
        Assert.Equal(Length.full |> Length.toString, "100%")

    [<Fact>]
    let ``create user space`` () =
        Assert.Equal(2.0 |> Length.ofUserSpace |> Length.toString, "2")
        Assert.Equal(2.1 |> Length.ofUserSpace |> Length.toString, "2.1")

    [<Fact>]
    let ``create pixels`` () =
        Assert.Equal(2 |> Length.ofPixels |> Length.toString, "2px")

    [<Fact>]
    let ``create em`` () =
        Assert.Equal(2.0 |> Length.ofEm |> Length.toString, "2em")
        Assert.Equal(2.1 |> Length.ofEm |> Length.toString, "2.1em")

    [<Fact>]
    let ``create percent`` () =
        Assert.Equal(2.0 |> Length.ofPercent |> Length.toString, "2%")
        Assert.Equal(2.1 |> Length.ofPercent |> Length.toString, "2.1%")

    [<SvgProperty>]
    let ``what goes in must come out`` (x) =
        Assert.Equal(x |> Length.ofUserSpace |> Length.toFloat, x)
        Assert.Equal(x |> Length.ofEm |> Length.toFloat, x)
        Assert.Equal(x |> Length.ofPercent |> Length.toFloat, x)
        Assert.Equal(x |> int |> Length.ofPixels |> Length.toFloat |> int, int x)
