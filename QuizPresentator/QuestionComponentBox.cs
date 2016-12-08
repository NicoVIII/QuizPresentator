using Xwt;
using Xwt.Drawing;
using System;

namespace QuizPresentation {
	/// <summary>
	/// Box, which is used to display the question or an answer
	/// </summary>
	public class QuestionComponentBox : Canvas {
		Label label = new Label();
		Color borderColor;

		public QuestionComponentBox() {
			this.BoundsChanged += (sender, e) => {
				int prefHeight = (int) label.Surface.GetPreferredSize(Math.Max(Size.Width - 2 * Parameter.BorderRadius, 1), SizeConstraint.Unconstrained).Height;
				HeightRequest = prefHeight + 2*Parameter.BorderRadius - 5;
				SetChildBounds(label, new Rectangle(Parameter.BorderRadius, Parameter.BorderRadius, Size.Width - 2 * Parameter.BorderRadius, prefHeight));
				QueueDraw();
			};

			label.Font = label.Font.WithSize(Parameter.FontSizeQuestion);
			label.Wrap = WrapMode.Word;
			AddChild(label, Parameter.BorderRadius, Parameter.BorderRadius);
			OnBoundsChanged();
			ResetBorderColor();
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
			ctx.SetColor(borderColor);
			ctx.Stroke();
		}

		public void SetText(string text) {
			label.Text = text;
			OnBoundsChanged();
			QueueDraw();
		}

		public void SetBorderColor(Color color) {
			borderColor = color;
			QueueDraw();
		}

		public void ResetBorderColor() {
			borderColor = Parameter.BoxBorderColor;
			QueueDraw();
		}
	}
}