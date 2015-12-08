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

// define test dlls
let testDlls = !! (testBinDir + "/Release/Tests.dll")

Target "Test" (fun _ ->
    testDlls
        |> xUnit (fun p -> 
            {p with
                ToolPath = "./packages/xunit.runner.console/tools/xunit.console.x86.exe"
            })
)

// Dependencies
"Clean"
  ==> "Build"
"Build"
  ==> "Test"

// start build
RunTargetOrDefault "Build"