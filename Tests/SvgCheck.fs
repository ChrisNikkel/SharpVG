/// SvgCheck: validates SVG output using xmllint (well-formed XML check).
/// xmllint ships with macOS and most Linux distros (libxml2 package).
///
/// NOTE: svgcheck (pip package) was evaluated but is an IETF RFC document
/// validator — it rejects valid SVG constructs like fill="blue" and animation
/// children. xmllint well-formedness validation is the right lightweight check
/// for this project.
module SvgCheck

open System
open System.Diagnostics
open System.IO

let private xmllintCandidates =
    [
        "/usr/bin/xmllint"
        "/usr/local/bin/xmllint"
        "/opt/homebrew/bin/xmllint"
    ]

let private findXmllint () =
    xmllintCandidates |> List.tryFind File.Exists

/// Validates SVG as well-formed XML via xmllint --noout.
/// Returns Ok() on success, Error(messages) on failure, or Ok() if xmllint is unavailable.
let validate (svgContent: string) : Result<unit, string list> =
    match findXmllint () with
    | None -> Ok ()  // xmllint not found; skip validation
    | Some xmllintPath ->
        let tmpFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".svg")
        try
            File.WriteAllText(tmpFile, svgContent)
            let psi = ProcessStartInfo(xmllintPath, $"--noout \"{tmpFile}\"")
            psi.RedirectStandardOutput <- true
            psi.RedirectStandardError <- true
            psi.UseShellExecute <- false
            let proc = Process.Start(psi)
            let stderr = proc.StandardError.ReadToEnd()
            proc.WaitForExit()
            if proc.ExitCode = 0 then
                Ok ()
            else
                let errors =
                    stderr.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    |> Array.filter (fun line -> line.Trim() <> "")
                    |> Array.toList
                Error errors
        finally
            if File.Exists(tmpFile) then File.Delete(tmpFile)

/// Assert that an SVG string is well-formed XML. Fails with xmllint error details on violation.
let assertValid (svgContent: string) =
    match validate svgContent with
    | Ok () -> ()
    | Error messages -> Xunit.Assert.Fail("SVG XML validation failed:\n" + String.concat "\n" messages)
