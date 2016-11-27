using Xwt;

namespace QuizPresentation {
	/// <summary>
	/// UtilityClass to initialize Xwt differently on different development plattforms.
	/// </summary>
	public class Initializer {
		private Initializer() {
			// Nothing to do
		}

		public static Window Initialize() {
			Application.Initialize(ToolkitType.Gtk);
			return new Window()
			{
				Title = "Quizpresentation",

				Width = 800,
				Height = 600//*/

				/*Decorated = true,
				FullScreen = true//*/
			};
		}
	}
}
