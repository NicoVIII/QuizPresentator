using Xwt;

namespace QuizPresentation {
	/// <summary>
	/// UtilityClass to initialize Xwt differently on different development plattforms.
	/// </summary>
	public class Initializer {
		private Initializer() {
			// Nothing to do
		}

		public static void Initialize() {
			Application.Initialize(ToolkitType.Gtk);
		}
	}
}
