namespace Helpers

open System.Diagnostics
open System.IO
open System.Runtime.InteropServices
open System.Reflection

module File =
    let getProgramPath =
        System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + (string Path.DirectorySeparatorChar)

    let getTemporaryFileNameWithExt ext =
        Path.ChangeExtension(Path.GetTempFileName(), ext)

    let saveToFile name lines =
        File.WriteAllLines(name, [lines])
