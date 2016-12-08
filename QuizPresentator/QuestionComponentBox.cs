using Xwt;
using Xwt.Drawing;

namespace QuizPresentation {
	/// <summary>
	/// Box, which is used to display the question or an answer
	/// </summary>
	public class QuestionComponentBox : Canvas {
		string text;

		public QuestionComponentBox() {
		}

		protected override void OnDraw(Context ctx, Rectangle dirtyRect) {
			ctx.Rectangle(new Rectangle(0, 0, dirtyRect.Height, dirtyRect.Height));
		}

		public void SetBorder(int border) {
			
		}

		public void SetText(string text) {
			this.text = text;
		}
	}
}