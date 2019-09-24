; Win + Shift + Z
#+z::
	checkFile = D:\Script\WX\CheckWX
	IfWinExist, ahk_class WeChatMainWndForPC
	{
		If FileExist(checkFile)
		{
			WinMinimize, ahk_class WeChatMainWndForPC
			FileDelete, %checkFile%
		}
		else
		{
			FileAppend, ,%checkFile%
			WinActivate, ahk_class WeChatMainWndForPC
		}
	}
	else
	{
		Run "D:\Soft\WC\WeChat.exe"
		FileAppend, , %checkFile%
	}
Return