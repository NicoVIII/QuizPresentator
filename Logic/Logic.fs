module QuizPresentation.Logic

type QuestionString = Question of string
type AnswerString = string
type AnswerIndex = | A | B | C | D

type Question = {question: QuestionString; answers: AnswerString * AnswerString * AnswerString * AnswerString; correct: AnswerIndex}
type public Quiz = {questions: Question list; results: bool list; nrOfQuestions: int}

// Creation
let private question question correct answerA answerB answerC answerD = {question = Question question; answers = (answerA, answerB, answerC, answerD); correct = correct}
let private emptyQuiz = {questions = []; results = []; nrOfQuestions = 0}
let private addQuestion quiz question =
    match quiz with
    | {questions = questions; results = results; nrOfQuestions = length} -> {questions = questions @ [question]; results = results; nrOfQuestions = length + 1}

let initQuiz =
    let quiz = emptyQuiz
    let quiz' = addQuestion quiz (question "Testfrage! Eine sehr sehr lange Testfrage, um die Zeilenumbrüche etc. zu testen! BOAH wie geil!" A "Answer A" "Answer B" "Answer C" "Answer D")
    let quiz'' = addQuestion quiz' (question "Dies ist die zweite Frage! Unglaublich..." B "Answer A2" "Answer B2" "Answer C2" "Answer R2D2")
    quiz''

// Simple Getter
let private getQuestion quiz =
    match quiz with
    | {questions = []} -> None
    | {questions = question::_} -> Some(question)
let private getAnswers quiz =
    match getQuestion quiz with
    | None -> ("", "", "", "")
    | Some {question = _; answers = answers; correct = _} -> answers

// Getter for Gui
let getQuestionString quiz =
    match getQuestion quiz with
    | None -> ""
    | Some {question = Question question; answers = _; correct = _} -> question

let private getAnswerString answerIndex quiz =
    let (aA, aB, aC, aD) = getAnswers quiz
    match answerIndex with | A -> aA | B -> aB | C -> aC | D -> aD
let getAnswerA = getAnswerString A
let getAnswerB = getAnswerString B
let getAnswerC = getAnswerString C
let getAnswerD = getAnswerString D

let getLength {questions = _; results = _; nrOfQuestions = length} = length

let getResults  {questions = _; results = results; nrOfQuestions = _} = results

let isEnded {questions = questions; results = _; nrOfQuestions = _} =
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
    | {questions = _::restQuestions; results = results; nrOfQuestions = length} ->
        match checkAnswer quiz answer with
        | true ->
            let quiz' = {questions = restQuestions; results = results @ [true]; nrOfQuestions = length}
            quiz'
        | false ->
            let quiz' = {questions = restQuestions; results = results @ [false]; nrOfQuestions = length}
            quiz'

// Define object functions for use in C#
type Quiz with
    member this.Question = getQuestionString this
    member this.AnswerA = getAnswerA this
    member this.AnswerB = getAnswerB this
    member this.AnswerC = getAnswerC this
    member this.AnswerD = getAnswerD this
    member this.Size = getLength this
    member this.Ended = isEnded this
    member this.Results = getResults this
    member this.CheckAnswer index = checkAnswer this index
    member this.ChooseAnswer index = chooseAnswer this index