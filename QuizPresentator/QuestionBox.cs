using QuizPresenter;
using System;
using Xwt;

namespace QuizPresenter {
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

		public void Update(Quiz quiz) {
			// Reset color
			answerBoxA.ResetColors();
			answerBoxB.ResetColors();
			answerBoxC.ResetColors();
			answerBoxD.ResetColors();

			// Update question and answer texts
			if (!quiz.Ended) {
				Question question = quiz.CurrentQuestion;
				questionBox.SetText(question.Question);
				answerBoxA.SetText("A: " + question.AnswerA);
				answerBoxB.SetText("B: " + question.AnswerB);
				answerBoxC.SetText("C: " + question.AnswerC);
				answerBoxD.SetText("D: " + question.AnswerD);
			} else {
				answerBoxA.Hide();
				answerBoxB.Hide();
				answerBoxC.Hide();
				answerBoxD.Hide();
				questionBox.label.Font = questionBox.label.Font.WithSize(10);
				questionBox.SetText("QuizPresenter v0.1 by NicoVIII (http://github.com/NicoVIII/QuizPresenter)");
			}
		}

		public void LogIn(AnswerIndex index) {
			if (index.Equals(AnswerIndex.A)) {
				answerBoxA.LogIn();
				answerBoxB.ResetColors();
				answerBoxC.ResetColors();
				answerBoxD.ResetColors();
			} else if (index.Equals(AnswerIndex.B)) {
				answerBoxA.ResetColors();
				answerBoxB.LogIn();
				answerBoxC.ResetColors();
				answerBoxD.ResetColors();
			} else if(index.Equals(AnswerIndex.C)) {
				answerBoxA.ResetColors();
				answerBoxB.ResetColors();
				answerBoxC.LogIn();
				answerBoxD.ResetColors();
			} else if (index.Equals(AnswerIndex.D)) {
				answerBoxA.ResetColors();
				answerBoxB.ResetColors();
				answerBoxC.ResetColors();
				answerBoxD.LogIn();
			}
		}

		public void LogOut() {
			answerBoxA.ResetColors();
			answerBoxB.ResetColors();
			answerBoxC.ResetColors();
			answerBoxD.ResetColors();
		}

		public void ShowResult(AnswerIndex index, bool result) {
			QuestionComponentBox box;

			if (index.Equals(AnswerIndex.A)) {
				box = answerBoxA;
			}
			else if (index.Equals(AnswerIndex.B)) {
				box = answerBoxB;
			}
			else if (index.Equals(AnswerIndex.C)) {
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