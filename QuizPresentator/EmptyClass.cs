using System.IO;
using Xwt;
using Xwt.Drawing;

namespace QuizPresentator {
	class ImageCanvas : Canvas {
		Image image;
		Rectangle rect;
		bool boundsChanged = false;

		public ImageCanvas(Image image) {
			this.image = image;

			BoundsChanged += (sender, e) => {
				if (this.image != null) {
					Size boxSize = new Size(Size.Width - 2 * Parameter.BorderRadius, Size.Height - 2 * Parameter.BorderRadius);
					Size imageSize = this.image.Size;
					if (imageSize.Width / imageSize.Height < boxSize.Width / boxSize.Height) {
						// Image is higher
						int width = (int)(imageSize.Width * (boxSize.Height / imageSize.Height));
						rect = new Rectangle((boxSize.Width - width) / 2, Parameter.BorderRadius, width, boxSize.Height);
					} else if (imageSize.Width / imageSize.Height > boxSize.Width / boxSize.Height) {
						int height = (int)(imageSize.Height * (boxSize.Width / imageSize.Width));
						rect = new Rectangle(Parameter.BorderRadius, (boxSize.Height - height) / 2, boxSize.Width, height);
					} else {
						rect = new Rectangle(Parameter.BorderRadius, Parameter.BorderRadius, boxSize.Width, boxSize.Height);
					}
					QueueDraw();
				}
			};
		}

		public void Update(int questionIndex) {
			string fileName = "images/" + questionIndex;
			if (File.Exists(fileName + ".jpg")) {
				image = Image.FromFile(fileName + ".jpg");
			} else if (File.Exists(fileName + ".png")) {
				image = Image.FromFile(fileName + ".png");
			} else {
				image = null;
			}

			boundsChanged = true;
			QueueDraw();
		}

		protected override void OnDraw(Context ctx, Rectangle dirtyRect) {
			if (boundsChanged) {
				OnBoundsChanged();
				boundsChanged = false;
			}

			ctx.MoveTo(Parameter.BorderRadius, 0);
			ctx.LineTo(0, Parameter.BorderRadius);
			ctx.LineTo(0, Size.Height - Parameter.BorderRadius);
			ctx.LineTo(Parameter.BorderRadius, Size.Height);
			ctx.LineTo(Size.Width - Parameter.BorderRadius, Size.Height);
			ctx.LineTo(Size.Width, Size.Height - Parameter.BorderRadius);
			ctx.LineTo(Size.Width, Parameter.BorderRadius);
			ctx.LineTo(Size.Width - Parameter.BorderRadius, 0);
			ctx.ClosePath();
			ctx.SetColor(Color.FromHsl(0, 0, 0.77));
			ctx.Fill();

			if (image != null)
				ctx.DrawImage(image, rect);
		}
	}
}
