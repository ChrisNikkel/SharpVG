namespace SharpVG.Tests

open SharpVG
open Xunit

module TestScript =

    [<Fact>]
    let ``toString wraps in script tag with CDATA`` () =
        let result = Script.toString "alert('hello')"
        Assert.Equal("<script type=\"application/ecmascript\"><![CDATA[alert('hello')]]></script>", result)

    [<Fact>]
    let ``toString empty string`` () =
        let result = Script.toString ""
        Assert.Equal("<script type=\"application/ecmascript\"><![CDATA[]]></script>", result)

    [<Fact>]
    let ``toString preserves content verbatim`` () =
        let js = "var x = 1 < 2 && 3 > 0;"
        let result = Script.toString js
        Assert.Contains(js, result)
