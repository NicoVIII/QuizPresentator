del /q "%~1\lib\*"
del /q "%~1\licenses\*"

::Workaround, don't know why those are there
rmdir "%~1\de"
rmdir "%~1\ja"