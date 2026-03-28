# Design

SharpVG is a library for generating SVG from F#. You build shapes and groups, wrap them in elements (with optional style, transform, and animation), then turn the result into an SVG document or HTML page. The diagram below shows how the main concepts fit together.

```mermaid
graph BT
    subgraph Geometry["Geometric Primitives"]
        Length
        Length-->Point & Area
    end
    subgraph BasicShapes["Basic Shapes"]
        Circle & Ellipse & Rect & Line & Polygon
    end
    subgraph ContentShapes["Path & Content"]
        Path & Polyline & Image & Text
    end
    subgraph Motion
        Timing-->Anim["Animation"]
    end
    subgraph Appearance
        Color-->Pen
        Pen-->Style
    end
    subgraph Effects
        FilterEffect-->Filter
    end
    subgraph XML["XML Infrastructure"]
        Attribute-->Tag
        ElementId
    end
    subgraph Composition
        Element & Anchor & Symbol & Use & Group & SvgDefinitions & Graphic & Transform
    end
    subgraph Document
        Svg & ViewBox & Script
    end
    Geometry-->BasicShapes
    Geometry-->ContentShapes
    BasicShapes-->Appearance
    ContentShapes-->Appearance
    Motion-->Composition
    Appearance-->Composition
    Effects-->Composition
    XML-->Composition
    Composition-->Document
```

## SVG specification (alignment)

SharpVG output is intended to conform to the following W3C specifications. Refer to them for element and attribute semantics, and for validation:

- [Scalable Vector Graphics (SVG) 1.1 (Second Edition)](https://www.w3.org/TR/SVG11/) — elements, attributes, coordinate systems, painting, text.
- [SVG 2](https://www.w3.org/TR/SVG2/) — modern SVG (supersedes 1.1 where implemented).
- [SVG Animation (SMIL)](https://www.w3.org/TR/smil-animation/) — `animate`, `set`, `animateTransform`, `animateMotion`, and timing.

## See also

For contribution and example standards, see [CLAUDE.md](../CLAUDE.md).

User-facing API documentation lives in the [SharpVG wiki](https://github.com/ChrisNikkel/SharpVG/wiki):

- [Style](https://github.com/ChrisNikkel/SharpVG/wiki/Style) — Pen and Style (stroke/fill)
- [Element](https://github.com/ChrisNikkel/SharpVG/wiki/Element) — Wrapping shapes and groups for output
- [Svg](https://github.com/ChrisNikkel/SharpVG/wiki/Svg) — Building the SVG document and HTML
- [Group](https://github.com/ChrisNikkel/SharpVG/wiki/Group) — Grouping elements
- [Transform](https://github.com/ChrisNikkel/SharpVG/wiki/Transform) — Transforms on elements and groups
- [Animation](https://github.com/ChrisNikkel/SharpVG/wiki/Animation) — SVG animation and timing
