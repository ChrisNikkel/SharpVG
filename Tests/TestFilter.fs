namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

module TestFilter =

    [<Fact>]
    let ``create blend filter`` () =
        let filter = Filter.create (FilterEffect.createBlend BlendMode.Lighten)
        let element = filter |> Element.createWithName "blendFilter"
        Assert.Equal("<filter id=\"blendFilter\"><feBlend mode=\"lighten\"/></filter>", element |> Element.toString)

    [<Fact>]
    let ``create gaussian blur filter`` () =
        let filter = Filter.create (FilterEffect.createGaussianBlur 5.0)
        let element = filter |> Element.createWithName "blureFilter"
        Assert.Equal("<filter id=\"blureFilter\"><feGaussianBlur stdDeviation=\"5\"/></filter>", element |> Element.toString)

    [<Fact>]
    let ``create color matrix saturate filter`` () =
        let fe = FilterEffect.createColorMatrix (Saturate 0.5)
        let result = fe |> FilterEffect.toString
        Assert.Contains("feColorMatrix", result)
        Assert.Contains("type=\"saturate\"", result)
        Assert.Contains("values=\"0.5\"", result)

    [<Fact>]
    let ``create color matrix matrix filter`` () =
        let identity = [1;0;0;0;0; 0;1;0;0;0; 0;0;1;0;0; 0;0;0;1;0]
        let result = FilterEffect.createColorMatrix (ColorMatrix.Matrix identity) |> FilterEffect.toString
        Assert.Contains("type=\"matrix\"", result)
        Assert.Contains("values=\"1 0 0 0 0 0 1 0 0 0 0 0 1 0 0 0 0 0 1 0\"", result)

    [<Fact>]
    let ``create color matrix hueRotate filter`` () =
        let result = FilterEffect.createColorMatrix (HueRotate 90.0) |> FilterEffect.toString
        Assert.Contains("type=\"hueRotate\"", result)
        Assert.Contains("values=\"90\"", result)

    [<Fact>]
    let ``create flood filter`` () =
        let fe = FilterEffect.createFlood (Color.ofName Colors.Red)
        let result = fe |> FilterEffect.toString
        Assert.Contains("feFlood", result)
        Assert.Contains("flood-color=\"red\"", result)

    [<Fact>]
    let ``create flood filter with opacity`` () =
        let fe = FilterEffect.createFloodWithOpacity (Color.ofName Colors.Blue) 0.5
        let result = fe |> FilterEffect.toString
        Assert.Contains("feFlood", result)
        Assert.Contains("flood-opacity=\"0.5\"", result)

    [<Fact>]
    let ``create offset filter`` () =
        let fe = FilterEffect.createOffset (Point.ofInts (5, 5))
        let result = fe |> FilterEffect.toString
        Assert.Contains("feOffset", result)

    [<Fact>]
    let ``create composite filter`` () =
        let fe = FilterEffect.createComposite Over
        let result = fe |> FilterEffect.toString
        Assert.Contains("feComposite", result)
        Assert.Contains("operator=\"over\"", result)

    [<Fact>]
    let ``create turbulence fractalNoise filter`` () =
        let fe = FilterEffect.createTurbulence FractalNoise 0.05 3
        let result = fe |> FilterEffect.toString
        Assert.Contains("feTurbulence", result)
        Assert.Contains("type=\"fractalNoise\"", result)
        Assert.Contains("baseFrequency=\"0.05\"", result)
        Assert.Contains("numOctaves=\"3\"", result)

    [<Fact>]
    let ``create turbulence turbulence type filter`` () =
        let fe = FilterEffect.createTurbulence TurbulenceNoise 0.1 2
        let result = fe |> FilterEffect.toString
        Assert.Contains("type=\"turbulence\"", result)

    [<Fact>]
    let ``create turbulence with seed`` () =
        let fe = FilterEffect.createTurbulenceWithSeed FractalNoise 0.05 3 42
        let result = fe |> FilterEffect.toString
        Assert.Contains("seed=\"42\"", result)

    [<Fact>]
    let ``create morphology erode filter`` () =
        let fe = FilterEffect.createMorphology Erode 1.0
        let result = fe |> FilterEffect.toString
        Assert.Contains("feMorphology", result)
        Assert.Contains("operator=\"erode\"", result)
        Assert.Contains("radius=\"1\"", result)

    [<Fact>]
    let ``create morphology dilate filter`` () =
        let fe = FilterEffect.createMorphology Dilate 2.0
        let result = fe |> FilterEffect.toString
        Assert.Contains("operator=\"dilate\"", result)

    [<Fact>]
    let ``create merge filter`` () =
        let fe = FilterEffect.createMerge [SourceGraphic; SourceAlpha]
        let result = fe |> FilterEffect.toString
        Assert.Contains("feMerge", result)
        Assert.Contains("feMergeNode", result)
        Assert.Contains("in=\"SourceGraphic\"", result)
        Assert.Contains("in=\"SourceAlpha\"", result)

    [<Fact>]
    let ``create displacement map filter`` () =
        let fe = FilterEffect.createDisplacementMap 20.0 RedChannel GreenChannel
        let result = fe |> FilterEffect.toString
        Assert.Contains("feDisplacementMap", result)
        Assert.Contains("scale=\"20\"", result)
        Assert.Contains("xChannelSelector=\"R\"", result)
        Assert.Contains("yChannelSelector=\"G\"", result)

    [<Fact>]
    let ``create specular lighting filter`` () =
        let fe = FilterEffect.createSpecularLighting 1.0 1.0 10.0 (DistantLight(45.0, 90.0))
        let result = fe |> FilterEffect.toString
        Assert.Contains("feSpecularLighting", result)
        Assert.Contains("surfaceScale=\"1\"", result)
        Assert.Contains("specularConstant=\"1\"", result)
        Assert.Contains("specularExponent=\"10\"", result)

    [<Fact>]
    let ``create component transfer filter`` () =
        let fe = FilterEffect.createComponentTransfer IdentityTransfer IdentityTransfer IdentityTransfer IdentityTransfer
        let result = fe |> FilterEffect.toString
        Assert.Contains("feComponentTransfer", result)
        Assert.Contains("feFuncR", result)
        Assert.Contains("feFuncG", result)
        Assert.Contains("feFuncB", result)
        Assert.Contains("feFuncA", result)

    [<Fact>]
    let ``create convolve matrix filter`` () =
        let fe = FilterEffect.createConvolveMatrix 3 [1.0;0.0;-1.0; 1.0;0.0;-1.0; 1.0;0.0;-1.0]
        let result = fe |> FilterEffect.toString
        Assert.Contains("feConvolveMatrix", result)
        Assert.Contains("order=\"3\"", result)
        Assert.Contains("kernelMatrix=", result)

    [<Fact>]
    let ``Filter withId adds id attribute`` () =
        let fe = FilterEffect.createGaussianBlur 3.0
        let result = Filter.create fe |> Filter.withId "myFilter" |> Filter.toString
        Assert.StartsWith("<filter id=\"myFilter\"", result)

    [<Fact>]
    let ``Filter withFilterUnits userSpaceOnUse`` () =
        let fe = FilterEffect.createGaussianBlur 3.0
        let filter = Filter.create fe
        let result = Filter.withFilterUnits filter (Some UserSpaceOnUse) |> Filter.toString
        Assert.Contains("filterUnits=\"userSpaceOnUse\"", result)

    [<Fact>]
    let ``Filter withLocation adds position attributes`` () =
        let fe = FilterEffect.createGaussianBlur 3.0
        let filter = Filter.create fe
        let position = Point.ofInts (-10, -10)
        let result = Filter.withLocation filter (Some position) |> Filter.toString
        Assert.Contains("x=\"-10\"", result)
        Assert.Contains("y=\"-10\"", result)

    [<Fact>]
    let ``Filter addFilterEffect appends a single effect`` () =
        let fe1 = FilterEffect.createGaussianBlur 3.0
        let fe2 = FilterEffect.createColorMatrix (Saturate 0.5)
        let result = Filter.create fe1 |> Filter.addFilterEffect fe2 |> Filter.toString
        Assert.Contains("feGaussianBlur", result)
        Assert.Contains("feColorMatrix", result)

    [<Fact>]
    let ``ColorMatrix luminanceToAlpha has correct type attribute`` () =
        let result = FilterEffect.createColorMatrix LuminanceToAlpha |> FilterEffect.toString
        Assert.Contains("type=\"luminanceToAlpha\"", result)

    [<Fact>]
    let ``BlendMode values are lowercase per SVG spec`` () =
        let modes = [Normal; Multiply; Screen; Overlay; Darken; Lighten; ColorDodge; ColorBurn; HardLight; SoftLight; Difference; Exclusion; Hue; Saturation; Color; Luminosity]
        for mode in modes do
            let modeString = mode.ToString()
            Assert.Equal(modeString, modeString.ToLowerInvariant())

    [<Fact>]
    let ``Composite operator values are lowercase per SVG spec`` () =
        let ops = [Composite.Over; Composite.In; Composite.Out; Composite.Atop; Composite.Xor; Composite.Lighter; Composite.Arithmetic]
        for op in ops do
            let operatorString = op.ToString()
            Assert.Equal(operatorString, operatorString.ToLowerInvariant())

    [<Fact>]
    let ``EdgeMode none value is correct per SVG spec`` () =
        Assert.Equal("none", NoEdge.ToString())
        Assert.Equal("duplicate", Duplicate.ToString())
        Assert.Equal("wrap", Wrap.ToString())

    [<Fact>]
    let ``Filter withFilterEffects multiple effects`` () =
        let fe1 = FilterEffect.createGaussianBlur 3.0
        let fe2 = FilterEffect.createColorMatrix (Saturate 0.5)
        let filter = Filter.create fe1
        let result = Filter.withFilterEffects filter [fe1; fe2] |> Filter.toString
        Assert.Contains("feGaussianBlur", result)
        Assert.Contains("feColorMatrix", result)

    [<Fact>]
    let ``create drop shadow basic`` () =
        let result = FilterEffect.createDropShadow 2.0 2.0 3.0 |> FilterEffect.toString
        Assert.Contains("feDropShadow", result)
        Assert.Contains("dx=\"2\"", result)
        Assert.Contains("dy=\"2\"", result)
        Assert.Contains("stdDeviation=\"3\"", result)

    [<Fact>]
    let ``create drop shadow with color`` () =
        let result = FilterEffect.createDropShadowWithColor 4.0 4.0 2.0 (Color.ofName Colors.Black) |> FilterEffect.toString
        Assert.Contains("flood-color=\"black\"", result)

    [<Fact>]
    let ``create drop shadow full`` () =
        let result = FilterEffect.createDropShadowFull 2.0 2.0 3.0 (Color.ofName Colors.Black) 0.5 |> FilterEffect.toString
        Assert.Contains("flood-color=\"black\"", result)
        Assert.Contains("flood-opacity=\"0.5\"", result)

    // FilterEffectSource variants
    [<Fact>]
    let ``FilterEffectSource BackgroundImage renders correctly`` () =
        let fe = FilterEffect.createMerge [BackgroundImage]
        let result = fe |> FilterEffect.toString
        Assert.Contains("in=\"BackgroundImage\"", result)

    [<Fact>]
    let ``FilterEffectSource BackgroundAlpha renders correctly`` () =
        let fe = FilterEffect.createMerge [BackgroundAlpha]
        let result = fe |> FilterEffect.toString
        Assert.Contains("in=\"BackgroundAlpha\"", result)

    [<Fact>]
    let ``FilterEffectSource FillPaint renders correctly`` () =
        let fe = FilterEffect.createMerge [FillPaint]
        let result = fe |> FilterEffect.toString
        Assert.Contains("in=\"FillPaint\"", result)

    [<Fact>]
    let ``FilterEffectSource StrokePaint renders correctly`` () =
        let fe = FilterEffect.createMerge [StrokePaint]
        let result = fe |> FilterEffect.toString
        Assert.Contains("in=\"StrokePaint\"", result)

    [<Fact>]
    let ``FilterEffectSource NamedFilterEffect uses effect name`` () =
        let blur = FilterEffect.withName (FilterEffect.createGaussianBlur 3.0) "blurResult"
        let offset = FilterEffect.createOffsetWithInput (Point.ofInts (2, 2)) (NamedFilterEffect blur)
        let result = offset |> FilterEffect.toString
        Assert.Contains("in=\"blurResult\"", result)

    // TransferFuncType variants
    [<Fact>]
    let ``TransferFuncType LinearTransfer renders slope and intercept`` () =
        let fe = FilterEffect.createComponentTransfer (LinearTransfer(0.5, 0.1)) IdentityTransfer IdentityTransfer IdentityTransfer
        let result = fe |> FilterEffect.toString
        Assert.Contains("type=\"linear\"", result)
        Assert.Contains("slope=\"0.5\"", result)
        Assert.Contains("intercept=\"0.1\"", result)

    [<Fact>]
    let ``TransferFuncType GammaTransfer renders amplitude exponent offset`` () =
        let fe = FilterEffect.createComponentTransfer (GammaTransfer(1.0, 2.0, 0.0)) IdentityTransfer IdentityTransfer IdentityTransfer
        let result = fe |> FilterEffect.toString
        Assert.Contains("type=\"gamma\"", result)
        Assert.Contains("amplitude=\"1\"", result)
        Assert.Contains("exponent=\"2\"", result)

    // LightSource variants
    [<Fact>]
    let ``LightSource PointLight renders x y z`` () =
        let fe = FilterEffect.createSpecularLighting 1.0 1.0 10.0 (PointLight(10.0, 20.0, 30.0))
        let result = fe |> FilterEffect.toString
        Assert.Contains("fePointLight", result)
        Assert.Contains("x=\"10\"", result)
        Assert.Contains("y=\"20\"", result)
        Assert.Contains("z=\"30\"", result)

    [<Fact>]
    let ``LightSource SpotLight renders source and target coords`` () =
        let fe = FilterEffect.createSpecularLighting 1.0 1.0 10.0 (SpotLight(0.0, 0.0, 50.0, 100.0, 100.0, 0.0, None))
        let result = fe |> FilterEffect.toString
        Assert.Contains("feSpotLight", result)

    // DiffuseLighting and NamedFilterEffect rendering
    [<Fact>]
    let ``DiffuseLighting renders feDiffuseLighting tag`` () =
        let fe = FilterEffect.createDiffuseLighting 1.0 0.5 1.0
        let result = fe |> FilterEffect.toString
        Assert.Contains("feDiffuseLighting", result)
        Assert.Contains("surfaceScale=\"1\"", result)
        Assert.Contains("diffuseConstant=\"0.5\"", result)

    [<Fact>]
    let ``NamedFilterEffect toString renders named effect`` () =
        let blur = FilterEffect.withName (FilterEffect.createGaussianBlur 2.0) "blurOut"
        let result = blur.ToString()
        Assert.Contains("feGaussianBlur", result)

    // Wiki: Filter page — gaussian blur example
    [<Fact>]
    let ``Filter wiki - gaussian blur applied via style`` () =
        let blur = FilterEffect.createGaussianBlur 4.0
        let filter = Filter.create blur |> Filter.withId "myBlur"
        let definitions = SvgDefinitions.create |> SvgDefinitions.addFilter filter
        let style = Style.createWithFill (Color.ofName Colors.Blue) |> Style.withFilter "myBlur"
        let position = Point.ofInts (20, 20)
        let area = Area.ofInts (200, 100)
        let rect = Rect.create position area |> Element.createWithStyle style
        let output =
            [ rect ]
            |> Svg.ofElementsWithDefinitions definitions
            |> Svg.toString
        Assert.Contains("<filter id=\"myBlur\">", output)
        Assert.Contains("feGaussianBlur", output)
        Assert.Contains("filter=\"url(#myBlur)\"", output)

    [<SvgProperty>]
    let ``gaussian blur filter with any positive stdDeviation always produces valid tag`` (stdDeviation: float) =
        let result = Filter.create (FilterEffect.createGaussianBlur stdDeviation) |> Filter.withId "f" |> Filter.toString
        checkTag "filter" result

    [<SvgProperty>]
    let ``offset filter with any positive coordinates always produces valid tag`` (dx: float, dy: float) =
        let result = Filter.create (FilterEffect.createOffset (Point.ofFloats (dx, dy))) |> Filter.withId "f" |> Filter.toString
        checkTag "filter" result

    [<SvgIdProperty>]
    let ``filter with any safe id always starts with that id attribute`` (id: string) =
        let result = Filter.create (FilterEffect.createGaussianBlur 1.0) |> Filter.withId id |> Filter.toString
        result.StartsWith(sprintf "<filter id=\"%s\"" id)

    [<SvgProperty>]
    let ``adding multiple effects to filter always produces balanced tags`` (stdDeviation: float, dx: float, dy: float) =
        let blur = FilterEffect.createGaussianBlur stdDeviation
        let offset = FilterEffect.createOffset (Point.ofFloats (dx, dy))
        let result = Filter.create blur |> Filter.addFilterEffect offset |> Filter.withId "f" |> Filter.toString
        checkTag "filter" result

    [<SvgProperty>]
    let ``turbulence filter with any baseFrequency always produces valid tag`` (freq: float) =
        let result = Filter.create (FilterEffect.createTurbulence FractalNoise freq 2) |> Filter.withId "t" |> Filter.toString
        checkTag "filter" result