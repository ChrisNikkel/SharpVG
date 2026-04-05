# Requirements: SVG Parsing & Mutation

## Problem Statement

SharpVG currently only generates SVG — it has no way to read existing SVG files. This means users cannot:
- Load a designer-produced SVG and programmatically modify it
- Round-trip SVG (load → mutate → save)
- Compose SharpVG-generated elements with existing SVG assets
- Build tools that process SVG pipelines (e.g. post-processing, templating, batch transforms)

This document captures the requirements for adding SVG parsing and mutation capabilities.

---

## Goals

1. **Parse** valid SVG strings/files into the existing SharpVG domain model
2. **Mutate** parsed (or generated) SVG using the existing SharpVG API
3. **Round-trip** SVG losslessly for elements SharpVG understands, and faithfully for elements it doesn't
4. **Compose** parsed SVGs with programmatically generated elements

---

## Non-Goals

- Full SVG spec compliance (we target the subset SharpVG can already express)
- SVGZ (compressed SVG) support in v1
- CSS stylesheet parsing (class-based styles remain opaque strings unless already named styles)
- JavaScript/scripting execution
- Rasterization or rendering to pixels

---

## Functional Requirements

### FR-1: Parse SVG from string
```
SvgParser.ofString : string -> Result<Svg, ParseError>
```
- Accepts any valid SVG string
- Returns a `Svg` record (the existing top-level type) on success
- Returns a `ParseError` with position and message on failure

### FR-2: Parse SVG from file
```
SvgParser.ofFile : string -> Result<Svg, ParseError>
```
- Reads a `.svg` file at the given path
- Delegates to `ofString` after reading

### FR-3: Parse SVG from stream
```
SvgParser.ofStream : System.IO.Stream -> Result<Svg, ParseError>
```
- For use in web/embedded contexts

### FR-4: Recognized element mapping
The parser must map SVG elements to existing SharpVG types where possible:

| SVG element | SharpVG type |
|---|---|
| `<circle>` | `Circle` |
| `<ellipse>` | `Ellipse` |
| `<rect>` | `Rect` |
| `<line>` | `Line` |
| `<path>` | `Path` |
| `<polygon>` | `Polygon` |
| `<polyline>` | `Polyline` |
| `<text>` | `Text` |
| `<image>` | `Image` |
| `<g>` | `Group` |
| `<use>` | `Use` |
| `<symbol>` | `Symbol` |
| `<a>` | `Anchor` |
| `<defs>` | `SvgDefinitions` |
| `<filter>` | `Filter` |
| `<linearGradient>` / `<radialGradient>` | `Gradient` |
| `<clipPath>` | `ClipPath` |
| `<mask>` | `Mask` |
| `<pattern>` | `Pattern` |
| `<marker>` | `Marker` |

### FR-5: Unknown element passthrough
SVG elements not in the table above (e.g. `<foreignObject>`, custom elements, `<animateMotion>`) must be preserved as opaque `RawElement` nodes. They must round-trip faithfully to their original XML string.

### FR-6: Attribute parsing
- Presentation attributes (fill, stroke, etc.) must be parsed into `Style` where recognized
- `transform` attributes must be parsed into `Transform` sequences
- `id` and `class` attributes must be parsed into `Element.Name` and `Element.Classes`

### FR-7: Mutation API
Parsed SVG should be mutable using the existing SharpVG API without any new mutation types:
```fsharp
SvgParser.ofFile "logo.svg"
|> Result.map (Svg.withSize (Area.ofInts (800, 600)))
```
Elements within a parsed SVG should be traversable and replaceable:
```fsharp
// Map over all elements
Svg.mapElements (fun el -> el |> Element.withStyle newStyle) svg

// Map elements matching a predicate
Svg.mapElementsWhere (Element.isNamed) (fun el -> ...) svg

// Find elements by id
Svg.findById "myCircle" svg   // -> Element option
```

### FR-8: Serialization (already exists, no change)
`Svg.toString` and `Svg.toHtml` continue to work unchanged on parsed SVGs.

### FR-9: Error reporting
Parse errors must include:
- Line and column number
- The offending token/element name
- A human-readable description

### FR-10: Partial parsing
If an SVG is mostly valid but contains one unrecognized structure, the parser should succeed with the valid portions and record warnings rather than failing entirely. (Opt-in strict mode available.)

---

## Non-Functional Requirements

### NFR-1: No new required dependencies
The parser uses only .NET built-ins: `System.Xml.Linq` for the XML layer and `System.Text.RegularExpressions` for SVG sub-language tokenization (e.g. `<path>` `d` attribute). No third-party parsing libraries required.

### NFR-1a: Prefer F# idioms over .NET/C# idioms
Where a choice exists, prefer F#-native approaches: active patterns over `if`/`else` dispatch chains, discriminated unions over inheritance hierarchies, recursive functions over imperative loops, immutable records over mutable state. The active-pattern-over-`XElement` style satisfies this without any additional dependencies.

### NFR-2: Netstandard2.1 compatible
Must target the same frameworks as the existing library.

### NFR-3: Performance
Parsing a 1 MB SVG file should complete in under 500ms on typical hardware.

### NFR-4: Immutability preserved
All parsed types remain immutable F# records. Mutation means creating modified copies, not in-place changes — consistent with the existing SharpVG style.

### NFR-5: Backward compatibility
No existing public API changes. All new functions are additive.

---

## User Stories

**US-1:** As a developer, I want to load a designer's SVG file and programmatically change all stroke colors to match a new brand palette.

**US-2:** As a developer, I want to parse an icon SVG, scale it to different sizes, and embed it in a generated HTML page.

**US-3:** As a developer, I want to load an SVG template with placeholder shapes (identified by id), replace them with generated SharpVG elements, and save the result.

**US-4:** As a developer, I want to merge two SVGs — one parsed, one generated — into a single document.

**US-5:** As a developer, I want to extract all `<path>` elements from an SVG file and apply a transform to each one.

---

## Open Questions

1. **Lossy unknown attributes:** If an element has attributes SharpVG doesn't model (e.g. `data-*`, `aria-*`), should they be preserved in a passthrough bag or silently dropped?
2. **Inline `<style>` blocks:** Should CSS class rules be parsed and associated with elements, or kept as opaque strings?
3. **`xlink:href` vs `href`:** SVG 1.1 uses `xlink:href`; SVG 2 uses `href`. Which do we normalize to?
4. **Streaming parse:** Is SAX-style streaming needed, or is DOM-based parsing sufficient for expected file sizes?
5. **Mutation traversal depth:** Should `Svg.mapElements` recurse into `<g>` and `<defs>`, or only the top-level body?
