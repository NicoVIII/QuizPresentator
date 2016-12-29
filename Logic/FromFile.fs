namespace QuizPresentator

open QuizPresentator
open QuizPresentator.Creation

module QuizFromFile =
    let indexFromString string =
        match string with
        | "A" | "a" | "1" -> A
        | "B" | "b" | "2" -> B
        | "C" | "c" | "3" -> C
        | "D" | "d" | "4" -> D
        | _ -> failwith "no valid string from AnswerIndex"

    let readLines filePath = System.IO.File.ReadLines(filePath) |> List.ofSeq

    let getArgsFromLine (line : string) = line.Split ';' |> List.ofArray |> List.map (fun el -> el.Trim ' ')

    let addQuestionFromLine (line : string) party =
        let args = getArgsFromLine line
        match args with
        | questionString::correct::answerA::answerB::answerC::[answerD] ->
            let index = indexFromString correct
            addQuestion party (question questionString index answerA answerB answerC answerD)
        | _ -> failwith "line has wrong number of arguments"

    let rec addQuestionsFromLines index lines quiz =
        match lines with
        | line::rest ->
            let {parties = parties} = quiz
            let party = parties.[index]
            let party' = addQuestionFromLine line party
            let quiz' = {quiz with parties = List.mapi (fun i party -> if i = index then party' else party) parties}
            addQuestionsFromLines ((index + 1) % (List.length quiz'.parties)) rest quiz'
        | [] ->
            quiz
        
    let initQuizFromFile filePath =
        // TODO remove and implement some way to define lifelines inside of quiz.txt
        let lifelines = [lifeline "50-50" FiftyFiftyLL; lifeline "telephone" JustImageLL; lifeline "audience" JustImageLL; lifeline "additional" JustImageLL]

        let lines = readLines filePath
        match lines with
        | [] ->
            failwith "file is empty"
        | partiesString::questionLines ->
            match System.Int32.TryParse(partiesString) with
            // First line is a single integer
            | (true,parties) ->
                emptyQuiz parties
                |> addQuestionsFromLines 0 questionLines
                |> addLifelines lifelines
            | _ ->
                emptyQuiz 1
                |> addQuestionsFromLines 0 lines
                |> addLifelines lifelines