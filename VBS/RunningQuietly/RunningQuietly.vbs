' Running some application quietly in the background and hiding the window.

dim wsh
Set wsh=CreateObject("wscript.shell")
wsh.run "D:\Soft\test.bat",0,False
