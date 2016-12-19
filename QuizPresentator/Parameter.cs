namespace QuizPresentator {
	public sealed class Parameter {
		/// <summary>
		/// The color of the background of used boxes.
		/// </summary>
		public readonly static Xwt.Drawing.Color BoxBackgroundColor = Xwt.Drawing.Colors.SkyBlue;
		public readonly static Xwt.Drawing.Color BoxBorderColor = Xwt.Drawing.Colors.DarkBlue;

		public readonly static Xwt.Drawing.Color WrongAnswerColor = Xwt.Drawing.Colors.IndianRed;
		public readonly static Xwt.Drawing.Color LogInColor = Xwt.Drawing.Colors.Orange;
		public readonly static Xwt.Drawing.Color CorrectAnswerColor = Xwt.Drawing.Colors.LightGreen;

		public readonly static Xwt.Drawing.Color WrongAnswerBorderColor = Xwt.Drawing.Colors.DarkRed;
		public readonly static Xwt.Drawing.Color LogInBorderColor = Xwt.Drawing.Colors.DarkOrange;
		public readonly static Xwt.Drawing.Color CorrectAnswerBorderColor = Xwt.Drawing.Colors.DarkGreen;

		public readonly static int FontSizeResult = 30;
		public readonly static int FontSizeQuestion = 36;
		public readonly static int BorderRadius = 16;
		public readonly static int BorderWidth = 4;
		public readonly static int JokerSize = 50;

		public readonly static string QuizFilePath = "quiz.txt";

		// Make constructor private to avoid instantiation
		private Parameter() {
			// Nothing to do
		}
	}
}