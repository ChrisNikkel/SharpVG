namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

module TestSvgDefinitions =

    [<Fact>]
    let ``create Svg with definitions containing symbol and use in body`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let body = [ Circle.create Point.origin Length.empty |> Element.create ]
        let symbolElement = Symbol.create viewBox |> Symbol.withBody body |> Element.createWithName "icon"
        let useElement = Use.create symbolElement Point.origin |> Element.create

        let definitions = SvgDefinitions.create |> SvgDefinitions.addElement symbolElement
        let svg = [ useElement ] |> Svg.ofElementsWithDefinitions definitions

        let output = svg |> Svg.toString
        Assert.Contains("<defs>", output)
        Assert.Contains("</defs>", output)
        Assert.Contains("<symbol id=\"icon\"", output)
        Assert.Contains("<use href=\"#icon\" x=\"0\" y=\"0\"", output)
        // Symbol should appear only inside defs (before the use)
        let defsEnd = output.IndexOf("</defs>")
        let useStart = output.IndexOf("<use ")
        Assert.True(defsEnd < useStart, "defs block should appear before use element")

    [<Fact>]
    let ``SvgDefinitions toString renders defs tag with content`` () =
        let circleElement = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let definitions = SvgDefinitions.create |> SvgDefinitions.addElement circleElement
        let output = definitions |> SvgDefinitions.toString
        Assert.Equal("<defs><circle r=\"5\" cx=\"0\" cy=\"0\"/></defs>", output)

    [<Fact>]
    let ``SvgDefinitions addGroup includes group in defs`` () =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let group = Group.ofList [circle]
        let output = SvgDefinitions.create |> SvgDefinitions.addGroup group |> SvgDefinitions.toString
        Assert.Contains("<defs>", output)
        Assert.Contains("<g>", output)
        Assert.Contains("<circle", output)

    [<Fact>]
    let ``SvgDefinitions addElements adds multiple elements`` () =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let rect = Rect.create Point.origin Area.full |> Element.create
        let output = SvgDefinitions.create |> SvgDefinitions.addElements [circle; rect] |> SvgDefinitions.toString
        Assert.Contains("<circle", output)
        Assert.Contains("<rect", output)

    [<Fact>]
    let ``SvgDefinitions toStyles collects styles from elements`` () =
        let fillColor = Color.ofName Colors.Red
        let style = Style.createWithFill fillColor
        let styledElement = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithStyle style
        let styles = SvgDefinitions.create |> SvgDefinitions.addElement styledElement |> SvgDefinitions.toStyles |> Seq.toList
        Assert.Equal(1, styles.Length)

    // Wiki: SvgDefinitions page — gradient and clip path combined
    [<Fact>]
    let ``SvgDefinitions wiki - multiple reusable items`` () =
        let stops =
            [ GradientStop.create 0.0 (Color.ofName Colors.Blue)
              GradientStop.create 1.0 (Color.ofName Colors.White) ]
        let gradient =
            LinearGradient.create "grad" (Point.ofFloats (0.0, 0.0)) (Point.ofFloats (1.0, 0.0)) stops
            |> Gradient.ofLinear
        let clipCircle =
            Circle.create (Point.ofInts (100, 100)) (Length.ofInt 80)
            |> Element.create
        let clipPath = ClipPath.ofElement "clip" clipCircle
        let definitions =
            SvgDefinitions.create
            |> SvgDefinitions.addGradient gradient
            |> SvgDefinitions.addClipPath clipPath
        let output = definitions |> SvgDefinitions.toString
        Assert.Contains("linearGradient", output)
        Assert.Contains("id=\"grad\"", output)
        Assert.Contains("clipPath", output)
        Assert.Contains("id=\"clip\"", output)

    [<Property>]
    let ``SvgDefinitions with any number of named elements always has balanced tags`` (counts: PositiveInt list) =
        let safeIds = counts |> List.truncate 10 |> List.mapi (fun i _ -> sprintf "el%d" i)
        let defs =
            safeIds
            |> List.fold (fun acc name ->
                let el = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName name
                SvgDefinitions.addElement el acc
            ) SvgDefinitions.create
        let result = defs |> SvgDefinitions.toString
        isMatched '<' '>' result

    [<Property>]
    let ``SvgDefinitions always wraps content in defs tags`` (n: PositiveInt) =
        let name = sprintf "el%d" n.Get
        let el = Rect.create Point.origin Area.full |> Element.createWithName name
        let result = SvgDefinitions.create |> SvgDefinitions.addElement el |> SvgDefinitions.toString
        result.StartsWith("<defs>") && result.EndsWith("</defs>")

    [<SvgProperty>]
    let ``SvgDefinitions with gradient always has balanced angle brackets`` (r, g, b, r2, g2, b2) =
        let stops =
            [ GradientStop.create 0.0 (Color.ofValues (r, g, b))
              GradientStop.create 1.0 (Color.ofValues (r2, g2, b2)) ]
        let gradient =
            LinearGradient.create "g" (Point.ofFloats (0.0, 0.0)) (Point.ofFloats (1.0, 0.0)) stops
            |> Gradient.ofLinear
        let result = SvgDefinitions.create |> SvgDefinitions.addGradient gradient |> SvgDefinitions.toString
        isMatched '<' '>' result
