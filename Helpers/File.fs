namespace Helpers

open System.Diagnostics
open System.IO
open System

module File =
    let saveToFile name lines =
        File.WriteAllLines(name, [lines]);

    let openFile (name:string) =
        Process.Start(name) |> ignore
