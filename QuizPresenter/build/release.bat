del /q "%~1\*.nlp"
del /q "%~1\*.pdb"
del /q "%~1\FSharp.Core.xml"
del /q "%~1\mscorlib.dll"
del /q "%~1\nunit.framework.dll"
del /q "%~1\nunit.framework.xml"
del /q "%~1\FsCheck.dll"
del /q "%~1\FsCheck.xml"

::Workaround, don't know why those are there
rd /s /q "%~1\de"
rd /s /q "%~1\ja"