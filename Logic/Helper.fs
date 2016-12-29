namespace QuizPresentator

[<AutoOpen>]
module Helper =
    let updateList list item item' =
        List.map (fun i -> if i = item then item' else i) list