::Libraries
mkdir "%~1\lib"
copy /y "%~2\ThirdParty\XWT\Xwt.Gtk\bin\Release\Xwt.Gtk.dll" "%~1\lib\"
copy /y "%~2\ThirdParty\XWT\Xwt.Gtk\bin\Release\Xwt.Gtk.dll.config" "%~1\lib\"
copy /y "%~2\ThirdParty\XWT\Xwt.WPF\bin\Release\Xwt.WPF.dll" "%~1\lib\"
move /y "%~1\FSharp.Core.dll" "%~1\lib\"
move /y "%~1\Xwt.dll" "%~1\lib\"
move /y "%~1\Logic.dll" "%~1\lib\"

::Licenses
mkdir "%~1\licenses"
move /y "%~1\LICENSE.txt" "%~1\licenses\"
move /y "%~1\LICENSE_XWT.txt" "%~1\licenses\"