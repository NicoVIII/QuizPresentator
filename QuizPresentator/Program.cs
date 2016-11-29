using System;
using Xwt;

namespace QuizPresentation {
	class MainClass {
		private static Logic.Quiz quiz;
		private static QuestionBox questionBox;
		private static ResultBox resultBox;
		private static State state = State.START;
		private static Logic.AnswerIndex choosenAnswer;

		private enum State {
			// TODO check if this state is necessary or if it could be removed. Result would be the new starting state
			START,
			WAIT_FOR_ANSWER,
			LOGGED_IN,
			RESULT,
			END
		}

		[STAThread]
		static void Main(string[] args)
		{
			// Initialize Quiz
			quiz = Logic.initQuizFromFile(Parameter.QuizFilePath);

			// Init Gui
			#if WIN
			Application.Initialize(ToolkitType.Wpf);
			var mainWindow = new Window()
			{
				Title = "Quizpresentation",

				Decorated = false,
				FullScreen = true
			};
			#elif MAC
			Application.Initialize(ToolkitType.Gtk);
			var mainWindow = new Window()
			{
				Title = "Quizpresentation",

				Width = 800,
				Height = 600
			};
			#else
			Application.Initialize(ToolkitType.Gtk);
			var mainWindow = new Window()
			{
				Title = "Quizpresentation",

				Decorated = false,
				FullScreen = true
			};
			#endif

			// Initialize question screen
			Box outerContainer = new VBox();
			mainWindow.Content = outerContainer;
			outerContainer.HorizontalPlacement = WidgetPlacement.Fill;

			// Upper half
			Box upperHalf = new HBox();
			outerContainer.PackStart(upperHalf, expand: true, fill: true);
			upperHalf.VerticalPlacement = WidgetPlacement.Fill;

			//upperHalf.PackStart(new Label("Picture"));
			resultBox = new ResultBox(quiz.Size, quiz.NrOfParties);
			upperHalf.PackEnd(resultBox);

			// Lower half
			questionBox = new QuestionBox();
			outerContainer.PackEnd(questionBox);

			// Initialize keyboard listener
			mainWindow.Content.KeyPressed += (sender, e) => {
				// Enable Escape Key
				if (e.Key == Xwt.Key.Escape) {
					Application.Exit();
				}

				switch (state) {
					case State.START:
						// Does the same as result atm
						goto case State.RESULT;
					case State.WAIT_FOR_ANSWER:
						switch (e.Key) {
							case Xwt.Key.K1:
							case Xwt.Key.NumPad1:
							case Xwt.Key.F1:
								choosenAnswer = Logic.AnswerIndex.A;
								questionBox.LogIn(choosenAnswer);
								state = State.LOGGED_IN;
								break;
							case Xwt.Key.K2:
							case Xwt.Key.NumPad2:
							case Xwt.Key.F2:
								choosenAnswer = Logic.AnswerIndex.B;
								questionBox.LogIn(choosenAnswer);
								state = State.LOGGED_IN;
								break;
							case Xwt.Key.K3:
							case Xwt.Key.NumPad3:
							case Xwt.Key.F3:
								choosenAnswer = Logic.AnswerIndex.C;
								questionBox.LogIn(choosenAnswer);
								state = State.LOGGED_IN;
								break;
							case Xwt.Key.K4:
							case Xwt.Key.NumPad4:
							case Xwt.Key.F4:
								choosenAnswer = Logic.AnswerIndex.D;
								questionBox.LogIn(choosenAnswer);
								state = State.LOGGED_IN;
								break;
						}
						break;
					case State.LOGGED_IN:
						// Update
						if (e.Key.Equals(Xwt.Key.Space)) {
							questionBox.ShowResult(choosenAnswer, quiz.CheckAnswer(choosenAnswer));
							quiz = quiz.ChooseAnswer(choosenAnswer);
							choosenAnswer = null;
							state = State.RESULT;
						}
						// Enable to change logged in answer
						goto case State.WAIT_FOR_ANSWER;
					case State.RESULT:
						// Update
						if (e.Key.Equals(Xwt.Key.Space)) {
							resultBox.Update(quiz);
							questionBox.Update(quiz);
							if (quiz.Ended) {
								state = State.END;
								questionBox.Hide();
							} else
								state = State.WAIT_FOR_ANSWER;
						}
						break;
					case State.END:
						// Nothing to do
						break;
				}
			};
			mainWindow.Content.CanGetFocus = true;
			mainWindow.Content.SetFocus();
			      
			// Start Application
			mainWindow.Show();
			Application.Run();
			mainWindow.Dispose();
		}
	}
}