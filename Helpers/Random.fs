namespace Helpers

open System

module Random =
    let randomsBetween seed min max =
        let gen = Random((int)DateTime.Now.Ticks ^^^ seed.GetHashCode())
        Seq.initInfinite (fun _ -> gen.Next(min,  max+1))