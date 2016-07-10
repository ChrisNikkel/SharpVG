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
                Attribute.createXML "repeatCount"
                    (
                        match repetitionCount with
                            | RepeatCount c -> string c
                            | RepeatCountValue.Indefinite -> "indefinite"
                    )

            let repetitionDurationToAttribute repetitionDuration =
                Attribute.createXML "repeatDuration"
                    (
                        match repetitionDuration with
                            | RepeatDuration c -> c |> timeSpanToString
                            | RepeatDurationValue.Indefinite -> "indefinite"
                    )

            match repetition with
                | None -> []
                | Some {RepeatCount = c; RepeatDuration = None} -> [repetitionCountToAttribute c]
                | Some {RepeatCount = c; RepeatDuration = Some(d)} -> [repetitionCountToAttribute c; repetitionDurationToAttribute d]

        let finalStateToString finalState =
            match finalState with
                | Freeze -> "freeze"
                | Remove -> "remove"

        let restartToString r = (Enum.GetName(typeof<Restart>, r).ToLower())

        [
            Some (Attribute.createXML "begin" (timing.Begin |> timeSpanToString))
            timing.Duration |> Option.map (durationToString >> Attribute.createXML "dur")
            timing.End |> Option.map (timeSpanToString >> Attribute.createXML "end")
            timing.Minimum |> Option.map (timeSpanToString >> Attribute.createXML "min")
            timing.Maximum |> Option.map (timeSpanToString >> Attribute.createXML "max")
            timing.Restart |> Option.map (restartToString >> Attribute.createXML "restart")
            timing.FinalState |> Option.map (finalStateToString >> Attribute.createXML "fill")
        ]
        |> List.choose id
        |> List.append (repetitionToAttributes timing.Repetition)
