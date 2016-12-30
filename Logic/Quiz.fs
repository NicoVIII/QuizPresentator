namespace QuizPresenter

open System

module QuizPresenter =
    type QuestionString = QuestionString of string
    type AnswerString = AnswerString of string
    type AnswerIndex = A | B | C | D
    type Result = Result of bool option
    type Question = {question: QuestionString; answers: AnswerString * AnswerString * AnswerString * AnswerString; correct: AnswerIndex; result: Result}

    type LLType = JustImageLL | FiftyFiftyLL
    type Lifeline = {name: string; ``type``: LLType}
    type LifelineInfo = {lifeline: Lifeline; used: bool}

    type Party = {active: bool; questions: Question list; lifelineInfos: LifelineInfo list}
    type Quiz = {parties: Party list; lifelines: Lifeline list}

    let nextAnswerIndex index =
        match index with
        | A -> B
        | B -> C
        | C -> D
        | D -> A

    let rec nextAnswerIndexN offset index =
        if offset = 1 then
            nextAnswerIndex index
        else
            nextAnswerIndex index
            |> nextAnswerIndexN (offset-1)

    let rec nextAnswerIndexNExcept offset skip index =
        if offset = 1 then
            let i = nextAnswerIndex index
            if i = skip then
                nextAnswerIndexNExcept offset skip i
            else
                i
        else
            let i = nextAnswerIndex index
            if i = skip then
                nextAnswerIndexNExcept offset skip i
            else
                nextAnswerIndexNExcept (offset-1) skip index

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

    let checkAnswer question answer =
        let {correct = correct} = question
        answer = correct

    let updateResult question result =
        {question with result = Result (Some result)}

    let updateQuestion party question question'=
        let {questions = questions} = party
        let questions' = updateListFirst questions question question'
        {party with questions = questions'}

    let updateParty quiz party party' =
        let {parties = parties} = quiz
        let parties' = updateListFirst parties party party'
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
            match List.rev list with
            | party::rest -> {quiz with parties = {party with active = true}::rest}
            // TODO error handling
            | [] -> invalidOp ""
        else
            {quiz with parties = List.rev list}

    let deleteAnswer question index =
        let {answers = (a, b, c, d)} = question
        match index with
        | A -> {question with answers = (AnswerString "", b, c, d)}
        | B -> {question with answers = (a, AnswerString "", c, d)}
        | C -> {question with answers = (a, b, AnswerString "", d)}
        | D -> {question with answers = (a, b, c, AnswerString "")}

    // TODO implement try-version of this
    let setLifelineUsed lifeline party =
        let infos = party.lifelineInfos
        let fold info lifelineInfo =
            match info with
            | Some _ -> info
            | None ->
                if lifelineInfo.lifeline = lifeline && lifelineInfo.used = false then
                    Some lifelineInfo
                else
                    None
        match List.fold fold None infos with
        // TODO decide if you want to throw an error or not
        | None -> party//invalidOp "This lifeline cannot be used, party has none of them left"
        | Some info ->
            let infos' = updateListFirst infos info {info with used = true}
            {party with lifelineInfos = infos'}

    let useLifeline lifeline party (random :Random) =
        match lifeline.``type`` with
        | FiftyFiftyLL ->
            let question = getNextQuestionFromParty party
            let stay = random.Next(1, 4)
            let stay' = nextAnswerIndexNExcept stay question.Value.correct D
            let question' = List.fold (fun q i -> if i = stay' || i = question.Value.correct then q else deleteAnswer q i) question.Value [A; B; C; D]
            updateQuestion party question.Value question'
        | JustImageLL -> // Nothing to do
        setLifelineUsed lifeline party

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
        let lifelineInfo lifeline = {lifeline = lifeline; used = false}
        let question question correct answerA answerB answerC answerD = {question = QuestionString question; answers = (AnswerString answerA, AnswerString answerB, AnswerString answerC, AnswerString answerD); correct = correct; result = Result None}
        let addQuestion party question =
            let {questions = questions} = party
            {party with questions = questions @ [question]}
        let addLifeline lifeline quiz =
            let {parties = parties; lifelines = lifelines} = quiz
            let map party =
                let {lifelineInfos = info} = party
                let lifelineInfo = lifelineInfo lifeline
                {party with lifelineInfos = info @ [lifelineInfo]}
            let parties' = List.map map parties
            {quiz with parties = parties'; lifelines = lifelines @ [lifeline]}
        let addLifelines lifelines quiz =
            List.fold (fun quiz' ll -> addLifeline ll quiz') quiz lifelines
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