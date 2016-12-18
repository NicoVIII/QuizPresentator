namespace QuizPresentator

open Logic
open System
open NUnit.Framework

[<TestFixture>]
type Test() = 

    [<Test>]
    member x.CheckInit() =
        let quiz = addQuestionsFromLines 0 ["Dies ist eine Frage!;3;A;B;C;D"; "Frage 2;1;LA;LALA;LALALA;LALALALA"] (emptyQuiz 1)
        let {parties = parties; activeParty = active} = quiz
        // Check if active party is initially 0
        Assert.AreEqual(0, active)
        // Check if number of questions is correct
        List.iter (fun {questions = questions} -> Assert.True((List.length questions) = 2)) parties
        // Check if ended is correct
        Assert.IsFalse(isEnded quiz)

    [<Test>]
    member x.CheckMultiPartyInit() =
        let quiz = addQuestionsFromLines 0 ["Dies ist eine Frage!;3;A;B;C;D"; "Frage 2;1;LA;LALA;LALALA;LALALALA"] (emptyQuiz 2)
        let {parties = parties; activeParty = active} = quiz
        // Check if active party is initially 0
        Assert.AreEqual(0, active)
        // Check if number of questions is correct
        List.iter (fun {questions = questions} -> Assert.True((List.length questions) = 1)) parties
        // Check if ended is correct
        Assert.IsFalse(isEnded quiz)