namespace SharpVG

open System

type Restart =
    | Always
    | WhenNotActive
    | Never

type DurationValue =
    | Duration of TimeSpan
    | Media

type RepeatCountValue =
    | RepeatCount of float
    | Indefinite

type RepeatDurationValue =
    | RepeatDuration of TimeSpan
    | Indefinite

type Repetition =
    {
        RepeatCount: RepeatCountValue
        RepeatDuration: RepeatDurationValue option
    }

type FinalState = // fill
    | Frozen // freeze
    | Hidden // remove

type Timing =
    {
        Begin: TimeSpan
        Duration: DurationValue option
        End: TimeSpan option
        Minimum: TimeSpan option
        Maximum: TimeSpan option
        Restart: Restart option
        Repetition: Repetition option
        FinalState: FinalState option
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Timing =
    let toString timing =
        let timeSpanToString (timeSpan:TimeSpan) =
            timeSpan.ToString("hh:mm:ss")

        let durationToString duration =
            match duration with
                | Duration(d) -> d |> timeSpanToString
                | Media -> "media"

        let repetitionToString repetition =
            "repeatCount=" + (
                Tag.quote(
                    match repetition.RepeatCount with
                        | RepeatCount c -> string c
                        | RepeatCountValue.Indefinite -> "indefinite"
            )) +
            match repetition.RepeatDuration with
                | Some d ->
                    "repeatDuration=" + (
                        Tag.quote(
                            match d with
                                | RepeatDuration c ->  c |> timeSpanToString
                                | RepeatDurationValue.Indefinite -> "indefinite"
                        )
                    )
                | None -> ""

        let finalStateToString finalState =
            match finalState with
                | Frozen -> "freeze"
                | Hidden -> "remove"

        [
            Some("begin=" + (timing.Begin |> timeSpanToString));
            timing.Duration |> Option.map (fun d -> "dur=" + Tag.quote(d |> durationToString));
            timing.End |> Option.map (fun e -> "end=" + Tag.quote(e |> timeSpanToString));
            timing.Minimum |> Option.map (fun m -> "min=" + Tag.quote(m |> timeSpanToString));
            timing.Maximum |> Option.map (fun m -> "max=" + Tag.quote(m |> timeSpanToString));
            timing.Restart |> Option.map (fun r -> Enum.GetName(typeof<Restart>, r).ToLower() |> Tag.quote);
            timing.Repetition |> Option.map (fun r -> r |> repetitionToString);
            timing.FinalState |> Option.map (fun f -> "fill=" + Tag.quote(f |> finalStateToString));
        ] |> List.choose id |> String.concat " "
