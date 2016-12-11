using Xwt;

namespace QuizPresentator {
	/// <summary>
	/// Box, which contains Question plus all four answers.
	/// </summary>
	public class QuestionBox : VBox {
		QuestionComponentBox questionBox, answerBoxA, answerBoxB, answerBoxC, answerBoxD;

		// TODO move Quiz to local variable
		public QuestionBox() : base() {
			// Questions Box
			questionBox = new QuestionComponentBox();
			this.PackStart(questionBox);

			// Surrounding Answers Box
			Table answersTable = new Table();
			this.PackStart(answersTable);

			answerBoxA = new QuestionComponentBox();
			answersTable.Add(answerBoxA, left: 0, top: 0, hexpand: true);
			answerBoxB = new QuestionComponentBox();
			answersTable.Add(answerBoxB, left: 1, top: 0, hexpand: true);

			answerBoxC = new QuestionComponentBox();
			answersTable.Add(answerBoxC, left: 0, top: 1, hexpand: true);
			answerBoxD = new QuestionComponentBox();
			answersTable.Add(answerBoxD, left: 1, top: 1, hexpand: true);
		}

		public void Update(Logic.Quiz quiz) {
			// Reset color
			answerBoxA.ResetColors();
			answerBoxB.ResetColors();
			answerBoxC.ResetColors();
			answerBoxD.ResetColors();

			// Update question and answer texts
			questionBox.SetText(quiz.Question);
			answerBoxA.SetText("A: "+quiz.AnswerA);
			answerBoxB.SetText("B: "+quiz.AnswerB);
			answerBoxC.SetText("C: "+quiz.AnswerC);
			answerBoxD.SetText("D: "+quiz.AnswerD);
		}

		public void LogIn(Logic.AnswerIndex index) {
			if (index.Equals(Logic.AnswerIndex.A)) {
				answerBoxA.LogIn();
				answerBoxB.ResetColors();
				answerBoxC.ResetColors();
				answerBoxD.ResetColors();
			} else if (index.Equals(Logic.AnswerIndex.B)) {
				answerBoxA.ResetColors();
				answerBoxB.LogIn();
				answerBoxC.ResetColors();
				answerBoxD.ResetColors();
			} else if(index.Equals(Logic.AnswerIndex.C)) {
				answerBoxA.ResetColors();
				answerBoxB.ResetColors();
				answerBoxC.LogIn();
				answerBoxD.ResetColors();
			} else if (index.Equals(Logic.AnswerIndex.D)) {
				answerBoxA.ResetColors();
				answerBoxB.ResetColors();
				answerBoxC.ResetColors();
				answerBoxD.LogIn();
			}
		}

		public void ShowResult(Logic.AnswerIndex index, bool result) {
			QuestionComponentBox box;

			if (index.Equals(Logic.AnswerIndex.A)) {
				box = answerBoxA;
			}
			else if (index.Equals(Logic.AnswerIndex.B)) {
				box = answerBoxB;
			}
			else if (index.Equals(Logic.AnswerIndex.C)) {
				box = answerBoxC;
			}
			else /*if (index.Equals(Logic.AnswerIndex.D)) */{
				box = answerBoxD;
			}

			if (result) {
				box.Correct();
			} else {
				box.Wrong();
			}
		}
	}
}