namespace QuizPresentator

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
    member x.``Test that question is correctly updated``() =
        let property party question question' =
            let party' = addQuestion party question
            let party'' = updateQuestion party' question question'
            party'' .=. addQuestion party question'

        Check.QuickThrowOnFailure property
