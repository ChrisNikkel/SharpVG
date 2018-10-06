namespace Helpers

open System.Diagnostics
open System.IO
open System.Runtime.InteropServices

module File =
    let getTemporaryFileNameWithExt ext =
        Path.ChangeExtension(Path.GetTempFileName(), ext)

    let saveToFile name lines =
        File.WriteAllLines(name, [lines])

    let openFile (name:string) =

        let checkOS os = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(os);
        let launchCommand =
            match checkOS OSPlatform.Windows, checkOS OSPlatform.OSX, checkOS OSPlatform.Linux with
                | true, _, _ -> "start"
                | _, true, _ -> "open"
                | _, _, true -> "xdg-open"
                | _, _, _ -> failwith ("Unsupported OS version: " + System.Runtime.InteropServices.RuntimeInformation.OSDescription)

        let info = new ProcessStartInfo(launchCommand, name)
        Process.Start(info) |> ignore
