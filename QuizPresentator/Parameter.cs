namespace QuizPresentation {
	public sealed class Parameter {
		/// <summary>
		/// The color of the background of used boxes.
		/// </summary>
		public readonly static Xwt.Drawing.Color BoxBorderColor = Xwt.Drawing.Colors.DarkViolet;

		public readonly static Xwt.Drawing.Color BoxBackgroundColor = Xwt.Drawing.Colors.SkyBlue;

		public readonly static Xwt.Drawing.Color LogInColor = Xwt.Drawing.Colors.DarkOrange;

		public readonly static Xwt.Drawing.Color CorrectAnswerColor = Xwt.Drawing.Colors.DarkGreen;

		public readonly static Xwt.Drawing.Color WrongAnswerColor = Xwt.Drawing.Colors.Red;

		public readonly static int FontSize = 22;

		public readonly static string QuizFilePath = "quiz.txt";

		// Make constructor private to avoid instantiation
		private Parameter() {
			// Nothing to do
		}
	}
}