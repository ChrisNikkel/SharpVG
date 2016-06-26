﻿namespace SharpVG

open System

type CalculationMode =
    | Discrete = 1
    | Linear = 2
    | Paced = 3
    | Spline = 4

type AttributeType =
    | CSS = 1
    | XML = 2

type SetChange =
    {
        AttributeName: string
        AttributeValue: string
        AttributeType: AttributeType
    }

type AnimateChange =
    {
        AttributeName: string
        AttributeFromValue: string
        AttributeToValue: string
        AttributeType: AttributeType
    }

type Motion =
    {
        Path: Path
        CalculationMode: CalculationMode option
    }

// TODO: Combine with Transform.fs if possible
type AnimateTransform =
    | Translate of X: Length * Y: Length option
    | Scale of X: Length * Y: Length option
    | Rotate of Angle: float *  X: Length option * Y: Length option
    | SkewX of Angle: float
    | SkewY of Angle: float

type AnimationType =
    | Set of SetChange
    | Animate of AnimateChange
    | Transform of AnimateTransform
    | Motion of Motion

type Animation =
    {
        AnimationType: AnimationType
        Timing: Timing
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Animation =

    let createSet timing attributeType attributeName attributeValue =
        {
            AnimationType = Set {AttributeName = attributeName; AttributeValue = attributeValue; AttributeType = attributeType };
            Timing = timing
        }

    // TODO: Use (-) with set [attribute] rather than requiring strings
    let createAnimation timing attributeType attributeName attributeFromValue attributeToValue =
        {
            AnimationType = Animate {AttributeName = attributeName; AttributeFromValue = attributeFromValue; AttributeToValue = attributeToValue; AttributeType = attributeType };
            Timing = timing
        }

    let createMotion timing path calculationMode =
        {
            AnimationType = Motion {Path = path; CalculationMode = calculationMode};
            Timing = timing
        }

    let toTag animation =
        let calculationModeToAttribute c =
            match c with
                | Some(c) -> set [Attribute.create "calculationMode" (Enum.GetName(typeof<CalculationMode>, c).ToLower())]
                | None -> set []
        let name, attributes =
            match animation.AnimationType with 
                | Set c -> "set", set [Attribute.create "attributeName" c.AttributeName; Attribute.create "attributeType" (Enum.GetName(typeof<AttributeType>, c.AttributeType)); Attribute.create "to" c.AttributeValue]
                | Animate c -> "animate", set [Attribute.create "attributeName" c.AttributeName; Attribute.create "attributeType" (Enum.GetName(typeof<AttributeType>, c.AttributeType)); Attribute.create "from" c.AttributeFromValue; Attribute.create "to" c.AttributeToValue]
                | Transform _ -> "animateTransform", set []
                | Motion m -> "animateMotion", set [m.Path |> Path.toAttribute] + (calculationModeToAttribute m.CalculationMode)
        Tag.create name
        |> Tag.addAttributes attributes
        |> Tag.addAttributes (animation.Timing |> Timing.toAttributes)

    let toString animation = animation |> toTag |> Tag.toString