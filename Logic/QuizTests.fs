﻿namespace QuizPresentator

open QuizPresentator.QuizPresentator
open QuizPresentator.QuizPresentator.Creation
open QuizPresentator.QuizFromFile
open NUnit.Framework
open FsUnit
open FsCheck
open System

[<TestFixture>]
type Test() = 
    let (.=.) left right = left = right |@ sprintf "%A = %A" left right

    [<Test>]
    member x.UpdateResultTest() =
        let property question res =
            let question' = updateResult question res
            question' .=. {question with result = Result (Some res)}

        Check.QuickThrowOnFailure property

    [<Test>]
    member x.``Question is correctly updated``() =
        let property party question question' =
            let party' = addQuestion party question
            let party'' = updateQuestion party' question question'
            party'' .=. addQuestion party question'

        Check.QuickThrowOnFailure property

    [<Test>]
    member x.``Quiz state ended works``() =
        let property quiz =
            let fold2 ended question =
                ended && not (question.result = Result None)

            let fold1 ended party =
                List.fold fold2 ended party.questions
            let ended = List.fold fold1 true quiz.parties
            isEnded quiz .=. ended

        Check.QuickThrowOnFailure property

    [<Test>]
    member x.``Lifeline is used correctly``() =
        let property party lifeline =
            let {lifelineInfos = infos} = party
            let info = lifelineInfo lifeline
            let party' = {party with lifelineInfos = info::infos}
            let party'' = useLifeline lifeline party'
            party'' .=. {party with lifelineInfos = {info with used = true}::infos}

        Check.QuickThrowOnFailure property