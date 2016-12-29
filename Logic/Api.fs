namespace QuizPresentator

open QuizPresentator
open QuizPresentator.Creation

// Define object functions for use in C#
type AnswerIndex =
    | A = 1
    | B = 2
    | C = 3
    | D = 4

module private Convert =
    let convertAnswerIndex index =
        match index with
        | A -> AnswerIndex.A
        | B -> AnswerIndex.B
        | C -> AnswerIndex.C
        | D -> AnswerIndex.D

    let deconvertAnswerIndex (index :AnswerIndex) =
        match index with
        | AnswerIndex.A -> QuizPresentator.AnswerIndex.A
        | AnswerIndex.B -> QuizPresentator.AnswerIndex.B
        | AnswerIndex.C -> QuizPresentator.AnswerIndex.C
        | AnswerIndex.D -> QuizPresentator.AnswerIndex.D
        // TODO error handling
        | _ -> invalidArg "index" ""

type Lifeline(inner) =
    let inner = inner

    member this.Name =
        inner.name
    member this.Type =
        inner.``type``

type LifelineInfo(inner) =
    let inner = inner

    member this.Lifeline =
        new Lifeline(inner.lifeline)
    member this.Used =
        inner.used

type Question(inner) =
    let inner = inner

    member this.Question =
        let (QuestionString q) = inner.question
        q
    member this.AnswerA =
        let (AnswerString a,_,_,_) = inner.answers
        a
    member this.AnswerB =
        let (_,AnswerString b,_,_) = inner.answers
        b
    member this.AnswerC =
        let (_,_,AnswerString c,_) = inner.answers
        c
    member this.AnswerD =
        let (_,_,_,AnswerString d) = inner.answers
        d

    member this.CheckAnswer answer =
        Convert.deconvertAnswerIndex answer
        |> checkAnswer inner
    member this.HasResult =
        not (inner.result = Result None)
    member this.Result =
        let {result = Result result} = inner
        match result with
        | None -> invalidOp "No result for this question."
        | Some result -> result

type Party(inner) =
    let inner = inner

    member this.Active =
        inner.active
    member this.LifelineInfos =
        inner.lifelineInfos
        |> List.map (fun info -> new LifelineInfo(info))
    member this.Questions =
        inner.questions
        |> List.map (fun question -> new Question(question))
        |> List.toArray

    member this.Points =
        let sum question =
            match question.result with
            | Result None -> 0
            | Result (Some r) -> if r then 1 else 0

        inner.questions
        |> List.sumBy sum

type Quiz (inner) =
    let inner = inner

    member this.Parties =
        inner.parties
        |> List.map (fun party -> new Party(party))
        |> List.toArray

    member this.ActiveParty =
        new Party(QuizPresentator.getActiveParty inner)
    member this.ChooseAnswer index =
        Convert.deconvertAnswerIndex index
        |> chooseAnswer inner
        |> (fun args -> new Quiz(args))
    member this.CurrentQuestion =
        match QuizPresentator.getNextQuestionFromQuiz inner with
        | Some q -> new Question(q)
        // TODO error handling
        | None -> invalidOp ""
    member this.Ended = isEnded inner
    member this.Lifelines =
        inner.lifelines
        |> List.map (fun lifeline -> new Lifeline(lifeline))
        |> List.toArray
    member this.QuestionAmount =
        inner.parties
        |> List.sumBy (fun {questions = questions} -> List.length questions)

    static member FromFile path =
        new Quiz(QuizFromFile.initQuizFromFile path)