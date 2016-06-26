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
            Begin = b
            Duration = None
            End = None
            Minimum = None
            Maximum = None
            Restart = None
            Repetition = None
            FinalState = None
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

    let toAttributes timing =
        let timeSpanToString (timeSpan:TimeSpan) =
            string timeSpan.TotalSeconds

        let durationToString duration =
            match duration with
                | Duration(d) -> d |> timeSpanToString
                | Media -> "media"

        let repetitionToAttributes repetition =
            let repetitionCountToAttribute repetitionCount =
                Attribute.create "repeatCount"
                    (
                        match repetitionCount with
                            | RepeatCount c -> string c
                            | RepeatCountValue.Indefinite -> "indefinite"
                    )

            let repetitionDurationToAttribute repetitionDuration =
                Attribute.create "repeatDuration"
                    (
                        match repetitionDuration with
                            | RepeatDuration c -> c |> timeSpanToString
                            | RepeatDurationValue.Indefinite -> "indefinite"
                    )

            match repetition with
                | None -> set []
                | Some {RepeatCount = c; RepeatDuration = None} -> set [repetitionCountToAttribute c]
                | Some {RepeatCount = c; RepeatDuration = Some(d)} -> set [repetitionCountToAttribute c; repetitionDurationToAttribute d]

        let finalStateToString finalState =
            match finalState with
                | Freeze -> "freeze"
                | Remove -> "remove"

        let restartToString r = (Enum.GetName(typeof<Restart>, r).ToLower())

        ([
            Some (Attribute.create "begin" (timing.Begin |> timeSpanToString))
            timing.Duration |> Option.map (durationToString >> Attribute.create "dur")
            timing.End |> Option.map (timeSpanToString >> Attribute.create "end")
            timing.Minimum |> Option.map (timeSpanToString >> Attribute.create "min")
            timing.Maximum |> Option.map (timeSpanToString >> Attribute.create "max")
            timing.Restart |> Option.map (restartToString >> Attribute.create "restart")
            timing.FinalState |> Option.map (finalStateToString >> Attribute.create "fill")
        ] |> List.choose id |> Set.ofList) + (repetitionToAttributes timing.Repetition)
