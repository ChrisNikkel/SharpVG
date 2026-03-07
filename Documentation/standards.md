# Design, code and documentation standards

This document defines consistent standards for documentation, testing, and code examples in SharpVG. Follow it when editing docs, adding tests, or writing examples (README, notebooks).

## Documentation (code examples)

### Named bindings

Do not use inline construction inside API calls in examples (e.g. no `Point.ofInts (x, y)` or `Length.ofInt n` inside the call). Define values with `let` first, then use the variable. Tuples of named bindings are fine when appropriate (e.g. `(startPoint, endPoint)` for a line, or `(fromPoint, toPoint)`).

### Standard names (use consistently)

- **Points**: `center` (circle/ellipse center), `position` (rect/shape position), `pathPoint1`, `pathPoint2`, `startingPoint` for path motion. For lines: `startPoint`, `endPoint` (or `fromPoint`, `toPoint`); passing `(startPoint, endPoint)` is appropriate.
- **Areas/sizes**: `area` (rect size), `size` when referring to dimensions.
- **Lengths**: `radius` (circle), `penWidth` (stroke), `pivot` (rotate center when a single length is reused for x/y).
- **Time**: `beginTime`, `duration`, `repeatDuration`.
- **Style**: `strokeColor`, `fillColor`, `strokePen`, `fillPen`, `style`.
- **Transform**: `translateX`, `translateY` (or `offsetX`, `offsetY`) when naming translate components; `transform` for the composed value.

### API

Use tuple form for `Point.ofInts` and `Area.ofInts`: `(x, y)` and `(w, h)`.

### Valid SVG

Every doc example must be runnable and produce valid SVG (no orphan animations or incomplete snippets).

---

## Testing

### Doc-proof tests

For each non-trivial code example in `Documentation/*.md`, there should be a test that runs the same logic (or the same example) and asserts on output (e.g. `Assert.Contains` for expected tags/attributes).

### Naming

Name facts that verify docs like `"Module wiki - short description"` or reference the doc and section (e.g. "Documentation/Group.md Creating groups example"). Keep a brief comment above the fact pointing to the doc path and section.

### Coverage

Prefer tests that both prove the doc and exercise the code path. Use the same variable names (center, radius, position, area, etc.) as the docs so examples and tests stay in sync.

---

## Coding (library / tests)

- **F# style**: Match existing style (modules, `let` bindings, pipelines).
- **SharpVG API**: Use `Point.ofInts (x, y)` and `Area.ofInts (w, h)` (tuple form), not the curried form.
