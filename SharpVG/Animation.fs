namespace SharpVG

open System

type CalculationMode =
    | Discrete = 1
    | Linear = 2
    | Paced = 3
    | Spline = 4

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

type AnimationType =
    | Set of SetChange
    | Animate of AnimateChange
    | Transform of From: Transform * To: Transform
    | Motion of Motion

type Animation =
    {
        AnimationType: AnimationType
        Timing: Timing
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Animation =

    let createTransform timing fromTransform toTransform =
        {
            AnimationType = Transform (From = fromTransform, To = toTransform)
            Timing = timing
        }

    let createSet timing attributeType attributeName attributeValue =
        {
            AnimationType = Set {AttributeName = attributeName; AttributeValue = attributeValue; AttributeType = attributeType }
            Timing = timing
        }

    let createAnimation timing attributeType attributeName attributeFromValue attributeToValue =
        {
            AnimationType = Animate {AttributeName = attributeName; AttributeFromValue = attributeFromValue; AttributeToValue = attributeToValue; AttributeType = attributeType }
            Timing = timing
        }

    let createMotion timing path calculationMode =
        {
            AnimationType = Motion {Path = path; CalculationMode = calculationMode}
            Timing = timing
        }

    let toTag animation =
        let calculationModeToAttribute c =
            match c with
                | Some(c) -> List.singleton (Attribute.createXML "calculationMode" (Enum.GetName(typeof<CalculationMode>, c).ToLower()))
                | None -> []
        let name, attributes =
            match animation.AnimationType with 
                | Set c -> "set", [Attribute.createXML "attributeName" c.AttributeName; Attribute.createXML "attributeType" (Enum.GetName(typeof<AttributeType>, c.AttributeType)); Attribute.createXML "to" c.AttributeValue]
                | Animate c -> "animate", [Attribute.createXML "attributeName" c.AttributeName; Attribute.createXML "attributeType" (Enum.GetName(typeof<AttributeType>, c.AttributeType)); Attribute.createXML "from" c.AttributeFromValue; Attribute.createXML "to" c.AttributeToValue]
                | Transform (f, t) -> "animateTransform", [Attribute.createXML "type" (Transform.getTypeName f); Attribute.createXML "from" (f |> Transform.toString); Attribute.createXML "to" (t |> Transform.toString)]
                | Motion m -> "animateMotion", [m.Path |> Path.toAttribute] @ (calculationModeToAttribute m.CalculationMode)
        Tag.create name
        |> Tag.addAttributes attributes
        |> Tag.addAttributes (animation.Timing |> Timing.toAttributes)

    let toString animation = animation |> toTag |> Tag.toString