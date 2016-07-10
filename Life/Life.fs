module LifeExample

open SharpVG
open System
open System.Diagnostics
open System.IO

let combine (a, b) = (fst a + fst b, snd a + snd b) 

let surrounding = 
    [for i in -1..1 -> [for j in -1..1 -> (i, j)]] 
    |> List.concat

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

let born neighborCount = neighborCount = 3

let iterateDeadGeneration previousData deadCells =
    deadCells |> List.filter(fun deadCell -> (countNeighbors previousData deadCell) |> born)

let removeDups data = data |> Set.ofList |> Set.toList

let rec iterateAliveGeneration previousData newData cellIndex =
    if cellIndex < List.length previousData then
        let aliveCell = previousData.[cellIndex] 
        let (aliveNeighbors, deadNeighbors) = categorizeNeighbors previousData aliveCell
        let newAliveData = 
            if survives (List.length aliveNeighbors)
            then aliveCell :: newData |> removeDups
            else newData
        (iterateAliveGeneration previousData newAliveData (cellIndex + 1)) @ (iterateDeadGeneration previousData deadNeighbors) |> removeDups
    else
        newData 

let doIteration initialData = (iterateAliveGeneration initialData [] 0)

let setItemPosition x y item = item |> List.map (fun (ix, iy) -> (ix + x, iy + y))
let addItem item x y map = (item |> setItemPosition x y) @ map

let crossItem = [(1, 1); (1, 2); (1, 3)]
let gliderItem = [(1, 1); (2, 1); (3, 1); (3, 2); (2, 3)]
let toadItem = [(1, 1); (2, 1); (3, 1); (2, 2); (3, 2); (4, 2)]
let blockItem = [(1, 1); (1, 2); (2, 1); (2, 2)]
let beaconItem = [(4, 1); (4, 2); (3, 1); (1, 3); (1, 4); (2, 4)]

let randomGenerator = Random()
let randomsBetween min max = Seq.initInfinite (fun _ -> randomGenerator.Next(min,  max+1))

let itemByNumber number =
    match number with
        | 1 -> gliderItem
        | 2 -> crossItem
        | 3 -> toadItem
        | 4 -> beaconItem
        | _ -> blockItem

[<EntryPoint>]
let main argv = 

    // Helper Functions
    let saveToFile name lines =
        File.WriteAllLines(name, [lines]);

    let openFile (name:string) =
        Process.Start(name) |> ignore

    // Initalize
    let fileName = ".\\life.html"
    
    let iterations, delay, cellSize = 100, 0.25, 15.0
    let size = 70
    let boardSize = cellSize * float size
    let boardLength = Length.ofFloat boardSize

    let randomItems =
        let quantity = randomsBetween 20 40 |> Seq.take 1 |> Seq.head
        let randomPositions = randomsBetween 0 (size - 5)
        let randomXs = randomPositions |> Seq.take quantity
        let randomYs = randomPositions |> Seq.skip quantity |> Seq.take quantity
        let randomItemTypes = randomsBetween 1 5 |> Seq.take quantity
        Seq.zip3 randomItemTypes randomXs randomYs |> List.ofSeq

    let initialData =
        randomItems
        |> List.scan (fun board (itemType, x, y) -> board |> addItem (itemByNumber itemType) x y) []
        |> List.concat
        |> removeDups

    let style = { Stroke = Some(Name Colors.Black); StrokeWidth = Some(Length.ofInt 1); Fill = Some(Name Colors.White); Opacity = None }

    let namedStyle = style |> NamedStyle.ofStyle "std"

    // Execute

    let makeElement x y =
        let point = Point.create <| Length.ofFloat (float x * cellSize) <| Length.ofFloat (float y * cellSize)
        Circle.create point <| Length.ofFloat (cellSize / 2.0) |> Element.ofCircle

    let addAnimation element times =
        let duration = Duration (TimeSpan.FromSeconds(delay))
        let createAnimation t =
            let start = TimeSpan.FromSeconds(t)        
            let timing = Timing.create start |> (Timing.withDuration duration)
            Animation.createSet timing AttributeType.CSS "display" "inline"
        let animations =
            Animation.createSet (Timing.create (TimeSpan.FromSeconds(0.0))) AttributeType.CSS "display" "none"
            |> Seq.singleton
            |> Seq.append (times |> Seq.map ((*) delay >> createAnimation))
        element |> Element.withAnimations animations

    seq { 1 .. iterations }
    |> Seq.scan (fun (d, t) _ -> (doIteration d, t + delay)) (initialData, 0.0)
    |> Seq.collect (fun (p, t) -> p |> List.map (fun (x, y) -> ((x, y), t)))
    |> Seq.groupBy (fun ((x, y), t) -> (x, y))
    |> Seq.map (fun ((x, y), times) -> addAnimation (makeElement x y) (times |> Seq.map snd) |> Element.withNamedStyle namedStyle)
    |> Group.ofSeq
    |> Group.asCartesian Length.empty (Length.ofFloat boardSize)
    |> Svg.ofGroup
    |> Svg.withStyle namedStyle
    |> Svg.withSize {Height = boardLength; Width = boardLength}
    |> Svg.withViewbox {Minimums = Point.ofInts (0, 0); Size = Area.full}
    |> Svg.toHtml "SVG Life Example"
    |> saveToFile fileName

    openFile fileName
    0 // return an integer exit code
