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
		int border = 4;

		public QuestionComponentBox() {
			this.BoundsChanged += (sender, e) => {
				label.WidthRequest = Math.Max(Size.Width - label.MarginLeft - label.MarginRight, 1);
				HeightRequest = label.Size.Height + label.MarginTop + label.MarginBottom;
				label.MarginLeft = HeightRequest / 2;
				label.MarginRight = HeightRequest / 2;
				Clear();
				AddChild(label, label.MarginLeft, label.MarginTop);
				QueueDraw();
			};

			label.Font = label.Font.WithSize(Parameter.FontSizeQuestion);
			label.Wrap = WrapMode.Word;
			label.Margin = new WidgetSpacing(left: 20, right: 20, top: 15, bottom: 15);
			label.MinHeight = label.Font.Size;
			OnBoundsChanged();
			ResetBorderColor();

			SetText("Hey");
		}

		protected override void OnDraw(Context ctx, Rectangle dirtyRect) {
			ctx.MoveTo(Size.Height / 2, border / 2 + 1 );
			ctx.LineTo(border / 2 + 1, Size.Height / 2);
			ctx.LineTo(Size.Height / 2, Size.Height - (border / 2 + 1));
			ctx.LineTo(Size.Width - (Size.Height / 2), Size.Height - (border / 2 + 1));
			ctx.LineTo(Size.Width - (border / 2 + 1), Size.Height / 2);
			ctx.LineTo(Size.Width - (Size.Height / 2), (border / 2 + 1));
			ctx.SetColor(Parameter.BoxBackgroundColor);
			ctx.FillPreserve();
			ctx.SetColor(borderColor);
			ctx.ClosePath();
			ctx.SetLineWidth(border);
			ctx.Stroke();
		}

		public void SetText(string text) {
			label.Text = text;
			this.OnBoundsChanged();
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