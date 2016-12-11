using Xwt;
using Xwt.Drawing;
using System;

namespace QuizPresentator {
	/// <summary>
	/// Box, which is used to display the question or an answer
	/// </summary>
	public class QuestionComponentBox : Canvas {
		Label label = new Label();
		Color borderColor, backgroundColor;

		public QuestionComponentBox() {
			this.BoundsChanged += (sender, e) => {
				var prefHeight = (int) label.Surface.GetPreferredSize(Math.Max(Size.Width - 2 * Parameter.BorderRadius, 1), SizeConstraint.Unconstrained).Height;
				HeightRequest = prefHeight + 2*Parameter.BorderRadius - 5;
				SetChildBounds(label, new Rectangle(Parameter.BorderRadius, Parameter.BorderRadius, Size.Width - 2 * Parameter.BorderRadius, prefHeight));
				QueueDraw();
			};

			label.Font = label.Font.WithSize(Parameter.FontSizeQuestion);
			label.Wrap = WrapMode.Word;
			AddChild(label, Parameter.BorderRadius, Parameter.BorderRadius);
			OnBoundsChanged();
			ResetColors();
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
			ctx.SetColor(backgroundColor);
			ctx.FillPreserve();
			ctx.SetLineWidth(Parameter.BorderWidth);
			ctx.SetColor(borderColor);
			ctx.Stroke();
		}

		public void SetText(string text) {
			label.Text = text;
			OnBoundsChanged();
			QueueDraw();
		}

		public void Correct() {
			borderColor = Parameter.CorrectAnswerBorderColor;
			backgroundColor = Parameter.CorrectAnswerColor;
			QueueDraw();
		}

		public void Wrong() {
			borderColor = Parameter.WrongAnswerBorderColor;
			backgroundColor = Parameter.WrongAnswerColor;
			QueueDraw();
		}

		public void LogIn() {
			borderColor = Parameter.LogInBorderColor;
			backgroundColor = Parameter.LogInColor;
			QueueDraw();
		}

		public void ResetColors() {
			borderColor = Parameter.BoxBorderColor;
			backgroundColor = Parameter.BoxBackgroundColor;
			QueueDraw();
		}
	}
}