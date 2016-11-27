# QuizPresentator
The goal of this project is to provide a tool to simply present a quiz. It should be a tool for a moderator of a quiz on a party or something similar.

## Handling
Use "space" to go through the screens. If there is a question, use the F1-F4 keys or 1-4 keys to choose an answer. Use "space" again to show the result and another time to go to the next question.

## Customisation
Questions can be customised via the quiz.txt file. Every question has to be in one line and the parameter have to be seperated by a semicolon (";").

This are the six arguments which are necessary to define a question:

1. Question
2. Correct answer (use a letter A-D in upper or lower case or a number 1-4)
3. Answer 1
4. Answer 2
5. Answer 3
6. Answer 4

## Development
### Versioning
I will try to stick to Semantic Versioning 2.0.0 (http://semver.org/spec/v2.0.0.html).

### Used Tools
I write the C# code for the most of the GUI in "Xamarin Studio" (https://www.xamarin.com/studio).

The F# code is written in "Visual Studio Code" (https://code.visualstudio.com/) with the F#-Plugin Ionide (http://ionide.io/).

I use "SourceTree" (https://www.sourcetreeapp.com/) to manage git .

### Used Third-Party Libraries
* XWT
