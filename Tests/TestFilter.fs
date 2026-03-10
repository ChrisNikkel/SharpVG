namespace SharpVG.Tests

open SharpVG
open Xunit

module TestFilter =

    [<Fact>]
    let ``create blend filter`` () =
        let filter = Filter.create (FilterEffect.createBlend BlendMode.Lighten)
        let element = filter |> Element.createWithName "blendFilter"
        Assert.Equal("<filter id=\"blendFilter\"><feBlend mode=\"Lighten\"/></filter>", element |> Element.toString)

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
        Assert.Contains("operator=\"Over\"", result)

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
        let f = Filter.create fe
        let result = Filter.withFilterUnits f (Some UserSpaceOnUse) |> Filter.toString
        Assert.Contains("filterUnits=\"userSpaceOnUse\"", result)

    [<Fact>]
    let ``Filter withLocation adds position attributes`` () =
        let fe = FilterEffect.createGaussianBlur 3.0
        let f = Filter.create fe
        let result = Filter.withLocation f (Some (Point.ofInts (-10, -10))) |> Filter.toString
        Assert.Contains("x=\"-10\"", result)
        Assert.Contains("y=\"-10\"", result)

    [<Fact>]
    let ``Filter withFilterEffects multiple effects`` () =
        let fe1 = FilterEffect.createGaussianBlur 3.0
        let fe2 = FilterEffect.createColorMatrix (Saturate 0.5)
        let f = Filter.create fe1
        let result = Filter.withFilterEffects f [fe1; fe2] |> Filter.toString
        Assert.Contains("feGaussianBlur", result)
        Assert.Contains("feColorMatrix", result)