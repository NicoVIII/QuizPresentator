using Xwt;

namespace QuizPresentation {
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
			answerBoxA.BackgroundColor = Parameter.BoxBackgroundColor;
			answerBoxB.BackgroundColor = Parameter.BoxBackgroundColor;
			answerBoxC.BackgroundColor = Parameter.BoxBackgroundColor;
			answerBoxD.BackgroundColor = Parameter.BoxBackgroundColor;

			// Update question and answer texts
			questionBox.SetText(quiz.Question);
			answerBoxA.SetText("1: "+quiz.AnswerA);
			answerBoxB.SetText("2: "+quiz.AnswerB);
			answerBoxC.SetText("3: "+quiz.AnswerC);
			answerBoxD.SetText("4: "+quiz.AnswerD);
		}

		public void LogIn(Logic.AnswerIndex index) {
			if (index.Equals(Logic.AnswerIndex.A)) {
				answerBoxA.BackgroundColor = Parameter.LogInColor;
				answerBoxB.BackgroundColor = Parameter.BoxBackgroundColor;
				answerBoxC.BackgroundColor = Parameter.BoxBackgroundColor;
				answerBoxD.BackgroundColor = Parameter.BoxBackgroundColor;
			} else if (index.Equals(Logic.AnswerIndex.B)) {
				answerBoxA.BackgroundColor = Parameter.BoxBackgroundColor;
				answerBoxB.BackgroundColor = Parameter.LogInColor;
				answerBoxC.BackgroundColor = Parameter.BoxBackgroundColor;
				answerBoxD.BackgroundColor = Parameter.BoxBackgroundColor;
			} else if(index.Equals(Logic.AnswerIndex.C)) {
				answerBoxA.BackgroundColor = Parameter.BoxBackgroundColor;
				answerBoxB.BackgroundColor = Parameter.BoxBackgroundColor;
				answerBoxC.BackgroundColor = Parameter.LogInColor;
				answerBoxD.BackgroundColor = Parameter.BoxBackgroundColor;
			} else if (index.Equals(Logic.AnswerIndex.D)) {
				answerBoxA.BackgroundColor = Parameter.BoxBackgroundColor;
				answerBoxB.BackgroundColor = Parameter.BoxBackgroundColor;
				answerBoxC.BackgroundColor = Parameter.BoxBackgroundColor;
				answerBoxD.BackgroundColor = Parameter.LogInColor;
			}
		}

		public void ShowResult(Logic.AnswerIndex index, bool result) {
			Xwt.Drawing.Color color = result ? Parameter.CorrectAnswerColor : Parameter.WrongAnswerColor;

			if (index.Equals(Logic.AnswerIndex.A)) {
				answerBoxA.BackgroundColor = color;
			} else if (index.Equals(Logic.AnswerIndex.B)) {
				answerBoxB.BackgroundColor = color;
			} else if (index.Equals(Logic.AnswerIndex.C)) {
				answerBoxC.BackgroundColor = color;
			} else if (index.Equals(Logic.AnswerIndex.D)) {
				answerBoxD.BackgroundColor = color;
			}
		}
	}
}