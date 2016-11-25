namespace QuizPresentation {
	public sealed class Parameter {
		public readonly static int NrOfParties = 2;
		/// <summary>
		/// The color of the background of used boxes.
		/// </summary>
		public readonly static Xwt.Drawing.Color BoxBackgroundColor = Xwt.Drawing.Colors.SkyBlue;

		public readonly static Xwt.Drawing.Color LogInColor = Xwt.Drawing.Colors.DarkOrange;

		public readonly static Xwt.Drawing.Color CorrectAnswerColor = Xwt.Drawing.Colors.DarkGreen;

		public readonly static Xwt.Drawing.Color WrongAnswerColor = Xwt.Drawing.Colors.DarkRed;

		public readonly static string QuizFilePath = "quiz.txt";

		// Make constructor private to avoid instantiation
		private Parameter() {
			// Nothing to do
		}
	}
}