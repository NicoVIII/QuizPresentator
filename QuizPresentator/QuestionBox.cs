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
			answerBoxA.ResetBorderColor();
			answerBoxB.ResetBorderColor();
			answerBoxC.ResetBorderColor();
			answerBoxD.ResetBorderColor();

			// Update question and answer texts
			questionBox.SetText(quiz.Question);
			answerBoxA.SetText("A: "+quiz.AnswerA);
			answerBoxB.SetText("B: "+quiz.AnswerB);
			answerBoxC.SetText("C: "+quiz.AnswerC);
			answerBoxD.SetText("D: "+quiz.AnswerD);
		}

		public void LogIn(Logic.AnswerIndex index) {
			if (index.Equals(Logic.AnswerIndex.A)) {
				answerBoxA.SetBorderColor(Parameter.LogInColor);
				answerBoxB.ResetBorderColor();
				answerBoxC.ResetBorderColor();
				answerBoxD.ResetBorderColor();
			} else if (index.Equals(Logic.AnswerIndex.B)) {
				answerBoxA.ResetBorderColor();
				answerBoxB.SetBorderColor(Parameter.LogInColor);
				answerBoxC.ResetBorderColor();
				answerBoxD.ResetBorderColor();
			} else if(index.Equals(Logic.AnswerIndex.C)) {
				answerBoxA.ResetBorderColor();
				answerBoxB.ResetBorderColor();
				answerBoxC.SetBorderColor(Parameter.LogInColor);
				answerBoxD.ResetBorderColor();
			} else if (index.Equals(Logic.AnswerIndex.D)) {
				answerBoxA.ResetBorderColor();
				answerBoxB.ResetBorderColor();
				answerBoxC.ResetBorderColor();
				answerBoxD.SetBorderColor(Parameter.LogInColor);
			}
		}

		public void ShowResult(Logic.AnswerIndex index, bool result) {
			Xwt.Drawing.Color color = result ? Parameter.CorrectAnswerColor : Parameter.WrongAnswerColor;

			if (index.Equals(Logic.AnswerIndex.A)) {
				answerBoxA.SetBorderColor(color);
			} else if (index.Equals(Logic.AnswerIndex.B)) {
				answerBoxB.SetBorderColor(color);
			} else if (index.Equals(Logic.AnswerIndex.C)) {
				answerBoxC.SetBorderColor(color);
			} else if (index.Equals(Logic.AnswerIndex.D)) {
				answerBoxD.SetBorderColor(color);
			}
		}
	}
}