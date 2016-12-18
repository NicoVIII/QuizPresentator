using System;
using System.Collections.Generic;
using Xwt;
using Xwt.Drawing;

namespace QuizPresentator {
	public class ResultBoxes : HBox {
		VBox[] lifelineBoxes;
		ResultBox[] boxes;
		ImageView[,] lifelines;

		public ResultBoxes(int nrOfQuestions, int nrOfParties, int nrOfLifelines, Logic.Quiz quiz) {
			boxes = new ResultBox[nrOfParties];
			lifelineBoxes = new VBox[nrOfParties];
			// Initialise boxes
			int questionsLeft = nrOfQuestions;
			lifelines = new ImageView[nrOfParties, nrOfLifelines];
			for (int i = 0; i < nrOfParties; i++) {
				lifelineBoxes[i] = new VBox();

				for (int j = 0; j < nrOfLifelines; j++) {
					Logic.Lifeline ll = quiz.Parties[i].Lifelines[j];

					//Determine image
					Image image = null;
					switch (ll.Name) {
						case "50-50":
							image = Image.FromFile("images/lifelines/50-50.png").WithSize(85, 85);
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

				int questions = (int) Math.Ceiling((double)(questionsLeft / (nrOfParties - i)));
				questionsLeft -= questions;
				ResultBox box = new ResultBox(questions);
				boxes[i] = box;
				PackStart(box);
			}
		}

		public void Update(Logic.Quiz quiz) {
			int i = 0;
			List<bool>[] results = new List<bool>[quiz.NrOfParties];
			for (int j = 0; j < quiz.NrOfParties; j++) {
				results[j] = new List<bool>();
			}

			foreach (bool result in quiz.Results) {
				results[i].Add(result);
				i = (i + 1) % quiz.NrOfParties;
			}

			for (int j = 0; j < quiz.NrOfParties; j++) {
				boxes[j].Update(results[j], quiz.ResultOfParty(j), (quiz.Results.Length % quiz.NrOfParties) == j);
			}

			// Update lifelines
			for (int j = 0; j < quiz.NrOfParties; j++) {
				for (int k = 0; k < quiz.NrOfLifelines; k++) {
					if (quiz.Parties[j].Lifelines[k].Used) {
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