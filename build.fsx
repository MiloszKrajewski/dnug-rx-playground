open Fake

#r ".fake/FakeLib.dll"
#load "build.tools.fsx"

let solutions = Proj.settings |> Config.keys "Build"
let packages = Proj.settings |> Config.keys "Pack"

//----

let clean () = !! "**/bin/" ++ "**/obj/" |> DeleteDirs
let restore () = solutions |> Seq.iter Proj.restore
let build () = solutions |> Seq.iter Proj.build
let test () = Proj.xtestAll ()
let release () = packages |> Proj.packMany

//----

Target "Clean" (fun _ -> clean ())

Target "Restore" (fun _ -> restore ())

Target "Build" (fun _ -> build ())

Target "Rebuild" ignore

Target "Release" (fun _ -> release ())

Target "Test" (fun _ -> test ())

"Restore" ==> "Build" ==> "Rebuild" ==> "Test" ==> "Release"
"Clean" ==> "Rebuild"
"Clean" ?=> "Restore"
"Build" ?=> "Test"


RunTargetOrDefault "Build"