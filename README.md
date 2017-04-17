# QuizPresentator
[![GitHub release](https://img.shields.io/github/release/NicoVIII/QuizPresenter.svg)]()
[![Github Releases](https://img.shields.io/github/downloads/NicoVIII/QuizPresenter/latest/total.svg)]()
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/NicoVIII/QuizPresenter/master/LICENSE.txt)

The goal of this project is to provide a tool to simply present a quiz. It should be a tool for a moderator of a quiz on a party or something similar.

## Prerequisites
* Mono or a version of .NET (I guess?)
* GtkSharp on Unix Platforms (http://www.mono-project.com/docs/gui/gtksharp/)

## Handling
Use "space" to go through the screens. If there is a question, use the 1-4 keys to log in an answer. Use "space" again to show the result and another time to go to the next question.
If you have logged in an answer, you can still change the logged in answer with the 1-4 keys and "log out" it with the backspace or delete key.
To use a lifeline use the F1-F4 keys. At the moment the F1 key is used for the first lifeline, F2 for the second etc. If you have used the first one you still have to press F2 to use the second one, although it seems the second is the first one now, because the original first one disappeared. (Confused? I'm sorry ^.^)

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
[![Build Status (Windows)](https://ci.appveyor.com/api/projects/status/3b9teq09cpx52k0w/branch/master?svg=true)](https://ci.appveyor.com/project/NicoVIII/quizpresenter/branch/master)
[![Build Status (Unix)](https://travis-ci.org/NicoVIII/QuizPresenter.svg?branch=master)](https://travis-ci.org/NicoVIII/QuizPresenter)
[![GitHub issues](https://img.shields.io/github/issues/NicoVIII/QuizPresenter.svg)](https://github.com/NicoVIII/QuizPresenter/issues)

### Versioning
I will try to stick to Semantic Versioning 2.0.0 (http://semver.org/spec/v2.0.0.html).

### Used Tools
I write the code in "Xamarin Studio" (https://www.xamarin.com/studio).

I use "SourceTree" (https://www.sourcetreeapp.com/) to manage git.

### Used Third-Party Libraries
* Mono (http://www.mono-project.com/)
* XWT (https://github.com/mono/xwt)
