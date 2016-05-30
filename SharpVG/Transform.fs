namespace SharpVG

type transform =
    | Matrix of int * int * int * int * int * int
    | Translate of int * int
    | Scale of int * int
    | Rotate of int * int * int
    | SkewX of int
    | SkewY of int

module Transform =
    let toString transform =
         match transform with
            | Matrix (a, b, c, d, e, f) -> "matrix(" + string a + " "  + string b + " "  + string c + " "  + string d + " "  + string e + " "  + string f + ")"
            | Translate(x, y) -> "translate(" + string x + " " + string y + ")"
            | Scale(x, y) -> "scale(" + string x + " " + string y + ")"
            | Rotate(a, x, y) -> "rotate(" + string a + " " + string x + " " + string y + ")"
            | SkewX(a) -> "skewX(" + string a + ")"
            | SkewY(a) -> "skewY(" + string a + ")"
