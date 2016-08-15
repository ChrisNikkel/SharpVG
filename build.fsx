// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"
open Fake

// Properties
let buildSharpVGReleaseDir = "./SharpVG/bin/Release"
let buildSharpVGDebugDir = "./SharpVG/bin/Debug"
let buildSharpVGTestDir  = "./Tests/bin/Debug"

// Targets
Target "Clean" (fun _ ->
  CleanDirs [buildSharpVGReleaseDir; buildSharpVGDebugDir; buildSharpVGTestDir]
)

Target "Build" (fun _ ->
  !! "./SharpVG/SharpVG.fsproj"
  |> MSBuildRelease buildSharpVGReleaseDir "Build"
  |> Log "AppBuild-Output: "
)

Target "BuildDebug" (fun _ ->
  !! "./SharpVG/SharpVG.fsproj"
  |> MSBuildDebug buildSharpVGDebugDir "Build"
  |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
  !! "./Tests/Test.fsproj"
  |> MSBuildDebug buildSharpVGTestDir "Build"
  |> Log "AppBuild-Output: "
)

// Dependencies
"Clean"
  ==> "Build"
  ==> "BuildDebug"
  ==> "BuildTest"

// start build
RunTargetOrDefault "Build"
