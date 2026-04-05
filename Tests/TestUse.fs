namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

module TestUse =

    [<Fact>]
    let ``create symbol and use`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let body = [ Circle.create Point.origin Length.empty |> Element.create ]
        let symbolElement = Symbol.create viewBox |> Symbol.withBody body |> Element.createWithName "name"
        let useElement = Use.create symbolElement Point.origin |> Element.create
        let elements = [ symbolElement; useElement ]

        Assert.Equal("<symbol id=\"name\" viewBox=\"0,0 100%,100%\"><circle r=\"0\" cx=\"0\" cy=\"0\"/></symbol><use href=\"#name\" x=\"0\" y=\"0\"/>", elements |> List.map Element.toString |> String.concat "")

    [<Fact>]
    let ``create symbol and use with style`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let body = [ Circle.create Point.origin Length.empty |> Element.create ]
        let symbolElement = Symbol.create viewBox |> Symbol.withBody body |> Element.createWithName "name"
        let style = Style.empty |> Style.withStroke (Name Colors.White) |> Style.withFill (Name Colors.Black)
        let useElement = Use.create symbolElement Point.origin |> Element.createWithStyle style
        let elements = [ symbolElement; useElement ]
        Assert.Equal("<symbol id=\"name\" viewBox=\"0,0 100%,100%\"><circle r=\"0\" cx=\"0\" cy=\"0\"/></symbol><use stroke=\"white\" fill=\"black\" href=\"#name\" x=\"0\" y=\"0\"/>", elements |> List.map Element.toString |> String.concat "")

    [<Fact>]
    let ``tryCreate returns Some when element has name`` () =
        let element = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName "dot"
        let result = Use.tryCreate element Point.origin
        Assert.True(result.IsSome)
        Assert.Contains("href=\"#dot\"", result.Value |> Use.toString)

    [<Fact>]
    let ``tryCreate returns None when element has no name`` () =
        let element = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let result = Use.tryCreate element Point.origin
        Assert.True(result.IsNone)

    // Wiki: Use page — place element at multiple positions
    [<Fact>]
    let ``Use wiki - reference element placed at multiple positions`` () =
        let center = Point.ofInts (0, 0)
        let radius = Length.ofInt 20
        let circleElement = Circle.create center radius |> Element.createWithName "dot"
        let definitions = SvgDefinitions.create |> SvgDefinitions.addElement circleElement
        let position1 = Point.ofInts (50, 50)
        let position2 = Point.ofInts (150, 80)
        let use1 = Use.create circleElement position1 |> Element.create
        let use2 = Use.create circleElement position2 |> Element.create
        let output =
            [ use1; use2 ]
            |> Svg.ofElementsWithDefinitions definitions
            |> Svg.toString
        Assert.Contains("<defs>", output)
        Assert.Contains("id=\"dot\"", output)
        Assert.Contains("href=\"#dot\"", output)
        let useCount = output.Split("<use ").Length - 1
        Assert.Equal(2, useCount)

    [<SvgIdProperty>]
    let ``Use.create always references the element's id in href`` (name: string) =
        let element = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName name
        let result = Use.create element Point.origin |> Use.toString
        result.Contains(sprintf "href=\"#%s\"" name)

    [<SvgIdProperty>]
    let ``Use.toString always produces a bodyless tag`` (name: string) =
        let element = Rect.create Point.origin Area.full |> Element.createWithName name
        let result = Use.create element Point.origin |> Use.toString
        checkBodylessTag "use" result

    [<Property>]
    let ``Use placed at any position always contains those coordinates`` (x: PositiveInt, y: PositiveInt) =
        let element = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName "dot"
        let position = Point.ofInts (x.Get, y.Get)
        let result = Use.create element position |> Use.toString
        result.Contains(sprintf "x=\"%d\"" x.Get) && result.Contains(sprintf "y=\"%d\"" y.Get)
