module QuizPresentation.Logic

type QuestionString = Question of string
type AnswerString = string
type AnswerIndex = | A | B | C | D

let indexFromString string =
    match string with
    | "A" | "a" | "1" -> A
    | "B" | "b" | "2" -> B
    | "C" | "c" | "3" -> C
    | "D" | "d" | "4" -> D
    | _ -> failwith "no valid string from AnswerIndex"

type Question = {question: QuestionString; answers: AnswerString * AnswerString * AnswerString * AnswerString; correct: AnswerIndex}
type public Quiz = {questions: Question list; results: bool list; nrOfQuestions: int; nrOfParties: int}

// Creation
let private question question correct answerA answerB answerC answerD = {question = Question question; answers = (answerA, answerB, answerC, answerD); correct = correct}
let private emptyQuiz parties = {questions = []; results = []; nrOfQuestions = 0; nrOfParties = parties}
let private addQuestion quiz question =
    match quiz with
    | {questions = questions; nrOfQuestions = length} -> {quiz with questions = questions @ [question]; nrOfQuestions = length + 1}

(*let initQuiz =
    let quiz = emptyQuiz
    let quiz' = addQuestion quiz (question "Testfrage! Eine sehr sehr lange Testfrage, um die Zeilenumbrüche etc. zu testen! BOAH wie geil!" A "Answer A" "Answer B" "Answer C" "Answer D")
    let quiz'' = addQuestion quiz' (question "Dies ist die zweite Frage! Unglaublich..." B "Answer A2" "Answer B2" "Answer C2" "Answer R2D2")
    quiz''*)

// Simple Getter
let private getQuestion quiz =
    match quiz with
    | {questions = []} -> None
    | {questions = question::_} -> Some(question)
let private getAnswers quiz =
    match getQuestion quiz with
    | None -> ("", "", "", "")
    | Some {answers = answers} -> answers

// Getter for Gui
let getQuestionString quiz =
    match getQuestion quiz with
    | None -> ""
    | Some {question = Question question} -> question

let private getAnswerString answerIndex quiz =
    let (aA, aB, aC, aD) = getAnswers quiz
    match answerIndex with | A -> aA | B -> aB | C -> aC | D -> aD
let getAnswerA = getAnswerString A
let getAnswerB = getAnswerString B
let getAnswerC = getAnswerString C
let getAnswerD = getAnswerString D

let getLength {nrOfQuestions = length} = length

let getResults {results = results} = results

let getNrOfParties {nrOfParties = parties} = parties

let isEnded {questions = questions} =
    match questions with
    | [] -> true
    | question::_ -> false

// Flow
let private checkAnswer quiz answer =
    match getQuestion quiz with
    | None -> failwith "Quiz has no valid state. No question to choose an answer from!"
    | Some {question = _; answers = _; correct = correct} -> answer = correct

let chooseAnswer quiz answer =
    match quiz with
    | {questions = []} ->
        failwith "Quiz has no valid state. No question to choose an answer from!"
    | {questions = _::restQuestions; results = results} ->
        match checkAnswer quiz answer with
        | true -> {quiz with questions = restQuestions; results = results @ [true]}
        | false -> {quiz with questions = restQuestions; results = results @ [false]}

// Questions from file
let readLines filePath = 
    let lines = System.IO.File.ReadLines(filePath)
    List.ofSeq lines

let getArgsFromLine (line : string) =
    let args = line.Split ':'
    List.ofArray args

let addQuestionFromLine (line : string) quiz =
    let args = getArgsFromLine line
    match args with
    | questionString::correct::answerA::answerB::answerC::[answerD] ->
        let index = indexFromString correct
        addQuestion quiz (question questionString index answerA answerB answerC answerD)
    | _ -> failwith "line has wrong number of arguments"

let rec addQuestionsFromLines lines quiz =
    match lines with
    | line::rest ->
        let quiz' = addQuestionFromLine line quiz
        addQuestionsFromLines rest quiz'
    | [] ->
        quiz
    
let initQuizFromFile filePath =
    match readLines filePath with
    | [] -> failwith "file is empty"
    | partiesString::lines ->
        match System.Int32.TryParse(partiesString) with
        // First line is a single integer
        | (true,parties) -> 
            let quiz = emptyQuiz parties
            addQuestionsFromLines lines quiz
        | _ -> 
            let quiz = emptyQuiz 1
            addQuestionsFromLines lines quiz

// Define object functions for use in C#
type Quiz with
    member this.Question = getQuestionString this
    member this.AnswerA = getAnswerA this
    member this.AnswerB = getAnswerB this
    member this.AnswerC = getAnswerC this
    member this.AnswerD = getAnswerD this
    member this.Size = getLength this
    member this.Ended = isEnded this
    member this.NrOfParties = getNrOfParties this
    member this.Results = getResults this
    member this.CheckAnswer index = checkAnswer this index
    member this.ChooseAnswer index = chooseAnswer this index