using Xwt;

namespace QuizPresentation {
	/// <summary>
	/// Box, which is used to display the question or an answer
	/// </summary>
	public class QuestionComponentBox : HBox {
		private Label label = new Label();

		public QuestionComponentBox() {
			this.BackgroundColor = Parameter.BoxBackgroundColor;

			// Make all used boxes visible, even if the layout is not well configured
			this.MinHeight = 10;
			this.MinWidth = 10;

			// Init Label
			label.Margin = new WidgetSpacing(5, 5, 5, 5);
			// TODO does not work, maybe implement custom version
			//label.Wrap = WrapMode.Word;

			this.PackStart(label);
		}

		public void SetText(string text) {
			label.Text = text;
		}
	}
}