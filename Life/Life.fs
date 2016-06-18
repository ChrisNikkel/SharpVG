module LifeExample

open SharpVG
open System
open System.Diagnostics
open System.IO

let combine (a, b) = (fst a + fst b, snd a + snd b) 

let surrounding = 
    [for i in -1..1 -> [for j in -1..1 -> (i, j) ] ] 
    |> List.concat

//TODO: use collect instead of this
let findSurrounding d = 
    surrounding 
    |> List.map(fun cell -> combine(d, cell)) 
    |> List.filter(fun x -> x<>d)

let categorizeNeighbors d centerCell = 
    List.partition (fun cell -> (List.exists (fun c -> c = cell) d)) (findSurrounding centerCell)   
   
let countNeighbors d centerCell =
    List.length (fst (categorizeNeighbors d centerCell))
    
   
// Any live cell with fewer than two live neighbours dies, as if caused by under-population.
// Any live cell with two or three live neighbours lives on to the next generation.
// Any live cell with more than three live neighbours dies, as if by overcrowding.
// Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
let survives neighborCount =
    match neighborCount with
        | 2 | 3 -> true
        | _ -> false

let born neighborCount = if neighborCount = 3 then true else false

let iterateDeadGeneration previousData deadCells =
    deadCells |> List.filter(fun deadCell -> born (countNeighbors previousData deadCell))

let removeDups data = data |> Set.ofList |> Set.toList

let rec iterateAliveGeneration previousData newData cellIndex =
    if cellIndex < List.length previousData then
        let aliveCell = previousData.[cellIndex] 
        let (aliveNeighbors, deadNeighbors) = categorizeNeighbors previousData aliveCell
        let newAliveData = 
            if survives (List.length aliveNeighbors)
            then aliveCell :: newData |> removeDups
            else newData
        iterateDeadGeneration previousData deadNeighbors 
            |> List.append (iterateAliveGeneration previousData (newAliveData) (cellIndex + 1)) |> removeDups
    else
        newData 

let doIteration initialData = (iterateAliveGeneration initialData [] 0)

let setItemPosition x y item = item |> List.map (fun (ix, iy) -> (ix + x, iy + y))
let addItem item x y map = map |> List.append (item |> setItemPosition x y)
let crossItem = [(1, 1); (1, 2); (1, 3)]
let gliderItem = [(1, 1); (2, 1); (3, 1); (3, 2); (2, 3)]
let toadItem = [(1, 1); (2, 1); (3, 1); (2, 2); (3, 2); (4, 2)]
let blockItem = [(1, 1); (1, 2); (2, 1); (2, 2)]
let beaconItem = [(4, 1); (4, 2); (3, 1); (1, 3); (1, 4); (2, 4)]


[<EntryPoint>]
let main argv = 

    // Helper Functions
    let saveToFile name lines =
        File.WriteAllLines(name, [lines]);

    let openFile (name:string) =
        Process.Start(name) |> ignore

    // Initalize
    let fileName = ".\\life.html"

    let initialData = 
        blockItem |> setItemPosition 25 46
        |> addItem gliderItem 5 65 |> addItem gliderItem 25 52 |> addItem gliderItem 25 65
        |> addItem gliderItem 5 52 |> addItem crossItem 20 50 |> addItem toadItem 50 50
        |> addItem gliderItem 22 22 |> addItem crossItem 44 11 |> addItem toadItem 23 55
        |> addItem blockItem 50 35 |> addItem beaconItem 70 60 |> addItem beaconItem 30 30
        |> addItem beaconItem 10 60 |> addItem beaconItem 30 10 |> removeDups
            
    let iterations, delay, size = 10, 1.0, 10.0

    let style = { Stroke = Some(Name Colors.Black); StrokeWidth = Some(Pixels 1.0); Fill = Some(Name Colors.White); Opacity = None }
    // Execute
    
    let makeElement x y =
        let point = Point.create <| Pixels (float x * size) <| Pixels (float y * size)
        Circle.create point <| Pixels size |> Element.ofCircle 

    let addAnimation element times =
        let duration = Duration (TimeSpan.FromSeconds(delay))
        let createAnimation t =
            let start = TimeSpan.FromSeconds(t)        
            let timing = Timing.create start |> (Timing.withDuration duration)
            Animation.createSet timing AttributeType.CSS "display" "inline"
        let initial = Animation.createSet (Timing.create (TimeSpan.FromSeconds(0.0))) AttributeType.CSS "display" "none" |> Seq.singleton
        let animations = initial |> Seq.append (times |> Seq.map (fun t -> ((createAnimation (t * delay)))))
        element |> (Element.withAnimations animations)

    seq { 1 .. iterations }
    |> Seq.scan (fun (d, t) _ -> (doIteration d, t + delay)) (initialData, 0.0)
    |> Seq.collect (fun (p, t) -> p |> List.map (fun (x, y) -> ((x, y), t)))
    |> Seq.groupBy (fun ((x, y), t) -> (x, y))
    |> Seq.map (fun ((x, y), d) -> ((x, y), d |> Seq.map snd))
    |> Seq.map (fun ((x, y), times) -> addAnimation (makeElement x y) times)
    |> Seq.map (Element.withStyle style)
    |> Group.ofSeq
    |> Group.withTransform (Transform.createWithScale (1.0, -1.0) |> Transform.withTranslate (0.0, 1000.0))
    |> Svg.ofGroup
    |> Svg.withSize {Height = Pixels 1000.0; Width = Pixels 1000.0}
    |> Svg.withViewbox {Minimums = { X = Pixels 0.0; Y = Pixels 0.0}; Size = {Height = Pixels 1000.0; Width = Pixels 1000.0 }}
    |> Svg.toHtml "SVG Life Example"
    |> saveToFile fileName


//    seq { 1 .. iterations }
//    |> Seq.scan (fun (d, t) _ -> (doIteration d, t + delay)) (initialData, 0.0)
//    |> Seq.map (fun (p, t) -> p |> List.map (fun (x, y) -> ((x, y), t)))
//    |> Seq.map (fun d -> d |> List.groupBy (fun ((x, y), t) -> (x, y)))
//    |> Seq.map (fun d -> d |> List.map (fun (p, d) -> (p, d |> List.map (fun (p, t) -> t))))
//    |> Seq.map (fun d -> d |> List.map (fun ((x, y), t) -> addAnimation (makeElement x y) t))
//    |> Svg.ofSeq
//    |> Svg.toHtml "Life"
//    |> saveToFile fileName
    openFile fileName
    0 // return an integer exit code
