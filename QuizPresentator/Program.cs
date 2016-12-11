using System;
using System.IO;
using Xwt;
using Xwt.Drawing;

namespace QuizPresentator {
	class ImageCanvas : Canvas {
		private Image image;
		private Rectangle rect;

		public ImageCanvas(Image image) {
			this.image = image;

			BoundsChanged += (sender, e) => {
				Size size = image.Size;
				if (size.Width/size.Height < Size.Width/Size.Height) {
					// Image is higher
					int width = (int) (size.Width * (Size.Height / size.Height));
					rect = new Rectangle((Size.Width - width) / 2, 0, width, Size.Height);
				} else if (size.Width / size.Height > Size.Width / Size.Height) {
					int height = (int) (size.Height * (Size.Width / size.Width));
					rect = new Rectangle(0, (Size.Height - height) / 2, Size.Width, height);
				} else {
					rect = new Rectangle(0, 0, Size.Width, Size.Height);
				}
				QueueDraw();
			};
		}

		public void Update(int questionIndex) {
			string fileName = "images/" + questionIndex;
			if (File.Exists(fileName + ".jpg")) {
				image = Image.FromFile(fileName + ".jpg");
			} else if (File.Exists(fileName + ".png")) {
				image = Image.FromFile(fileName + ".png");
			} else {
				image = null;
			}

			OnBoundsChanged();
			QueueDraw();
		}

		protected override void OnDraw(Context ctx, Rectangle dirtyRect) {
			ctx.MoveTo(Parameter.BorderRadius, 0);
			ctx.LineTo(0, Parameter.BorderRadius);
			ctx.LineTo(0, Size.Height - Parameter.BorderRadius);
			ctx.LineTo(Parameter.BorderRadius, Size.Height);
			ctx.LineTo(Size.Width - Parameter.BorderRadius, Size.Height);
			ctx.LineTo(Size.Width, Size.Height - Parameter.BorderRadius);
			ctx.LineTo(Size.Width, Parameter.BorderRadius);
			ctx.LineTo(Size.Width - Parameter.BorderRadius, 0);
			ctx.ClosePath();
			ctx.SetColor(Colors.LightGray);
			ctx.Fill();

			if (image != null)
				ctx.DrawImage(image, rect);
		}
	}

	class MainClass {
		private static Logic.Quiz quiz;
		private static QuestionBox questionBox;
		private static ResultBoxes resultBoxes;
		private static State state = State.START;
		private static Logic.AnswerIndex choosenAnswer;

		private static int question = 0;

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
			outerContainer.PackStart(upperHalf, true, true);
			upperHalf.VerticalPlacement = WidgetPlacement.Fill;

			// Imageview
			ImageCanvas imageCanvas = new ImageCanvas(Image.FromFile("images/example.png"));
			upperHalf.PackStart(imageCanvas, true);

			// ResultBoxes
			resultBoxes = new ResultBoxes(quiz.Size, quiz.NrOfParties);
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
							case Key.F1:
								choosenAnswer = Logic.AnswerIndex.A;
								questionBox.LogIn(choosenAnswer);
								state = State.LOGGED_IN;
								break;
							case Key.K2:
							case Key.NumPad2:
							case Key.F2:
								choosenAnswer = Logic.AnswerIndex.B;
								questionBox.LogIn(choosenAnswer);
								state = State.LOGGED_IN;
								break;
							case Key.K3:
							case Key.NumPad3:
							case Key.F3:
								choosenAnswer = Logic.AnswerIndex.C;
								questionBox.LogIn(choosenAnswer);
								state = State.LOGGED_IN;
								break;
							case Key.K4:
							case Key.NumPad4:
							case Key.F4:
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
							resultBoxes.Update(quiz);
							questionBox.Update(quiz);
							imageCanvas.Update(++question);
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