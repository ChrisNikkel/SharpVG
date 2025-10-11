namespace SharpVG.Tests
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

module TestLength =

    [<Fact>]
    let ``create empty user space`` () =
        Assert.Equal("0", Length.empty |> Length.toString)

    [<Fact>]
    let ``create full percent`` () =
        Assert.Equal("100%", Length.full |> Length.toString)

    [<Fact>]
    let ``create user space`` () =
        Assert.Equal("2", 2.0 |> Length.ofUserSpace |> Length.toString)
        Assert.Equal("2.1", 2.1 |> Length.ofUserSpace |> Length.toString)

    [<Fact>]
    let ``create pixels`` () =
        Assert.Equal("2px", 2 |> Length.ofPixels |> Length.toString)

    [<Fact>]
    let ``create em`` () =
        Assert.Equal("2em", 2.0 |> Length.ofEm |> Length.toString)
        Assert.Equal("2.1em", 2.1 |> Length.ofEm |> Length.toString)

    [<Fact>]
    let ``create percent`` () =
        Assert.Equal("2%", 2.0 |> Length.ofPercent |> Length.toString)
        Assert.Equal("2.1%", 2.1 |> Length.ofPercent |> Length.toString)
    
    [<Fact>]
    let ``create cm`` () =
        Assert.Equal("2cm", 2.0 |> Length.ofCm |> Length.toString)
        Assert.Equal("2.1cm", 2.1 |> Length.ofCm |> Length.toString)
    
    [<Fact>]
    let ``create mm`` () =
        Assert.Equal("2mm", 2.0 |> Length.ofMm |> Length.toString)
        Assert.Equal("2.1mm", 2.1 |> Length.ofMm |> Length.toString)
    
    [<Fact>]
    let ``create in`` () =
        Assert.Equal("2in", 2.0 |> Length.ofIn |> Length.toString)
        Assert.Equal("2.1in", 2.1 |> Length.ofIn |> Length.toString)
    
    [<Fact>]
    let ``create pt`` () =
        Assert.Equal("2pt", 2.0 |> Length.ofPt |> Length.toString)
        Assert.Equal("2.1pt", 2.1 |> Length.ofPt |> Length.toString)

    [<SvgProperty>]
    let ``what goes in must come out (float)`` (x) =
        Assert.Equal(x, x |> Length.ofUserSpace |> Length.toFloat)
        Assert.Equal(x, x |> Length.ofEm |> Length.toFloat)
        Assert.Equal(x, x |> Length.ofPercent |> Length.toFloat)
        Assert.Equal(int x, x |> int |> Length.ofPixels |> Length.toFloat |> int)

    [<SvgProperty>]
    let ``what goes in must come out (int)`` (x) =
        Assert.Equal(int x, x |> Length.ofUserSpace |> Length.toInt)
        Assert.Equal(int x, x |> Length.ofEm |> Length.toInt)
        Assert.Equal(int x, x |> Length.ofPercent |> Length.toInt)
        Assert.Equal(int x, x |> int |> Length.ofPixels |> Length.toInt)
