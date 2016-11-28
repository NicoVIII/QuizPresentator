using System;
using System.Diagnostics;
using Xwt;

namespace QuizPresentation {
	public class ResultBox : HBox {
		// TODO think about using quiz locally to avoid assertion in Update
		private VBox[] boxes;
		private Label[] labels;

		public ResultBox(int nrOfQuestions, int nrOfParties) {
			// Init Boxes
			boxes = new VBox[nrOfParties];
			for (int i = 0; i < nrOfParties; i++) {
				VBox box = new VBox();
				box.BackgroundColor = Parameter.BoxBackgroundColor;
				PackStart(box);
				boxes[i] = box;
			}

			// Init Labels
			labels = new Label[nrOfQuestions];
			for (int i = 0; i < nrOfQuestions; i++) {
				Label label = new Label("Frage " + (Math.Floor((double) i/nrOfParties) + 1));
				label.Margin = new WidgetSpacing(top: 15, bottom: 15, left: 20, right: 20);
				label.Font = label.Font.WithSize(Parameter.FontSize);

				boxes[i % nrOfParties].PackStart(label);
				labels[i] = label;
			}
		}

		public void Update(Logic.Quiz quiz) {
			Debug.Assert(quiz.Size == labels.Length);

			// Color question labels
			for (int i = 0; i < quiz.Size; i++) {
				// Already answered questions
				if (i < quiz.Results.Length) {
					if (quiz.Results[i]) {
						labels[i].TextColor = Xwt.Drawing.Colors.DarkGreen;
					} else {
						labels[i].TextColor = Xwt.Drawing.Colors.DarkRed;
					}
				}
				// Current question
				else if (i == quiz.Results.Length) {
					labels[i].TextColor = Xwt.Drawing.Colors.DarkOrange;
				}
				// Coming questions
				else {
					labels[i].TextColor = Xwt.Drawing.Colors.Black;
				}
			}
		}
	}
}
