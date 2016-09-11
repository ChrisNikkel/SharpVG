namespace SharpVG

open System

type CalculationMode =
    | Discrete
    | Linear
    | Paced
    | Spline

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
    | Transform of From: Transform * To: Transform  // This likely needs to be of type Attribute rather than Transform
    | Motion of Motion

type Additive =
    | Replace
    | Sum

type Animation =
    {
        AnimationType: AnimationType
        Timing: Timing
        Target: string option
        Additive: Additive option
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Animation =

    let createTransform timing fromTransform toTransform =
        {
            AnimationType = Transform (From = fromTransform, To = toTransform)
            Timing = timing
            Target = None
            Additive = None
        }

    let createSet timing attributeType attributeName attributeValue =
        {
            AnimationType = Set {AttributeName = attributeName; AttributeValue = attributeValue; AttributeType = attributeType }
            Timing = timing
            Target = None
            Additive = None
        }

    let createAnimation timing attributeType attributeName attributeFromValue attributeToValue =
        {
            AnimationType = Animate {AttributeName = attributeName; AttributeFromValue = attributeFromValue; AttributeToValue = attributeToValue; AttributeType = attributeType }
            Timing = timing
            Target = None
            Additive = None
        }

    let createMotion timing path calculationMode =
        {
            AnimationType = Motion {Path = path; CalculationMode = calculationMode}
            Timing = timing
            Target = None
            Additive = None
        }

    let withAdditive additive animation =
        {animation with Additive = Some(additive) }

    let withTarget target animation =
        {animation with Target = Some(target) }

    let toTag animation =
        let calculationModeToAttribute c =
            match c with
                | Some CalculationMode.Discrete -> List.singleton (Attribute.createXML "calculationMode" "discrete")
                | Some CalculationMode.Linear -> List.singleton (Attribute.createXML "calculationMode" "linear")
                | Some CalculationMode.Paced -> List.singleton (Attribute.createXML "calculationMode" "paced")
                | Some CalculationMode.Spline -> List.singleton (Attribute.createXML "calculationMode" "spline")
                | None -> []

        let additiveToAttribute a =
            match a with
                | Some Additive.Replace -> List.singleton (Attribute.createXML "additive" "replace")
                | Some Additive.Sum -> List.singleton (Attribute.createXML "additive" "sum")
                | None -> []

        let targetToAttribute t =
            match t with
                | Some(t) -> List.singleton (Attribute.createXML "xlink:href" t)
                | None -> []

        let name, attributes =
            match animation.AnimationType with 
                | Set c -> "set", [Attribute.createXML "attributeName" c.AttributeName; Attribute.createXML "attributeType" (Enum.GetName(typeof<AttributeType>, c.AttributeType)); Attribute.createXML "to" c.AttributeValue]
                | Animate c -> "animate", [Attribute.createXML "attributeName" c.AttributeName; Attribute.createXML "attributeType" (Enum.GetName(typeof<AttributeType>, c.AttributeType)); Attribute.createXML "from" c.AttributeFromValue; Attribute.createXML "to" c.AttributeToValue]
                | Transform (f, t) -> "animateTransform", [Attribute.createXML "attributeName" "transform"; Attribute.createXML "attributeType" "XML"; Attribute.createXML "type" (Transform.getTypeName f); Attribute.createXML "from" (f |> Transform.toString); Attribute.createXML "to" (t |> Transform.toString)]
                | Motion m -> "animateMotion", [m.Path |> Path.toAttribute] @ (calculationModeToAttribute m.CalculationMode)
        Tag.create name
        |> Tag.addAttributes (attributes @ (targetToAttribute animation.Target) @ (additiveToAttribute animation.Additive))
        |> Tag.addAttributes (animation.Timing |> Timing.toAttributes)

    let toString animation = animation |> toTag |> Tag.toString

