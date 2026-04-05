# Changelog

All notable changes to SharpVG will be documented in this file.

## [Unreleased]

### Added
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
  - `Svg.toStringForEditing`, `toHtmlForEditing` — render SVG with ephemeral `data-edit-id` attributes encoding each element's tree position
  - `Svg.parseEditPath` — parse a `data-edit-id` string back to an `int list` path
  - `Svg.findAtEditPath`, `mapAtEditPath` — locate or transform the element at a given tree path
- `SvgDefinitions.addSymbol` — add a `Symbol` to a definitions block
- `SymbolDef` case added to `SvgDefinitionsContent` DU

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
