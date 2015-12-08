// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"
open Fake

// Properties
let buildObjDir = "./SharpVG/obj"
let buildBinDir = "./SharpVG/bin"
let testObjDir  = "./Tests/obj"
let testBinDir  = "./Tests/bin"

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildObjDir; buildBinDir]
    CleanDirs [testObjDir; testBinDir]
)

Target "Build" (fun _ ->
    MSBuildWithDefaults "Build" ["./SharpVG.sln"]
    |> Log "AppBuild-Output: "
)

// Dependencies
"Clean"
  ==> "Build"

// start build
RunTargetOrDefault "Build"