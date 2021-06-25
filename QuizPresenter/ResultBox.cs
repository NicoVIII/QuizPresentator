using Gtk;
using System;

namespace QuizPresenter {
	public class ResultBox : Canvas {
		// TODO think about using quiz locally
		private VBox box;
		private Label[] labels;
		private int nrOfQuestions;
		private readonly static string PointsPattern = "{0} Punkte";

		public ResultBox(Party party) {
			nrOfQuestions = party.Questions.Length;
			Margin = new WidgetSpacing(3, 3, 3, 3);
			MinHeight = 2 * Parameter.BorderRadius;

			// Init Box
			box = new VBox();
			box.BackgroundColor = Parameter.BoxBackgroundColor;

			// Init Labels
			labels = new Label[nrOfQuestions+1];
			// Question Labels
			for (int i = 0; i < nrOfQuestions; i++) {
				Label l = new Label("Frage " + (i+1));
				l.Margin = new WidgetSpacing(top: i > 0?10:0);
				l.Font = l.Font.WithSize(Parameter.FontSizeResult);

				box.PackStart(l);
				labels[i] = l;
			}

			// Result Labels
			Label label = new Label(String.Format(PointsPattern, 0));
			label.Margin = new WidgetSpacing(top: 20);
			label.Font = label.Font.WithWeight(FontWeight.Bold).WithSize(Parameter.FontSizeResult);
			box.PackStart(label);
			labels[nrOfQuestions] = label;

			AddChild(box, new Rectangle(Parameter.BorderRadius, Parameter.BorderRadius, box.Surface.GetPreferredSize().Width, Math.Min(box.Surface.GetPreferredSize().Height - 2 * Parameter.BorderRadius, Size.Height)));
			WidthRequest = box.Surface.GetPreferredSize().Width + 2 * Parameter.BorderRadius;
			QueueDraw();

			BoundsChanged += (sender, e) => {
				SetChildBounds(box, new Rectangle(Parameter.BorderRadius, Parameter.BorderRadius, box.Surface.GetPreferredSize().Width, Math.Min(box.Surface.GetPreferredSize().Height, Size.Height - 2 * Parameter.BorderRadius)));
				WidthRequest = box.Surface.GetPreferredSize().Width + 2 * Parameter.BorderRadius;
				QueueDraw();
			};
		}

		protected override void OnDraw(Context ctx, Rectangle dirtyRect) {
			ctx.MoveTo(Parameter.BorderRadius, Parameter.BorderWidth / 2);
			ctx.LineTo(Parameter.BorderWidth / 2, Parameter.BorderRadius);
			ctx.LineTo(Parameter.BorderWidth / 2, Size.Height - Parameter.BorderRadius);
			ctx.LineTo(Parameter.BorderRadius, Size.Height - (Parameter.BorderWidth / 2));
			ctx.LineTo(Size.Width - Parameter.BorderRadius, Size.Height - (Parameter.BorderWidth / 2));
			ctx.LineTo(Size.Width - (Parameter.BorderWidth / 2), Size.Height - Parameter.BorderRadius);
			ctx.LineTo(Size.Width - (Parameter.BorderWidth / 2), Parameter.BorderRadius);
			ctx.LineTo(Size.Width - Parameter.BorderRadius, Parameter.BorderWidth / 2);
			ctx.ClosePath();
			ctx.SetColor(Parameter.BoxBackgroundColor);
			ctx.FillPreserve();
			ctx.SetLineWidth(Parameter.BorderWidth);
			ctx.SetColor(Parameter.BoxBorderColor);
			ctx.Stroke();
		}

		public void Update(Party party) {
			// Color question labels
			// TODO maybe move active attribute also into question model?
			bool markedActiveQuestion = false;
			Question[] questions = party.Questions;
			for (int i = 0; i < questions.Length; i++) {
				Question question = questions[i];
				if (question.HasResult) {
					if (question.Result) {
						labels[i].TextColor = Colors.DarkGreen;
					} else {
						labels[i].TextColor = Colors.DarkRed;
					}
				} else {
					if (party.Active && !markedActiveQuestion) {
						labels[i].TextColor = Colors.DarkOrange;
						markedActiveQuestion = true;
					} else {
						labels[i].TextColor = Colors.Black;
					}
				}
			}

			// Fill result
			labels[nrOfQuestions].Text = string.Format(PointsPattern, party.Points);

			QueueDraw();
		}
	}
}
