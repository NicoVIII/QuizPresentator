using System;
using System.Collections.Generic;
using Xwt;

namespace QuizPresentator {
	public class ResultBoxes : HBox {
		ResultBox[] boxes;

		public ResultBoxes(int nrOfQuestions, int nrOfParties) {
			boxes = new ResultBox[nrOfParties];
			// Initialise boxes
			int questionsLeft = nrOfQuestions;
			for (int i = 0; i < nrOfParties; i++) {
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
		}
	}
}