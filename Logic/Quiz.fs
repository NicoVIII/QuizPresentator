namespace QuizPresentator

module QuizPresentator =
    type QuestionString = QuestionString of string
    type AnswerString = AnswerString of string
    type AnswerIndex = | A | B | C | D
    type Result = Result of bool option
    type Question = {question: QuestionString; answers: AnswerString * AnswerString * AnswerString * AnswerString; correct: AnswerIndex; result: Result}

    type LLType = JustImageLL | FiftyFiftyLL
    type Lifeline = {name: string; ``type``: LLType}
    type LifelineInfo = {lifeline: Lifeline; used: bool}

    type Party = {active: bool; questions: Question list; lifelineInfos: LifelineInfo list}
    type Quiz = {parties: Party list; lifelines: Lifeline list}

    let getActiveParty {parties = parties} =
        let filtered = List.filter (fun party -> party.active) parties
        match filtered with
        | [activeParty] -> activeParty
        | _ -> failwith "LogicError! Invalid amount of active Parties inside of quiz."

    let getNextQuestionFromParty party =
        let {questions = questions} = party
        let fold res question =
            let {result = Result result} = question
            match res with
            | Some q -> Some q
            | None ->
                match result with
                | None -> Some question
                | Some _ -> None
        List.fold fold None questions

    let getNextQuestionFromQuiz =
        getActiveParty
        >> getNextQuestionFromParty

    let isEnded quiz =
        let {parties = parties} = quiz
        let fold ended party =
            if ended = true then
                match getNextQuestionFromParty party with
                | None -> true
                | Some _ -> false
            else
                ended
        List.fold fold true parties

    // Flow
    let checkAnswer question answer =
        let {correct = correct} = question
        answer = correct

    let updateResult question result =
        {question with result = Result (Some result)}

    let updateQuestion party question question'=
        let {questions = questions} = party
        let questions' = updateList questions question question'
        {party with questions = questions'}

    let updateParty quiz party party' =
        let {parties = parties} = quiz
        let parties' = updateList parties party party'
        {quiz with parties = parties'}

    let tryUpdateResultInQuiz result quiz =
        let party = getActiveParty quiz
        let question = getNextQuestionFromParty party
        match question with
        | None -> None
        | Some question -> 
            updateResult question result
            |> updateQuestion party question
            |> updateParty quiz party
            |> Some

    let updateResultInQuiz result quiz =
        match tryUpdateResultInQuiz result quiz with
        | None -> invalidOp "No result can be updated"
        | Some quiz -> quiz

    let tryChooseAnswer quiz answer =
        match getNextQuestionFromQuiz quiz with
        | None ->
            None
        | Some question ->
            let result = checkAnswer question answer
            tryUpdateResultInQuiz result quiz

    let activateNextParty quiz =
        let fold (out, state) party =
            if party.active then
                ({party with active = false}::out, true)
            else
                if state = true then
                    ({party with active = true}::out, false)
                else
                    (party::out, state)

        let (list, res) = List.fold fold ([], false) quiz.parties
        if res then
            let (list, _) = List.fold fold ([], true) list
            {quiz with parties = List.rev list}
        else
            {quiz with parties = List.rev list}

    let chooseAnswer quiz answer =
        match getNextQuestionFromQuiz quiz with
        | None ->
            invalidOp "No question to choose an answer for left."
        | Some question ->
            let result = checkAnswer question answer
            updateResultInQuiz result quiz
            |> activateNextParty

    module Creation =
        let lifeline name ``type`` = {name = name; ``type`` = ``type``}
        let lifelineInfo lifeline used = {lifeline = lifeline; used = false}
        let question question correct answerA answerB answerC answerD = {question = QuestionString question; answers = (AnswerString answerA, AnswerString answerB, AnswerString answerC, AnswerString answerD); correct = correct; result = Result None}
        let emptyParty active =
            {questions = []; lifelineInfos = []; active = active}
        let rec emptyQuiz parties =
            match parties with
            | 1 ->
                {parties = [emptyParty true]; lifelines = []}
            | i when i > 1 ->
                let {parties = parties} = emptyQuiz (i-1)
                let parties' = List.append parties [emptyParty false]
                {parties = parties'; lifelines = []}
            | _ ->
                invalidArg "parties" "Invalid partysize"