namespace SharpVG.Tests

open System
open SharpVG
open Xunit

module TestTiming =

    [<Fact>]
    let ``create timing`` () =
        Assert.Equal("begin=\"4s\"", Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create timing with duration`` () =
        Assert.Equal("begin=\"4s\" dur=\"0.012s\"", Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withDuration (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create timing with media duration`` () =
        Assert.Equal("begin=\"4s\" dur=\"media\"", Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withMediaDuration |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create timing with end`` () =
        Assert.Equal("begin=\"4s\" end=\"0.012s\"", Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withEnd (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create timing with minimum`` () =
        Assert.Equal("begin=\"4s\" min=\"0.012s\"", Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withMinimum (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create timing with maximum`` () =
        Assert.Equal("begin=\"4s\" max=\"0.012s\"", Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withMaximum (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create timing with restart always`` () =
        Assert.Equal("begin=\"4s\" restart=\"always\"", Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withResart Always |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create timing with restart when not active`` () =
        Assert.Equal("begin=\"4s\" restart=\"whenNotActive\"", Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withResart WhenNotActive |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create timing with never restart`` () =
        Assert.Equal("begin=\"4s\" restart=\"never\"", Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withResart Never |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create timing with frozen final state`` () =
        Assert.Equal("begin=\"4s\" fill=\"freeze\"", Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withFinalState Freeze |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create timing with final state removed`` () =
        Assert.Equal("begin=\"4s\" fill=\"remove\"", Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withFinalState Remove |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " ")

    [<Fact>]
    let ``create timing with repeat count indefinite`` () =
        let repetition = { RepeatCount = RepeatCountValue.Indefinite; RepeatDuration = None }
        let attrs = Timing.create (TimeSpan.FromSeconds 0.0) |> Timing.withDuration (TimeSpan.FromSeconds 3.0) |> Timing.withRepetition repetition |> Timing.toAttributes
        let attributeString = attrs |> List.map Attribute.toString |> String.concat " "
        Assert.Contains("begin=\"0s\"", attributeString)
        Assert.Contains("dur=\"3s\"", attributeString)
        Assert.Contains("repeatCount=\"indefinite\"", attributeString)

    // Wiki: Animation (Timing section) examples
    [<Fact>]
    let ``Timing wiki - duration and repeat example`` () =
        let timing = Timing.create (TimeSpan.FromSeconds 0.0) |> Timing.withDuration (TimeSpan.FromSeconds 3.0) |> Timing.withRepetition { RepeatCount = RepeatCountValue.Indefinite; RepeatDuration = None }
        let attributeString = timing |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " "
        Assert.Contains("begin=\"0s\"", attributeString)
        Assert.Contains("dur=\"3s\"", attributeString)
        Assert.Contains("repeatCount=\"indefinite\"", attributeString)

    [<Fact>]
    let ``Timing wiki - restart and fill example`` () =
        let timing = Timing.create (TimeSpan.FromSeconds 2.0) |> Timing.withDuration (TimeSpan.FromSeconds 1.0) |> Timing.withResart Always |> Timing.withFinalState Freeze
        let attributeString = timing |> Timing.toAttributes |> List.map Attribute.toString |> String.concat " "
        Assert.Contains("begin=\"2s\"", attributeString)
        Assert.Contains("dur=\"1s\"", attributeString)
        Assert.Contains("restart=\"always\"", attributeString)
        Assert.Contains("fill=\"freeze\"", attributeString)
