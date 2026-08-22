# Changelog

All notable changes to SharpVG will be documented in this file.

## [1.0.0] - 2026-08-22

### Added
- **Round-trip tests** — 28 tests covering all major SVG constructs; exposed and fixed several parser gaps
- **`HRef` type** (`IdRef of ElementId | UrlRef of string`) with `HRef.ofId`, `HRef.ofUrl`, `HRef.toString` — type-safe distinction between element-id references (`href="#id"`) and URL references; replaces bare `string` in href positions
- **`Timing.createImmediate`** — create timing with no `begin` attribute (animation starts immediately); `Timing.withBegin` sets begin on an existing `Timing`
- **`FilterEffect.createDiffuseLightingWithKernelUnitLength`** — variant of `createDiffuseLighting` that sets `kernelUnitLength`
- **`FilterEffect.withResult`** — set a `result` name on any filter effect (enables downstream effects to reference it)
- **`FilterEffect.withInput`** — set the primary `in` source on any effect that accepts one
- **`FilterEffectSource.ResultRef`** — reference a previous effect's result by name string
- **`Filter.ofChain`** — build a filter from a list of effects, automatically wiring `result`/`in` between steps
- **`Filter.ofEffects`** — build a filter from a list of effects without auto-wiring
- **SVG parsing** — `SvgParser` module for loading SVG into the SharpVG model:
  - `SvgParser.ofString`, `ofFile`, `ofStream` — parse SVG from string, file, or stream
  - `SvgParser.ofGzipStream`, `ofGzipFile` — parse SVGZ (gzip-compressed SVG)
  - `SvgParser.ofHtmlString`, `ofHtmlFile` — extract and parse all `<svg>` elements embedded in HTML (supports XHTML and HTML5)
  - `SvgParser.stripUnknown` — remove unrecognized elements from a parsed SVG
  - Recognized elements: `circle`, `ellipse`, `rect`, `line`, `path`, `polygon`, `polyline`, `text`, `image`, `g`, `use`, `a`, `linearGradient`, `radialGradient`, `clipPath`, `mask`, `pattern`, `marker`, `filter`, `symbol`
  - Unrecognized elements preserved as raw `Element` values (round-trip faithful); `Element.isRaw` / `Element.rawContent` to inspect
  - `ParseResult<T>` carries parsed value plus any non-fatal `ParseWarning` list
- **Mutation helpers** on `Svg`:
  - `Svg.mapElements`, `mapElementsWhere` — transform elements recursively
  - `Svg.findById`, `findAll` — locate elements by id or predicate
  - `Svg.replaceById` — replace a named element
  - `Svg.addElement`, `addElements`, `addGroup` — append to SVG body
  - `Svg.removeById`, `removeWhere` — remove elements from SVG body (recurses into groups)
- **Mutation helpers** on `Group`:
  - `Group.mapElements`, `findById` — transform or locate elements within a group
  - `Group.removeById`, `removeWhere` — remove elements from a group (recurses into nested groups)
- **Element attribute API**:
  - `Element.getAttribute`, `withAttribute`, `removeAttribute` — read, write, remove individual attributes
  - `Element.clearAnimations`, `removeAnimationWhere`, `mapAnimations` — animation editing helpers
  - `Element.isRaw`, `rawContent`, `ofRaw` — inspect and construct raw/passthrough elements
- **Editor rendering** on `Svg`:
  - `Svg.toStringForEditing`, `toHtmlForEditing` — render SVG with ephemeral `data-edit-id` attributes encoding each element's tree position; both `<g>` tags and leaf elements are annotated
  - `Svg.parseEditPath` — parse a `data-edit-id` string back to an `int list` path
  - `Svg.findAtEditPath`, `mapAtEditPath` — locate or transform the element at a given tree path
  - `Svg.findGroupAtEditPath`, `mapGroupAtEditPath` — locate or transform the group at a given tree path
- `SvgDefinitions.addSymbol` — add a `Symbol` to a definitions block
- `SymbolDef` case added to `SvgDefinitionsContent` DU
- **`<style>` block parsing** — CSS rules inside `<style>` elements (both direct children of `<svg>` and inside `<defs>`) are parsed and applied to matching elements:
  - Class selectors (`.foo`) matched against `element.Classes`
  - Element-type selectors (`circle`, `rect`, …) matched against the element's tag name
  - ID selectors (`#myId`) matched against the element's `id` attribute
  - Multi-selector rules (`circle, .foo { … }`) supported
  - CSS comments stripped before parsing
  - Inline style attributes take precedence over stylesheet rules (sheet fills in fields not already set)
  - Extended `tryParseCssProperty` to cover all `Style` record fields: `clip-path`, `filter`, `marker-start`, `marker-mid`, `marker-end`, `stroke-miterlimit`, `mask`, `paint-order`, `vector-effect`, `shape-rendering`
- **Parse mode** — `ParseMode` DU (`Lenient` | `Strict`) controls how unknown elements are handled:
  - `Lenient` (default): unknown elements silently preserved as raw passthrough values
  - `Strict`: unknown body and definition elements each produce a `ParseWarning`; the element is still parsed as a raw passthrough
  - All entry points now have a `…With` variant accepting a `ParseMode`: `SvgParser.ofStringWith`, `ofFileWith`, `ofStreamWith`, `ofGzipStreamWith`, `ofGzipFileWith`, `ofHtmlStringWith`, `ofHtmlFileWith`
  - Existing `ofString`, `ofFile`, etc. unchanged — they use `Lenient` mode

### Changed
- **`Timing.Begin`** changed from `TimeSpan` to `TimeSpan option`; `None` omits the `begin` attribute so the animation starts immediately
- **`Timing.withResart`** renamed to **`Timing.withRestart`** (typo fix)
- **`Element.Href`**, **`TextPath.Href`**, **`LinearGradient.Href`**, **`RadialGradient.Href`** changed from `string option` / `ElementId` to `HRef option` / `HRef`; `withHref` functions updated accordingly
- **`Attribute.createHref`** now takes `HRef` instead of `string` — `#` prefix is no longer added unconditionally; use `HRef.ofId` for element references
- **`SvgParser` filter effect parsing** — `<feGaussianBlur>`, `<feOffset>`, `<feBlend>`, `<feColorMatrix>`, `<feFlood>`, `<feTurbulence>`, `<feMorphology>`, `<feDropShadow>`, `<feComposite>`, `<feMerge>` are now parsed from `<filter>` children; `result` and `in` attributes preserved
- **`SvgParser` presentation attribute parsing** — `clip-path`, `filter`, `mask`, `marker-start`, `marker-mid`, `marker-end`, `stroke-miterlimit`, `paint-order`, `vector-effect`, `shape-rendering` now read as presentation attributes (not only from `style="..."`)
- **`SvgParser` symbol id preservation** — `<symbol id="...">` now stores the symbol as a named element so the id round-trips
- **`FilterEffect.Result`** field added (`string option`); `FilterEffect.ToTag` emits `result="..."` when set
- **`ColorMatrix.Matrix`** element type changed from `int` to `float` — matrix values can now represent fractional coefficients
- **`DiffuseLighting.KernelUnitLength`** changed from `float` to `float option`; `createDiffuseLighting` and `createDiffuseLightingWithInput` no longer take a `kernelUnitLength` argument (defaults to `None`)

## [0.1.0] - 2026-03-14

### Added
- Gradients (linear and radial) with full stop support
- Stroke styling (dash arrays, line caps, line joins, miter limit)
- Clip paths (`ClipPath` module)
- Markers (arrowheads and other path decorations)
- Masks (`Mask` module)
- Patterns (`Pattern` module)
- `tspan` support within `Text` elements
- Filter effects and filter primitives (`Filter`, `FilterEffect` modules)
- SVG definitions (`SvgDefinitions`) for reusable elements
- Values-based animation support (issue #43)
- Fable/browser example (`Examples/FableClock`)

### Fixed
- 11 SVG output correctness bugs found via combination and validation analysis
- Various attribute rendering issues across shapes and styles

### Changed
- Test line coverage increased from 49% to 77%, branch coverage from 34% to 64%
- XML-validated tests using `xmllint`
- Promoted from alpha to stable release

## [0.0.20] - 2024

### Added
- Named styles scoped to specific SVG element types
- `create with class` for elements
- Additional `Length` units: `cm`, `mm`, `pt`, `in`
- Support for SVG animations and transformations
- SVG definitions support

### Changed
- Improved documentation; API docs moved to SharpVG wiki
