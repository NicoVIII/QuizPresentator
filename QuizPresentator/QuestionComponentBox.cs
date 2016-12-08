using Xwt;
using Xwt.Drawing;

namespace QuizPresentation {
	/// <summary>
	/// Box, which is used to display the question or an answer
	/// </summary>
	public class QuestionComponentBox : HBox {
		private Label label = new Label();
		private HBox hBox = new HBox();

		public QuestionComponentBox() {
			this.BackgroundColor = Parameter.BoxBorderColor;
			hBox.BackgroundColor = Parameter.BoxBackgroundColor;
			hBox.Margin = new WidgetSpacing(3, 3, 3, 3);

			// Init Label
			label.Margin = new WidgetSpacing(12, 12, 12, 12);
			label.Font = label.Font.WithSize(Parameter.FontSize);
			label.Wrap = WrapMode.Word;

			this.PackStart(hBox, true);
			hBox.PackStart(label, true);
		}

		public void SetBorder(int border) {
			int margin = 15;
			hBox.Margin = new WidgetSpacing(border, border, border, border);
			label.Margin = new WidgetSpacing(margin - border, margin - border, margin - border, margin - border);
		}

		public void SetText(string text) {
			label.Text = text;
		}
	}
}