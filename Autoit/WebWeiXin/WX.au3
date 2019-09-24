#Region ;**** 由 AccAu3Wrapper_GUI 创建指令 ****
#AccAu3Wrapper_Icon=WX.ico
#AccAu3Wrapper_Outfile=WX.exe
#AccAu3Wrapper_UseX64=n
#AccAu3Wrapper_Res_Fileversion=1.0.0.1
#AccAu3Wrapper_Res_Fileversion_AutoIncrement=y
#AccAu3Wrapper_Res_ProductVersion=1.0
#AccAu3Wrapper_Res_Language=2052
#AccAu3Wrapper_Res_requestedExecutionLevel=None
#AccAu3Wrapper_Run_Tidy=y
#AccAu3Wrapper_Antidecompile=y
#EndRegion ;**** 由 AccAu3Wrapper_GUI 创建指令 ****
#cs ＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿

	脚本作者:JianWudao
	脚本功能:WebWX
	建立日期:2017-03-03
	更新日志:

#ce ＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿脚本开始＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿
#include <Constants.au3>
#include <WindowsConstants.au3>
#include <GUIConstants.au3>

Opt("TrayIconHide", 0)
Opt("TrayMenuMode", 3)
Opt("TrayOnEventMode", 1)
TraySetOnEvent($TRAY_EVENT_PRIMARYUP, "tray")
TraySetOnEvent($TRAY_EVENT_SECONDARYUP, "trayexit")

Global $sTitle = "WebWX"
Global $sVer = "v1.0"
Local $TrayBool = 1
Global $hMain = GUICreate($sTitle & " " & $sVer, 1000, 800, -1, -1, -1)

$oIE = ObjCreate("Shell.Explorer.2")
GUICtrlCreateObj($oIE, 0, 0, 1000, 800)
$oIE.navigate("https://wx2.qq.com/")

GUISetState(@SW_SHOW)

While 1
	$Msg = GUIGetMsg()
	Switch $Msg
		Case $GUI_EVENT_CLOSE
			GUISetState(@SW_HIDE, $hMain)
			$TrayBool = 0
	EndSwitch
WEnd

Func tray()
	If ($TrayBool == 0) Then
		GUISetState(@SW_SHOW, $hMain)
		WinActivate($hMain)
		$TrayBool = 1
	ElseIf ($TrayBool == 1) Then
		GUISetState(@SW_HIDE, $hMain)
		$TrayBool = 0
	EndIf
EndFunc   ;==>tray


Func trayexit()
	Exit
EndFunc   ;==>trayexit
