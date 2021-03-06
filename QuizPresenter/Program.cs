﻿using System;
using Xwt;

namespace QuizPresenter {
	class MainClass {
		static Quiz quiz;
		static QuestionBox questionBox;
		static ResultBoxes resultBoxes;
		static State state = State.START;
		static AnswerIndex choosenAnswer;

		static int question = 0;

		enum State {
			// TODO check if this state is necessary or if it could be removed. Result would be the new starting state
			START,
			WAIT_FOR_ANSWER,
			LOGGED_IN,
			RESULT,
			END
		}

		static void useLifeline(int index) {
			quiz.UseLifeline(quiz.Lifelines[index]);
			resultBoxes.Update(quiz);
			questionBox.Update(quiz);
		}

		[STAThread]
		static void Main(string[] args)
		{
			// Initialize Quiz
			quiz = Quiz.FromFile(Parameter.QuizFilePath);

			string title = "Quizpresenter v0.1";
			// Init Gui
			Window mainWindow;
			PlatformID id = Environment.OSVersion.Platform;
			switch (id) {
				case PlatformID.Win32NT:
					Application.Initialize(ToolkitType.Wpf);
					mainWindow = new Window()
					{
						Title = title,

						Decorated = false,
						FullScreen = true
					};
					break;
				// HACK Whyever Mac seems to be Win32S here
				//case PlatformID.Win32S:
				case PlatformID.MacOSX:
					Application.Initialize(ToolkitType.Gtk);
					mainWindow = new Window()
					{
						Title = title,

						Decorated = true,
						Width = 800,
						Height = 600
					};
					break;
				//case PlatformID.Unix:
				default:
					Application.Initialize(ToolkitType.Gtk);
					mainWindow = new Window()
					{
						Title = title,

						Decorated = false,
						FullScreen = true
					};
					break;
			}

			// Initialize question screen
			Box outerContainer = new VBox();
			mainWindow.Content = outerContainer;
			outerContainer.HorizontalPlacement = WidgetPlacement.Fill;

			// Upper half
			Box upperHalf = new HBox();
			outerContainer.PackStart(upperHalf, true, true);
			upperHalf.VerticalPlacement = WidgetPlacement.Fill;

			// Imageview
			ImageCanvas imageCanvas = new ImageCanvas(null);
			upperHalf.PackStart(imageCanvas, true);

			// ResultBoxes
			resultBoxes = new ResultBoxes(quiz);
			upperHalf.PackEnd(resultBoxes);

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
							case Key.K1:
							case Key.NumPad1:
								choosenAnswer = AnswerIndex.A;
								questionBox.LogIn(choosenAnswer);
								state = State.LOGGED_IN;
								break;
							case Key.K2:
							case Key.NumPad2:
								choosenAnswer = AnswerIndex.B;
								questionBox.LogIn(choosenAnswer);
								state = State.LOGGED_IN;
								break;
							case Key.K3:
							case Key.NumPad3:
								choosenAnswer = AnswerIndex.C;
								questionBox.LogIn(choosenAnswer);
								state = State.LOGGED_IN;
								break;
							case Key.K4:
							case Key.NumPad4:
								choosenAnswer = AnswerIndex.D;
								questionBox.LogIn(choosenAnswer);
								state = State.LOGGED_IN;
								break;
							case Key.F1:
								useLifeline(0);
								break;
							case Key.F2:
								useLifeline(1);
								break;
							case Key.F3:
								useLifeline(2);
								break;
							case Key.F4:
								useLifeline(3);
								break;
						}
						break;
					case State.LOGGED_IN:
						// Update
						switch (e.Key) {
							case Key.Space:
								questionBox.ShowResult(choosenAnswer, quiz.CurrentQuestion.CheckAnswer(choosenAnswer));
								quiz.ChooseAnswer(choosenAnswer);
								state = State.RESULT;
								break;
							case Key.BackSpace:
							case Key.Delete:
								questionBox.LogOut();
								state = State.WAIT_FOR_ANSWER;
								break;
						}
						// Enable to change logged in answer
						goto case State.WAIT_FOR_ANSWER;
					case State.RESULT:
						// Update
						if (e.Key.Equals(Key.Space)) {
							resultBoxes.Update(quiz);
							questionBox.Update(quiz);
							imageCanvas.Update(++question);
							if (quiz.Ended) {
								state = State.END;
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