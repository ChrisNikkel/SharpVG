namespace SharpVG.Tests

open System
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

module TestTiming =

    [<Fact>]
    let ``create timing`` () =
        test <|
        <@
            Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.toAttributes 
            |> List.map Attribute.toString |> String.concat " " = "begin=\"4\""
        @>

    [<Fact>]
    let ``create timing with duration`` () =
        test <|
        <@
            Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withDuration (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes 
            |> List.map Attribute.toString |> String.concat " " = "begin=\"4\" dur=\"0.012\""
        @>

    [<Fact>]
    let ``create timing with media duration`` () =
        test <|
        <@
            Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withMediaDuration |> Timing.toAttributes 
            |> List.map Attribute.toString |> String.concat " " = "begin=\"4\" dur=\"media\""
        @>

    [<Fact>]
    let ``create timing with end`` () =
        test <|
        <@
            Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withEnd (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes 
            |> List.map Attribute.toString |> String.concat " " = "begin=\"4\" end=\"0.012\""
        @>

    [<Fact>]
    let ``create timing with minimum`` () =
        test <|
        <@
            Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withMinimum (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes 
            |> List.map Attribute.toString |> String.concat " " = "begin=\"4\" min=\"0.012\""
        @>

    [<Fact>]
    let ``create timing with maximum`` () =
        test <|
        <@
            Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withMaximum (TimeSpan.FromMilliseconds(12.0)) |> Timing.toAttributes 
            |> List.map Attribute.toString |> String.concat " " = "begin=\"4\" max=\"0.012\""
        @>

    [<Fact>]
    let ``create timing with restart always`` () =
        test <|
        <@
            Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withResart Always |> Timing.toAttributes 
            |> List.map Attribute.toString |> String.concat " " = "begin=\"4\" restart=\"always\""
        @>
    [<Fact>]
    let ``create timing with restart when not active`` () =
        test <|
        <@
            Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withResart WhenNotActive |> Timing.toAttributes 
            |> List.map Attribute.toString |> String.concat " " = "begin=\"4\" restart=\"whennotactive\""
        @>
    [<Fact>]
    let ``create timing with never restart`` () =
        test <|
        <@
            Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withResart Never |> Timing.toAttributes 
            |> List.map Attribute.toString |> String.concat " " = "begin=\"4\" restart=\"never\""
        @>

    [<Fact>]
    let ``create timing with frozen final state`` () =
        test <|
        <@
            Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withFinalState Freeze |> Timing.toAttributes 
            |> List.map Attribute.toString |> String.concat " " = "begin=\"4\" fill=\"freeze\""
        @>

    [<Fact>]
    let ``create timing with final state removed`` () =
        test <|
        <@
            Timing.create (TimeSpan.FromSeconds(4.0)) |> Timing.withFinalState Remove |> Timing.toAttributes 
            |> List.map Attribute.toString |> String.concat " " = "begin=\"4\" fill=\"remove\""
        @>

