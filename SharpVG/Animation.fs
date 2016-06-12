namespace SharpVG

open System

type CalculationMode =
    | Discrete
    | Linear
    | Paced
    | Spline

type Change =
    {
        AttributeName: string
        AttributeValue: string
    }

type Motion =
    {
        Path: Path
        CalculationMode: CalculationMode option
    }

type AnimationType =
    | Change of Change
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
                | Change c -> "set", "attributeName=" + Tag.quote(c.AttributeName) + " attributeType=\"XML\" to=" + Tag.quote(c.AttributeValue)
                | Adjust -> "animate", ""
                | Transform -> "animateTransform", ""
                | Motion m -> "animateMotion", (m.Path |> Path.toAttributeString) + match m.CalculationMode with | Some(c) -> " " + Enum.GetName(typeof<CalculationMode>, c).ToLower() | None -> ""
                | Color -> "animateColor", ""
        Tag.create name 
        |> Tag.addAttribute attribute
        |> Tag.addAttribute (animation.Timing |> Timing.toString)

    let toString animation = animation |> toTag |> Tag.toString