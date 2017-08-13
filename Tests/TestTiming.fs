namespace SharpVG.Tests

open System
open SharpVG
open Xunit

module TestTiming =

    [<Fact>]
    let ``create timing`` () =
        Assert.Equal(Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ", "begin=\"4s\"")

    [<Fact>]
    let ``create timing with duration`` () =
        Assert.Equal(Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withDuration (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ", "begin=\"4s\" dur=\"0.012s\"")

    [<Fact>]
    let ``create timing with media duration`` () =
        Assert.Equal(Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withMediaDuration |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ", "begin=\"4s\" dur=\"media\"")

    [<Fact>]
    let ``create timing with end`` () =
        Assert.Equal(Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withEnd (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ", "begin=\"4s\" end=\"0.012s\"")

    [<Fact>]
    let ``create timing with minimum`` () =
        Assert.Equal(Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withMinimum (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ", "begin=\"4s\" min=\"0.012s\"")

    [<Fact>]
    let ``create timing with maximum`` () =
        Assert.Equal(Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withMaximum (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ", "begin=\"4s\" max=\"0.012s\"")

    [<Fact>]
    let ``create timing with restart always`` () =
        Assert.Equal(Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withResart Always |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ", "begin=\"4s\" restart=\"always\"")

    [<Fact>]
    let ``create timing with restart when not active`` () =
        Assert.Equal(Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withResart WhenNotActive |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ", "begin=\"4s\" restart=\"whennotactive\"")

    [<Fact>]
    let ``create timing with never restart`` () =
        Assert.Equal(Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withResart Never |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ", "begin=\"4s\" restart=\"never\"")

    [<Fact>]
    let ``create timing with frozen final state`` () =
        Assert.Equal(Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withFinalState Freeze |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ", "begin=\"4s\" fill=\"freeze\"")

    [<Fact>]
    let ``create timing with final state removed`` () =
        Assert.Equal(Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withFinalState Remove |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ", "begin=\"4s\" fill=\"remove\"")
