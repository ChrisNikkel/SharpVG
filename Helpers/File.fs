namespace Helpers

open System.Diagnostics
open System.IO

module File =
    let getTemporaryFileName =
        Path.GetTempFileName()

    let saveToFile name lines =
        File.WriteAllLines(name, [lines])

    let openFile (name:string) =
        let info = new ProcessStartInfo("open", name)
        Process.Start(info) |> ignore
