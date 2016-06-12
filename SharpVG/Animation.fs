namespace SharpVG

open System

type CalculationMode =
    | Discrete
    | Linear
    | Paced
    | Spline

type Motion =
    {
        Path: Path
        CalculationMode: CalculationMode option
    }

type AnimationType =
    | Change // set
    | Adjust // animate
    | Transform
    | Motion of Motion
    | Color

type Animation =
    {
        AnimationType: AnimationType
        Timing: Timing
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Animation =
    let toTag animation =
        let name, attribute =
            match animation.AnimationType with 
                | Change -> "set", ""
                | Adjust -> "animate", ""
                | Transform -> "animateTransform", ""
                | Motion m -> "animateMotion", (m.Path |> Path.toAttributeString) + match m.CalculationMode with | Some(c) -> " " + Enum.GetName(typeof<CalculationMode>, c).ToLower() | None -> ""
                | Color -> "animateColor", ""
        Tag.create name 
        |> Tag.addAttribute attribute
        |> Tag.addAttribute (animation.Timing |> Timing.toString)

    let toString animation = animation |> toTag |> Tag.toString