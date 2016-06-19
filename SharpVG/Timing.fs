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
    | Freeze
    | Remove

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
    let create b =
        {
            Begin = b;
            Duration = None;
            End = None;
            Minimum = None;
            Maximum = None;
            Restart = None;
            Repetition = None;
            FinalState = None;
        }

    let withDuration duration timing =
        { timing with Duration = Some(duration) }

    let withEnd e timing =
        { timing with End = Some(e) }

    let withMinimum minimum timing =
        { timing with Minimum = Some(minimum) }

    let withMaximum maximum timing =
        { timing with Maximum = Some(maximum) }

    let withResart restart timing =
        { timing with Restart = Some(restart) }

    let withRepetition repetition timing =
        { timing with Repetition = Some(repetition) }

    let withFinalState finalState timing =
        { timing with FinalState = Some(finalState) }

    let toString timing =
        let timeSpanToString (timeSpan:TimeSpan) =
            string timeSpan.TotalSeconds

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
                | Freeze -> "freeze"
                | Remove -> "remove"

        [
            Some("begin=" + Tag.quote (timing.Begin |> timeSpanToString));
            timing.Duration |> Option.map (fun d -> "dur=" + Tag.quote (d |> durationToString));
            timing.End |> Option.map (fun e -> "end=" + Tag.quote(e |> timeSpanToString));
            timing.Minimum |> Option.map (fun m -> "min=" + Tag.quote (m |> timeSpanToString));
            timing.Maximum |> Option.map (fun m -> "max=" + Tag.quote (m |> timeSpanToString));
            timing.Restart |> Option.map (fun r -> "restart=" + Tag.quote (Enum.GetName(typeof<Restart>, r).ToLower()));
            timing.Repetition |> Option.map (fun r -> r |> repetitionToString);
            timing.FinalState |> Option.map (fun f -> "fill=" + Tag.quote (f |> finalStateToString));
        ] |> List.choose id |> String.concat " "
