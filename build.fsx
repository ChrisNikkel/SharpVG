// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Testing

// Properties
let sharpVGReleaseDir = "./SharpVG/bin/Release"
let sharpVGDebugDir = "./SharpVG/bin/Debug"
let sharpVGTestDir  = "./Tests/bin/Debug"
let sharpVGTestDlls = !! (sharpVGTestDir + "/Tests.dll")
let graphReleaseDir = "./Graph/bin/Release"
let lifeReleaseDir = "./Life/bin/Release"
let triangleReleaseDir = "./Triangle/bin/Release"

// Targets
Target "Clean" (fun _ ->
  CleanDirs [sharpVGReleaseDir; sharpVGDebugDir; sharpVGTestDir]
)

Target "Build" (fun _ ->
  !! "./SharpVG/SharpVG.fsproj"
  |> MSBuildRelease sharpVGReleaseDir "Build"
  |> Log "AppBuild-Output: "
)

Target "BuildDebug" (fun _ ->
  !! "./SharpVG/SharpVG.fsproj"
  |> MSBuildDebug sharpVGDebugDir "Build"
  |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
  !! "./Tests/Tests.fsproj"
  |> MSBuildDebug sharpVGTestDir "Build"
  |> Log "AppBuild-Output: "
)

Target "BuildExamples" (fun _ ->
  !! "./Graph/Graph.fsproj"
  |> MSBuildDebug graphReleaseDir "Build"
  |> Log "AppBuild-Output: "
  !! "./Life/Life.fsproj"
  |> MSBuildDebug lifeReleaseDir "Build"
  |> Log "AppBuild-Output: "
  !! "./Triangle/Triangle.fsproj"
  |> MSBuildDebug triangleReleaseDir "Build"
  |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
  sharpVGTestDlls
  |> xUnit2 (fun p ->
    {
      p with
        ShadowCopy = false;
    })
)
// Dependencies
"Clean"
  ==> "Build"
  ==> "BuildDebug"
  ==> "BuildTest"
"Build" ==> "BuildExamples"
"BuildTest" ==> "Test"

// start build
RunTargetOrDefault "Build"
