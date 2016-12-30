using System;
using System.Collections.Generic;
using Xwt;
using Xwt.Drawing;

namespace QuizPresenter {
	public class ResultBoxes : HBox {
		VBox[] lifelineBoxes;
		ResultBox[] boxes;
		ImageView[,] lifelines;

		public ResultBoxes(Quiz quiz) {
			Party[] parties = quiz.Parties;
			boxes = new ResultBox[parties.Length];
			lifelineBoxes = new VBox[parties.Length];
			// Initialise boxes
			lifelines = new ImageView[parties.Length, quiz.Lifelines.Length];
			for (int i = 0; i < parties.Length; i++) {
				lifelineBoxes[i] = new VBox();
				lifelineBoxes[i].MarginLeft = 20;

				for (int j = 0; j < quiz.Lifelines.Length; j++) {
					Lifeline ll = quiz.Lifelines[j];

					//Determine image
					Image image = null;
					switch (ll.Name) {
						case "50-50":
							image = Image.FromFile("images/lifelines/50_50_lifeline.png").WithSize(85, 85);
							break;
						case "telephone":
							image = Image.FromFile("images/lifelines/telephone_lifeline.png").WithSize(85, 85);
							break;
						case "audience":
							image = Image.FromFile("images/lifelines/ask_the_audience_lifeline.png").WithSize(85, 85);
							break;
						case "additional":
							image = Image.FromFile("images/lifelines/additional_lifeline.png").WithSize(85, 85);
							break;
					}

					ImageView iv = new ImageView(image);
					lifelineBoxes[i].PackStart(iv);
					lifelines[i, j] = iv;
				}
				PackStart(lifelineBoxes[i]);

				ResultBox box = new ResultBox(parties[i]);
				boxes[i] = box;
				PackStart(box);
			}
		}

		public void Update(Quiz quiz) {
			Party[] parties = quiz.Parties;
			for (int i = 0; i < parties.Length; i++) {
				boxes[i].Update(parties[i]);
			}

			// Update lifelines
			for (int j = 0; j < quiz.Parties.Length; j++) {
				for (int k = 0; k < quiz.Lifelines.Length; k++) {
					if (quiz.Parties[j].LifelineInfos[k].Used) {
						if (lifelines[j, k] != null) {
							lifelineBoxes[j].Remove(lifelines[j, k]);
							lifelines[j, k] = null;
						}
					}
				}
			}
		}
	}
}