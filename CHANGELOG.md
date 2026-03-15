# Changelog

All notable changes to SharpVG will be documented in this file.

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
