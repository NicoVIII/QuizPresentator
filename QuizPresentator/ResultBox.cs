using System;
using System.Collections.Generic;
using Xwt;
using Xwt.Drawing;

namespace QuizPresentator {
	public class ResultBox : Canvas {
		// TODO think about using quiz locally
		private VBox box;
		private Label[] labels;
		private int nrOfQuestions;
		private readonly static string PointsPattern = "{0} Punkte";

		public ResultBox(int nrOfQuestions) {
			this.nrOfQuestions = nrOfQuestions;
			this.Margin = new WidgetSpacing(3, 3, 3, 3);
			this.MinHeight = 2 * Parameter.BorderRadius;

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

		public void Update(List<bool> resultList, int resultOfParty, bool act) {
			// Color question labels
			for (int i = 0; i < labels.Length-1; i++) {
				// Already answered questions
				if (i < resultList.Count) {
					if (resultList[i]) {
						labels[i].TextColor = Colors.DarkGreen;
					} else {
						labels[i].TextColor = Colors.DarkRed;
					}
				}
				// Current question
				else if (i == resultList.Count && act) {
					labels[i].TextColor = Colors.DarkOrange;
				}
				// Coming questions
				else {
					labels[i].TextColor = Colors.Black;
				}
			}

			// Fill result
			labels[nrOfQuestions].Text = string.Format(PointsPattern, resultOfParty);

			QueueDraw();
		}
	}
}
