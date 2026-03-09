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

type AnimateValuesChange =
    {
        AttributeName: string
        AttributeValues: string list
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
    | AnimateValues of AnimateValuesChange
    | Transform of From: Transform * To: Transform
    | TransformValues of transformType: string * values: Transform list
    | Motion of Motion

type Additive =
    | Replace
    | Sum

type Animation =
    {
        AnimationType: AnimationType
        Timing: Timing
        Additive: Additive option
        KeyTimes: list<double>
        KeySplines: list<string>
    }
with
    static member ToTag animation =
        let calculationModeToAttribute c =
            let createCalculationMode mode = List.singleton (Attribute.createXML "calculationMode" mode)
            match c with
                | Some CalculationMode.Discrete -> createCalculationMode "discrete"
                | Some CalculationMode.Linear -> createCalculationMode "linear"
                | Some CalculationMode.Paced -> createCalculationMode "paced"
                | Some CalculationMode.Spline -> createCalculationMode "spline"
                | None -> []

        let additiveToAttribute additive =
            let createAdditive mode = List.singleton (Attribute.createXML "additive" mode)
            match additive with
                | Some Additive.Replace -> createAdditive "replace"
                | Some Additive.Sum -> createAdditive "sum"
                | None -> []

        let keyTimesToAttribute keyTimes =
            if List.isEmpty keyTimes then [] else keyTimes |> List.map string |> (String.concat ";") |> (Attribute.createXML "keyTimes") |> List.singleton

        let keySplinestoAttribute keySplines =
            if List.isEmpty keySplines then [] else keySplines |> (String.concat ";") |> (Attribute.createXML "keySplines") |> List.singleton

        let name, attributes =
            match animation.AnimationType with
                | Set c -> "set", [Attribute.createXML "attributeName" c.AttributeName; Attribute.createXML "attributeType" (AttributeType.toString c.AttributeType); Attribute.createXML "to" c.AttributeValue]
                | Animate c -> "animate", [Attribute.createXML "attributeName" c.AttributeName; Attribute.createXML "attributeType" (AttributeType.toString c.AttributeType); Attribute.createXML "from" c.AttributeFromValue; Attribute.createXML "to" c.AttributeToValue]
                | AnimateValues c -> "animate", [Attribute.createXML "attributeName" c.AttributeName; Attribute.createXML "attributeType" (AttributeType.toString c.AttributeType); Attribute.createXML "values" (c.AttributeValues |> String.concat ";")]
                | Transform (f, t) -> "animateTransform", [Attribute.createXML "attributeName" "transform"; Attribute.createXML "attributeType" "XML"; Attribute.createXML "type" (Transform.getTypeName f); Attribute.createXML "from" (f |> Transform.parametersToString); Attribute.createXML "to" (t |> Transform.parametersToString)]
                | TransformValues (typeName, vs) -> "animateTransform", [Attribute.createXML "attributeName" "transform"; Attribute.createXML "attributeType" "XML"; Attribute.createXML "type" typeName; Attribute.createXML "values" (vs |> List.map Transform.parametersToString |> String.concat ";")]
                | Motion m -> "animateMotion", [m.Path |> Path.toAttribute] @ (calculationModeToAttribute m.CalculationMode)

        Tag.create name
        |> Tag.addAttributes (attributes @ (keyTimesToAttribute animation.KeyTimes) @ (keySplinestoAttribute animation.KeySplines) @ (additiveToAttribute animation.Additive))
        |> Tag.addAttributes (animation.Timing |> Timing.toAttributes)

    override this.ToString() =
            this |> Animation.ToTag |> Tag.toString

module Animation =

    let createTransform timing fromTransform toTransform =
        {
            AnimationType = Transform (From = fromTransform, To = toTransform)
            Timing = timing
            Additive = None
            KeyTimes = List.empty
            KeySplines = List.empty
        }

    let createTransformWithValues timing values =
        match values with
        | [] -> failwith "createTransformWithValues: values list must not be empty"
        | first :: _ ->
            {
                AnimationType = TransformValues (Transform.getTypeName first, values)
                Timing = timing
                Additive = None
                KeyTimes = List.empty
                KeySplines = List.empty
            }

    let createSet timing attributeType attributeName attributeValue =
        {
            AnimationType = Set {AttributeName = attributeName; AttributeValue = attributeValue; AttributeType = attributeType }
            Timing = timing
            Additive = None
            KeyTimes = List.empty
            KeySplines = List.empty
        }

    let createAnimation timing attributeType attributeName attributeFromValue attributeToValue =
        {
            AnimationType = Animate {AttributeName = attributeName; AttributeFromValue = attributeFromValue; AttributeToValue = attributeToValue; AttributeType = attributeType }
            Timing = timing
            Additive = None
            KeyTimes = List.empty
            KeySplines = List.empty
        }

    let createAnimationWithValues timing attributeType attributeName attributeValues =
        {
            AnimationType = AnimateValues {AttributeName = attributeName; AttributeValues = attributeValues; AttributeType = attributeType }
            Timing = timing
            Additive = None
            KeyTimes = List.empty
            KeySplines = List.empty
        }

    let createMotion timing path calculationMode =
        {
            AnimationType = Motion {Path = path; CalculationMode = calculationMode}
            Timing = timing
            Additive = None
            KeyTimes = List.empty
            KeySplines = List.empty
        }

    let withAdditive additive animation =
        { animation with Additive = Some(additive) }

    let withKeyTimes keyTimes animation =
        { animation with KeyTimes = keyTimes }

    let withKeySplines keySplines animation =
        { animation with KeySplines = keySplines }

    let addKeyTime keyTime animation =
        let keyTimes = animation.KeyTimes |> (List.append keyTime)
        withKeyTimes keyTimes animation

    let toTag =
        Animation.ToTag

    let toString (animation : Animation) =
        animation.ToString()

