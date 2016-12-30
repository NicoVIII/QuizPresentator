#Libraries
mkdir "$1/lib"
cp "$2/ThirdParty/XWT/Xwt.Gtk/bin/Release/Xwt.Gtk.dll" "$1/lib/"
cp "$2/ThirdParty/XWT/Xwt.Gtk/bin/Release/Xwt.Gtk.dll.config" "$1/lib/"
mv "$1/FSharp.Core.dll" "$1/lib/"
mv "$1/Xwt.dll" "$1/lib/"
mv "$1/Logic.dll" "$1/lib/"

#Licenses
mkdir "$1/licenses"
mv "$1/LICENSE.txt" "$1/licenses/"
mv "$1/LICENSE_XWT.txt" "$1/licenses/"
exit 0