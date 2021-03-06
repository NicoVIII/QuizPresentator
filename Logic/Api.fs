namespace QuizPresenter

open QuizPresenter
open QuizPresenter.Creation
open System

// Define object functions for use in C#
type AnswerIndex =
    | A = 1
    | B = 2
    | C = 3
    | D = 4

module private ConvertAnswerIndex =
    let convert index =
        match index with
        | A -> AnswerIndex.A
        | B -> AnswerIndex.B
        | C -> AnswerIndex.C
        | D -> AnswerIndex.D

    let deconvert (index :AnswerIndex) =
        match index with
        | AnswerIndex.A -> QuizPresenter.AnswerIndex.A
        | AnswerIndex.B -> QuizPresenter.AnswerIndex.B
        | AnswerIndex.C -> QuizPresenter.AnswerIndex.C
        | AnswerIndex.D -> QuizPresenter.AnswerIndex.D
        // TODO error handling
        | _ -> invalidArg "index" ""

type Lifeline(inner) =
    member x._Internal = inner

    member x.Name =
        x._Internal.name
    member x.Type =
        x._Internal.``type``

module private ConvertLifeline =
    let convert lifeline =
        new Lifeline(lifeline)

    let deconvert (lifeline :Lifeline) =
        lifeline._Internal

type LifelineInfo(inner) =
    member x._Internal = inner

    member x.Lifeline =
        new Lifeline(x._Internal.lifeline)
    member x.Used =
        x._Internal.used

type Question(inner) =
    let inner = inner

    member x.Question =
        let (QuestionString q) = inner.question
        q
    member x.AnswerA =
        let (AnswerString a,_,_,_) = inner.answers
        a
    member x.AnswerB =
        let (_,AnswerString b,_,_) = inner.answers
        b
    member x.AnswerC =
        let (_,_,AnswerString c,_) = inner.answers
        c
    member x.AnswerD =
        let (_,_,_,AnswerString d) = inner.answers
        d

    member x.CheckAnswer answer =
        ConvertAnswerIndex.deconvert answer
        |> checkAnswer inner
    member x.HasResult =
        not (inner.result = Result None)
    member x.Result =
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
    let mutable inner = inner
    let random = new Random()

    member this.Parties =
        inner.parties
        |> List.map (fun party -> new Party(party))
        |> List.toArray

    member this.ActiveParty =
        new Party(getActiveParty inner)
    member this.ChooseAnswer index =
        inner <-
            ConvertAnswerIndex.deconvert index
            |> chooseAnswer inner
    member this.CurrentQuestion =
        match getNextQuestionFromQuiz inner with
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
    member this.UseLifeline lifeline =
        let lifeline' = ConvertLifeline.deconvert lifeline
        let activeParty = getActiveParty inner
        let party' = useLifeline lifeline' activeParty random
        inner <- {inner with parties = updateListFirst inner.parties activeParty party'}

    static member FromFile path =
        new Quiz(QuizFromFile.initQuizFromFile path)