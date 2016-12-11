using System;
using System.Collections.Generic;
using Xwt;
using Xwt.Drawing;

namespace QuizPresentator {
	public class ResultBoxes : HBox {
		ResultBox[] boxes;

		public ResultBoxes(int nrOfQuestions, int nrOfParties) {
			boxes = new ResultBox[nrOfParties];
			// Initialise boxes
			int questionsLeft = nrOfQuestions;
			for (int i = 0; i < nrOfParties; i++) {
				VBox lifelineBox = new VBox();
				ImageView iv = new ImageView(Image.FromFile("images/lifelines/50-50.png").WithSize(85,85));
				lifelineBox.PackStart(iv);
				ImageView iv2 = new ImageView(Image.FromFile("images/lifelines/telephone_lifeline.png").WithSize(85, 85));
				lifelineBox.PackStart(iv2);
				ImageView iv3 = new ImageView(Image.FromFile("images/lifelines/ask_the_audience_lifeline.png").WithSize(85, 85));
				lifelineBox.PackStart(iv3);
				ImageView iv4 = new ImageView(Image.FromFile("images/lifelines/ask_the_audience_lifeline.png").WithSize(85, 85));
				lifelineBox.PackStart(iv4);
				ImageView iv5 = new ImageView(Image.FromFile("images/lifelines/additional_lifeline.png").WithSize(85, 85));
				lifelineBox.PackStart(iv5);
				PackStart(lifelineBox);

				int questions = (int) Math.Ceiling((double)(questionsLeft / (nrOfParties - i)));
				questionsLeft -= questions;
				ResultBox box = new ResultBox(questions);
				boxes[i] = box;
				PackStart(box);
			}
		}

		public void Update(Logic.Quiz quiz) {
			int i = 0;
			List<bool>[] results = new List<bool>[quiz.nrOfParties];
			for (int j = 0; j < quiz.nrOfParties; j++) {
				results[j] = new List<bool>();
			}

			foreach(bool result in quiz.Results) {
				results[i].Add(result);
				i = (i + 1) % quiz.nrOfParties;
			}

			for (int j = 0; j < quiz.nrOfParties; j++) {
				boxes[j].Update(results[j], quiz.ResultOfParty(j), (quiz.Results.Length % quiz.nrOfParties) == j);
			}
		}
	}
}