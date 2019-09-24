#Region ;**** 由 AccAu3Wrapper_GUI 创建指令 ****
#AccAu3Wrapper_Icon=MRRemote.ico
#AccAu3Wrapper_Outfile=MRRemote.exe
#AccAu3Wrapper_UseX64=n
#AccAu3Wrapper_Res_Fileversion=1.0.0.8
#AccAu3Wrapper_Res_Fileversion_AutoIncrement=y
#AccAu3Wrapper_Res_ProductVersion=1.0
#AccAu3Wrapper_Res_Language=2052
#AccAu3Wrapper_Res_requestedExecutionLevel=None
#AccAu3Wrapper_Run_Tidy=y
#AccAu3Wrapper_Antidecompile=y
#EndRegion ;**** 由 AccAu3Wrapper_GUI 创建指令 ****
#cs ＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿

	脚本作者:JianWudao
	脚本功能:视讯监控
	建立日期:2016-11-18
	更新日志:

#ce ＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿脚本开始＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿
#include <WindowsConstants.au3>
#include <GUIConstants.au3>

Opt("TrayIconHide", 1)

Global $sTitle = "MRRemote"
Global $sVer = "v1.0"

Global $hMain = GUICreate($sTitle & " " & $sVer, 1000, 325, -1, -1, -1)
Local $idCheckbox = GUICtrlCreateCheckbox("Top Most", 6, -1, 65, 25)
Local $idLable = GUICtrlCreateLabel("透明度", 160, 6, 40, 25)
Local $idSlider = GUICtrlCreateSlider(200, -1, 300, 25)
GUICtrlSetLimit(-1, 255, 0)
GUICtrlSetData($idSlider, 200)

$oIE = ObjCreate("Shell.Explorer.2")
GUICtrlCreateObj($oIE, 0, 30, 1000, 300)
$oIE.navigate("http://10.244.213.180/web/call")
Sleep(2000)
GUISetState(@SW_SHOW)
WinSetTrans($hMain, "", GUICtrlRead($idSlider))
Sleep(500)

Send("admin")
Sleep(200)
Send("{Tab}")
Sleep(200)
Send("1")
Sleep(200)
Send("{Enter}")
Sleep(2000)
WinActivate($hMain)
WinWaitActive($hMain)
MouseWheel("down", 4)

While 1
	$Msg = GUIGetMsg()
	Switch $Msg
		Case $GUI_EVENT_CLOSE
			Exit
		Case $idSlider
			WinSetTrans($hMain, "", GUICtrlRead($idSlider))
		Case $idCheckbox
			If _IsChecked($idCheckbox) Then
				WinSetOnTop($hMain, "Example", 1)
			Else
				WinSetOnTop($hMain, "Example", 0)
			EndIf
	EndSwitch
WEnd

Func _IsChecked($idControlID)
	Return BitAND(GUICtrlRead($idControlID), $GUI_CHECKED) = $GUI_CHECKED
EndFunc   ;==>_IsChecked
