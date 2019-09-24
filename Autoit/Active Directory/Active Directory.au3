#Region ;**** 由 AccAu3Wrapper_GUI 创建指令 ****
#AccAu3Wrapper_Icon=Active Directory.ico
#AccAu3Wrapper_Outfile=Active Directory.exe
#AccAu3Wrapper_UseX64=n
#AccAu3Wrapper_Res_Comment=Active Directory
#AccAu3Wrapper_Res_Description=Active Directory
#AccAu3Wrapper_Res_Fileversion=0.0.3.7
#AccAu3Wrapper_Res_ProductVersion=1.0
#AccAu3Wrapper_Res_LegalCopyright=劍無道
#AccAu3Wrapper_Res_Language=2052
#AccAu3Wrapper_Res_requestedExecutionLevel=None
#AccAu3Wrapper_Res_Icon_Add=AD_ShuaX.ico
#AccAu3Wrapper_Run_Tidy=y
#AccAu3Wrapper_Antidecompile=y
#EndRegion ;**** 由 AccAu3Wrapper_GUI 创建指令 ****
#cs ＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿

	脚本作者:JianWudao
	脚本功能:Active Direcory
	建立日期:2016-10-31
	更新日志:
	2017-07-10
	1、修復刪除SuperNotes緩存Bug
	2、完善更改服務器代碼

	2017-07-05
	1、修復樹目錄展開函數
	2、修復新增帳戶時選擇OU問題
	3、修復“查詢用戶信息”裡面最後修改時間異常
	4、新增“添加USB權限”
	5、優化錯誤代碼顯示

	2017-07-04
	1、界面改版，優化日誌輸出
	2、增加“查詢用戶信息”
	3、增加“刪除登錄日誌”
	4、增加“添加打印機權限”
	5、增加綁定電腦名後刪除登錄日誌
	6、增加重置密碼後刪除登錄日誌
	7、增加“刪除SuperNotes緩存”

	2017-07-03
	1、增加重置密碼的時候判斷帳戶是否已鎖定，是否已禁用
	2、增加綁定帳號的時候密碼是否已經過期，是否已鎖定，是否已禁用
	2、增加增加綁定的時候密碼是否已經過期，是否已鎖定，是否已禁用

	2017-06-30
	1、增加托盤菜單
	2、增加刷新按鈕
	3、增加電腦名輸入框背景和默認提示語

	2017-06-29
	1、增加“增加綁定”功能
	2、重新排版，更人性化
	3、增加時間和作者顯示
	4、增加快捷鍵功能

	2017-06-27
	1、增加手動修改目錄樹，重載目錄樹
	2、增加切換服務器

#ce ＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿脚本开始＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿

#include <AD.au3>
#include <Array.au3>
#include <Date.au3>
#include <FontConstants.au3>
#include <GUIConstants.au3>
#include <GuiTreeView.au3>
#include <Misc.au3>
#include <MsgBoxConstants.au3>
#include <TrayConstants.au3>
#include <TreeviewConstants.au3>
#include <WindowsConstants.au3>

Opt("TrayAutoPause", 0)
Opt("TrayIconHide", 0)
Opt("TrayMenuMode", 3)
Opt("TrayOnEventMode", 1)

Global $sOU = "OU=CD,OU=Users,OU=DSBG,DC=idsbg,DC=lh,DC=com" ; FQDN of the OU where to start
Global $sTitle = "Active Directory"
Global $sVer = "v3.7"
Global $ADServer

#Region  ***限制運行一個實例***
If _Singleton("Active Directory", 1) = 0 Then
	MsgBox($MB_SYSTEMMODAL, "提示", $sTitle & " " & $sVer & "已經運行！")
	WinActivate($sTitle & " " & $sVer)
	Exit
EndIf
#EndRegion ***限制運行一個實例***

#Region  ***托盤菜單相關***
Local $idAbout = TrayCreateItem("關於")
TrayCreateItem("")
Local $idExit = TrayCreateItem("退出")
TraySetState($TRAY_ICONSTATE_SHOW)
TraySetClick(8)

TrayItemSetOnEvent($idAbout, "About")
TrayItemSetOnEvent($idExit, "idExit")
#EndRegion ***托盤菜單相關***

#Region ### 定義常量變量 ###
Global $hMain = GUICreate($sTitle & " " & $sVer, 500, 700, -1, -1)
Global $hTree = GUICtrlCreateTreeView(6, 45, 200, 420, -1, $WS_EX_CLIENTEDGE)
Global $DCArray[0]

Local $OUlable = GUICtrlCreateInput($sOU, 6, 11, 384, 20)
Local $OUbutton = GUICtrlCreateButton("重載目錄樹", 396, 6, 100, 30)
Local $idQieHlable = GUICtrlCreateLabel("切換服務器", 226, 47, 60, 20)
Local $idShuaXbutton = GUICtrlCreateButton("", 288, 45, 20, 20, $BS_ICON)
;GUICtrlSetImage(-1, "shell32.dll", 16739, 0)	;Win7下圖標正常，XP下圖標不正常
If @Compiled Then
	GUICtrlSetImage(-1, @AutoItExe, '201', 0) ; 文件包含多个图标时的图标名称. 如为图标序号, 则可以是负数. 否则设为 -1.
Else
	GUICtrlSetImage(-1, "AD_ShuaX.ico", -1, 0)
EndIf
Local $ADCombo = GUICtrlCreateCombo("", 310, 45, 185, 30)

;電腦名輸入框背景設定
Global $MARk = 0
Global $DEFAULTINPUTDATA = "多個電腦名請以英文逗號間隔(,)"
Global $NONEAACTIVECOLOR = 0x989898

Local $idGongHlable = GUICtrlCreateLabel("工  號", 226, 80, 40, 20)
Local $idGongHinput = GUICtrlCreateInput("", 266, 78, 85, 20, $ES_UPPERCASE)
Local $idXingMlable = GUICtrlCreateLabel("電腦名", 226, 113, 40, 20)
Local $idDianNMinput = GUICtrlCreateInput($DEFAULTINPUTDATA, 266, 111, 230, 20, $ES_UPPERCASE)
GUICtrlSetColor(-1, $NONEAACTIVECOLOR)
Local $idXingMlable = GUICtrlCreateLabel("姓  名", 371, 80, 40, 20)
Local $idXingMinput = GUICtrlCreateInput("", 411, 78, 85, 20)
Local $idMiaoSlable = GUICtrlCreateLabel("描  述", 226, 146, 40, 20)
Local $idMiaoSinput = GUICtrlCreateInput("", 266, 144, 230, 20)
Local $idMiMlable = GUICtrlCreateLabel("密  碼", 226, 179, 40, 20)
Local $idMiMinput = GUICtrlCreateInput("dsbg123.", 266, 177, 85, 20)
Local $idIPlable = GUICtrlCreateLabel("IP地址", 226, 212, 40, 20)
Local $idIPinput = GUICtrlCreateInput("", 266, 210, 230, 20)

Local $QingKbutton = GUICtrlCreateButton("清空(Q)", 226, 243, 70, 30)
Local $BangDbutton = GUICtrlCreateButton("綁定(B)", 321, 243, 70, 30)
Local $YiDbutton = GUICtrlCreateButton("移動(M)", 416, 243, 70, 30)
Local $JianLbutton = GUICtrlCreateButton("建立並綁定(J)", 226, 283, 110, 30)
Local $ZhengJbutton = GUICtrlCreateButton("增加綁定(A)", 376, 283, 110, 30)
Local $ChongZbutton = GUICtrlCreateButton("重置密碼(R)", 226, 323, 110, 30)
Local $ChaXbutton = GUICtrlCreateButton("查詢電腦名(C)", 376, 323, 110, 30)

;查詢帳戶信息
Local $ChaXUserbutton = GUICtrlCreateButton("查詢帳戶信息(U)", 226, 363, 110, 30)
Global $aProperties[1][2]
Global $UserProperties[14][2]
$UserProperties[0][0] = "13"
$UserProperties[0][1] = "2"
$UserProperties[1][0] = "工    號"
$UserProperties[2][0] = "姓    名"
$UserProperties[3][0] = "描    述"
$UserProperties[4][0] = "電 腦 名"
$UserProperties[5][0] = "路    徑"
$UserProperties[6][0] = "帳戶啟用"
$UserProperties[7][0] = "帳戶到期"
$UserProperties[8][0] = "帳戶鎖定"
$UserProperties[9][0] = "上次密碼"
$UserProperties[10][0] = "密碼到期"
$UserProperties[11][0] = "上次登錄"
$UserProperties[12][0] = "帳戶建立"
$UserProperties[13][0] = "帳戶修改"

Local $ShanCbutton = GUICtrlCreateButton("刪除登錄日誌(D)", 376, 363, 110, 30)
Local $DelSPbutton = GUICtrlCreateButton("清理SuperNotes(N)", 226, 403, 110, 30)
Local $AddUSBbutton = GUICtrlCreateButton("添加USB權限(S)", 376, 403, 110, 30)
Global $USBOU = "OU=DeviceLock,OU=Special,DC=idsbg,DC=lh,DC=com"
Global $USBGroup = "Allow_Access_to_Removable"

;添加打印機權限
Global $PrintArray[5] = ["C10-1F-C", "C25-4F-C", "D01-3F-C", "D02-3F-C", "D45-1F"]
Global $PrintGroup
Local $idPrintlable = GUICtrlCreateLabel("添加打印權限", 226, 443, 80, 20)
Local $PrintCombo = GUICtrlCreateCombo("", 311, 441, 90, 30)
Local $Printbutton = GUICtrlCreateButton("添加(P)", 416, 436, 70, 30)
GUICtrlSetData($PrintCombo, _ArrayToString($PrintArray, "|", 0, UBound($PrintArray) - 1), "")

Local $Log = ""
Local $idLogLable = GUICtrlCreateLabel("Log:", 6, 472, 40, 150)
GUICtrlSetFont($idLogLable, 9, $FW_BOLD, "", "Arial")
Local $idLogedit = GUICtrlCreateEdit($Log, 6, 492, 488, 180, $WS_VSCROLL + $ES_READONLY, $WS_EX_CLIENTEDGE)
GUICtrlSetBkColor(-1, $COLOR_WHITE)

Local $Timelable = GUICtrlCreateLabel("", 246, 680, 254, 20)
#EndRegion ### 定義常量變量 ###

TrayTip("提示", "正在執行" & $sTitle & " " & $sVer & @CRLF & "請稍候...", 3, $TIP_ICONASTERISK)
GetDC()
CheckDC()
Local $AccelKeys[12][2] = [["!q", $QingKbutton], ["!b", $BangDbutton], ["!m", $YiDbutton], ["!j", $JianLbutton], ["!a", $ZhengJbutton], _
		["!r", $ChongZbutton], ["!c", $ChaXbutton], ["!u", $ChaXUserbutton], ["!d", $ShanCbutton], ["!p", $Printbutton], ["!n", $DelSPbutton], ["!s", $AddUSBbutton]]
GUISetAccelerators($AccelKeys)
StartAD()

Func StartAD()
	$sOU = GUICtrlRead($OUlable)
	_GUICtrlTreeView_DeleteAll($hTree)
	Global $aTreeView = _AD_GetOUTreeView($sOU, $hTree, True, "user", "", 2)
	If @error <> 0 Then MsgBox(16, $sTitle, "從OU " & $sOU & " 創建列表失敗！" & @CRLF & _
			"Error returned by function _AD_GetALLOUs: @error = " & @error & ", @extended =  " & @extended, "", $hMain)
	GUISetState(@SW_SHOW)
	Sleep(50)
	_GUICtrlTreeView_SetBold($hTree, _GUICtrlTreeView_GetFirstItem($hTree))
	
	;雙擊第一層樹目錄，使其從收起狀態展開
	;_GUICtrlTreeView_ClickItem($hTree, _GUICtrlTreeView_GetFirstItem($hTree), "left", False, 2)
	;會展開所有樹目錄
	;_GUICtrlTreeView_Expand($hTree, _GUICtrlTreeView_GetFirstItem($hTree))
	;只展開第一層樹目錄
	If Not IsHWnd($hTree) Then $hTree = GUICtrlGetHandle($hTree)
	_SendMessage($hTree, $TVM_EXPAND, $TVE_EXPAND, _GUICtrlTreeView_GetFirstItem($hTree), 0, "wparam", "handle")

	Sleep(50)
	GUICtrlSetState($idGongHinput, $GUI_FOCUS)
	;_AD_Open("", "", "", $ADServer)
EndFunc   ;==>StartAD

While 1
	_CheckInput($hMain, $idDianNMinput, "多個電腦名請以英文逗號間隔(,)", $DEFAULTINPUTDATA, $MARk)
	$nt = "作者：劍無道 " & _DateDayOfWeek(@WDAY, $DMW_LOCALE_LONGNAME) & " " & _NowCalc()
	If $nt <> GUICtrlRead($Timelable) Then GUICtrlSetData($Timelable, $nt)
	$Msg = GUIGetMsg()
	Switch $Msg
		;重載樹目錄
		Case $OUbutton
			StartAD()
			
			;關閉軟件
		Case $GUI_EVENT_CLOSE
			_AD_Close()
			Exit
			
			;服務器列表下拉菜單
		Case $ADCombo
			$ADServer = GUICtrlRead($ADCombo)
			_AD_Close()
			_AD_Open("", "", "", $ADServer)
			
			;刷新服務器列表按鈕
		Case $idShuaXbutton
			TrayTip("提示", "正在刷新服務器列表" & @CRLF & "請稍候...", 3, $TIP_ICONASTERISK)
			GUICtrlSetState($idShuaXbutton, $GUI_DISABLE)
			_AD_Close()
			GetDC()
			CheckDC()
			GUICtrlSetState($idShuaXbutton, $GUI_ENABLE)
			TrayTip("提示", "刷新服務器列表完成！", 3, $TIP_ICONASTERISK)
			
			;清空按鈕
		Case $QingKbutton
			GUICtrlSetData($idGongHinput, "")
			GUICtrlSetData($idXingMinput, "")
			GUICtrlSetData($idMiaoSinput, "")
			GUICtrlSetData($idMiMinput, "dsbg123.")
			GUICtrlSetData($idDianNMinput, "")
			GUICtrlSetData($idIPinput, "")
			GUICtrlSetData($idLogedit, "")
			_GUICtrlTreeView_SelectItem($hTree, _GUICtrlTreeView_GetFirstItem($hTree))
			GUICtrlSetData($PrintCombo, _ArrayToString($PrintArray, "|", 0, UBound($PrintArray) - 1), "")
			GUICtrlSetState($idGongHinput, $GUI_FOCUS)

			;綁定按鈕
		Case $BangDbutton
			If GUICtrlRead($idGongHinput) = "" Then
				MsgBox("", "提示", "工號不能為空！", "", $hMain)
			ElseIf GUICtrlRead($idDianNMinput) = "" Or GUICtrlRead($idDianNMinput) = $DEFAULTINPUTDATA Or GUICtrlRead($idDianNMinput) = "多個電腦名請以英文逗號間隔(,)" Then
				MsgBox("", "提示", "電腦名不能為空！", "", $hMain)
			Else
				$Log = "Time：" & _NowTime() & @CRLF
				Global $iValue = _AD_BindCompuer(GUICtrlRead($idGongHinput), GUICtrlRead($idDianNMinput))
				If $iValue = 1 Then
					$Log &= GUICtrlRead($idGongHinput) & " 綁定 " & GUICtrlRead($idDianNMinput) & " 成功！" & @CRLF
					If _AD_IsPasswordExpiredNew(GUICtrlRead($idGongHinput)) = 1 Then
						If GUICtrlRead($idMiMinput) = "" Then
							$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 密碼已過期，但密碼為空，未重置密碼！" _
									 & @CRLF & "請輸入密碼後再重置密碼！" & @CRLF
						Else
							Global $iValue0 = _AD_SetPassword(GUICtrlRead($idGongHinput), GUICtrlRead($idMiMinput))
							If $iValue0 = 1 Then
								$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 之前密碼已過期，現密碼已修改成功！" & @CRLF
								If _AD_IsObjectLocked(GUICtrlRead($idGongHinput)) Then
									Global $iValue2 = _AD_UnlockObject(GUICtrlRead($idGongHinput))
									If $iValue2 = 1 Then
										$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 之前已鎖定，現已解鎖！" & @CRLF
									ElseIf @error = 1 Then
										$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
									Else
										$Log &= "Error：" & @error & @CRLF
									EndIf
								EndIf
								If _AD_IsObjectDisabled(GUICtrlRead($idGongHinput)) Then
									Global $iValue3 = _AD_EnableObject(GUICtrlRead($idGongHinput))
									If $iValue3 = 1 Then
										$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 之前已禁用，現已啟用！" _
												 & @CRLF & _AD_GetObjectAttribute(GUICtrlRead($idGongHinput), "distinguishedname") & @CRLF
									ElseIf @error = 1 Then
										$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
									Else
										$Log &= "Error：" & @error & @CRLF
									EndIf
								EndIf
							ElseIf @error = 1 Then
								$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
							Else
								$Log &= "Error：" & @error & @CRLF
							EndIf
						EndIf
					EndIf
					Local $UserLog = "\\10.244.170.206\limitloginlogs$\" & GUICtrlRead($idGongHinput) & ".txt"
					If FileExists($UserLog) Then
						Local $iDelete = FileDelete($UserLog)
						If $iDelete Then
							$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 登錄日誌刪除成功！" & @CRLF
						Else
							$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 登錄日誌刪除失敗！" & @CRLF
						EndIf
					Else
						$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 無登錄日誌！" & @CRLF
					EndIf
				ElseIf @error = 1 Then
					$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
					GUICtrlSetState($idXingMinput, $GUI_FOCUS)
				ElseIf @error = 2 Then
					$Log &= "電腦名 " & GUICtrlRead($idDianNMinput) & " 不存在！" & @CRLF
				Else
					$Log &= "Error：" & @error & @CRLF
				EndIf
				GUICtrlSetData($idLogedit, $Log)
			EndIf

			;增加綁定
		Case $ZhengJbutton
			If GUICtrlRead($idGongHinput) = "" Then
				MsgBox("", "提示", "工號不能為空！", "", $hMain)
			ElseIf GUICtrlRead($idDianNMinput) = "" Or GUICtrlRead($idDianNMinput) = $DEFAULTINPUTDATA Or GUICtrlRead($idDianNMinput) = "多個電腦名請以英文逗號間隔(,)" Then
				MsgBox("", "提示", "電腦名不能為空！", "", $hMain)
			Else
				$Log = "Time：" & _NowTime() & @CRLF
				Global $iValue = _AD_BindCompuerRetain(GUICtrlRead($idGongHinput), GUICtrlRead($idDianNMinput))
				If $iValue = 1 Then
					$Log &= GUICtrlRead($idGongHinput) & " 增加綁定 " & GUICtrlRead($idDianNMinput) & " 成功！" & @CRLF
					$Log &= "所有電腦名：" & _AD_GetComputer(GUICtrlRead($idGongHinput)) & @CRLF
					If _AD_IsPasswordExpiredNew(GUICtrlRead($idGongHinput)) = 1 Then
						If GUICtrlRead($idMiMinput) = "" Then
							$Log &= GUICtrlRead($idGongHinput) & " 密碼已過期，但密碼為空，未重置密碼！" _
									 & @CRLF & "請輸入密碼後再重置密碼！" & @CRLF
						Else
							Global $iValue0 = _AD_SetPassword(GUICtrlRead($idGongHinput), GUICtrlRead($idMiMinput))
							If $iValue0 = 1 Then
								$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 之前密碼已過期，現密碼已修改成功！" & @CRLF
								If _AD_IsObjectLocked(GUICtrlRead($idGongHinput)) Then
									Global $iValue2 = _AD_UnlockObject(GUICtrlRead($idGongHinput))
									If $iValue2 = 1 Then
										$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 之前已鎖定，現已解鎖！" & @CRLF
									ElseIf @error = 1 Then
										$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
									Else
										$Log &= "Error：" & @error & @CRLF
									EndIf
								EndIf
								If _AD_IsObjectDisabled(GUICtrlRead($idGongHinput)) Then
									Global $iValue3 = _AD_EnableObject(GUICtrlRead($idGongHinput))
									If $iValue3 = 1 Then
										$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 之前已禁用，現已啟用！" _
												 & @CRLF & _AD_GetObjectAttribute(GUICtrlRead($idGongHinput), "distinguishedname") & @CRLF
									ElseIf @error = 1 Then
										$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
									Else
										$Log &= "Error：" & @error & @CRLF
									EndIf
								EndIf
							ElseIf @error = 1 Then
								$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
							Else
								$Log &= "Error：" & @error & @CRLF
							EndIf
						EndIf
					EndIf
					Local $UserLog = "\\10.244.170.206\limitloginlogs$\" & GUICtrlRead($idGongHinput) & ".txt"
					If FileExists($UserLog) Then
						Local $iDelete = FileDelete($UserLog)
						If $iDelete Then
							$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 登錄日誌刪除成功！" & @CRLF
						Else
							$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 登錄日誌刪除失敗！" & @CRLF
						EndIf
					Else
						$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 無登錄日誌！" & @CRLF
					EndIf
				ElseIf @error = 1 Then
					$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
					GUICtrlSetState($idXingMinput, $GUI_FOCUS)
				ElseIf @error = 2 Then
					$Log &= "電腦名 " & GUICtrlRead($idDianNMinput) & " 不存在！" & @CRLF
				Else
					$Log &= "Error：" & @error & @CRLF
				EndIf
				GUICtrlSetData($idLogedit, $Log)
			EndIf
			
			;建立按鈕
		Case $JianLbutton
			$hSelection = _GUICtrlTreeView_GetSelection($hTree)
			$sSelection = _GUICtrlTreeView_GetText($hTree, $hSelection)
			For $i = 1 To $aTreeView[0][0]
				If $hSelection = $aTreeView[$i][2] Then ExitLoop
			Next
			$sOU = $aTreeView[$i][1]
			If $sOU = "OU=CD,OU=Users,OU=DSBG,DC=idsbg,DC=lh,DC=com" Then
				MsgBox("", "提示", "請選擇OU！", "", $hMain)
			Else
				If GUICtrlRead($idGongHinput) = "" Then
					MsgBox("", "提示", "工號不能為空！", "", $hMain)
				ElseIf GUICtrlRead($idXingMinput) = "" Then
					MsgBox("", "提示", "姓名不能為空！", "", $hMain)
				ElseIf GUICtrlRead($idMiaoSinput) = "" Then
					MsgBox("", "提示", "描述不能為空！", "", $hMain)
				ElseIf GUICtrlRead($idDianNMinput) = "" Then
					MsgBox("", "提示", "電腦名不能為空！", "", $hMain)
				Else
					$Log = "Time：" & _NowTime() & @CRLF
					Local $command = "dsadd user " & '"' & "cn=" & GUICtrlRead($idXingMinput) & "," & $sOU _
							 & '"' & " -s " & '"' & $ADServer & '"' & " -samid " & GUICtrlRead($idGongHinput) _
							 & " -upn " & GUICtrlRead($idGongHinput) & "@idsbg.lh.com -fn " & '"' & GUICtrlRead($idXingMinput) _
							 & '"' & " -display " & '"' & GUICtrlRead($idXingMinput) & '"' & " -pwd dsbg123. -disabled no  -mustchpwd no -desc " _
							 & '"' & GUICtrlRead($idMiaoSinput) & '"'
					Local $foo = Run(@ComSpec & " /c " & $command, _
							@ScriptDir, @SW_HIDE, $STDERR_CHILD + $STDOUT_CHILD)
					While 1
						$sDat1 = StdoutRead($foo)
						$sDat2 = StderrRead($foo)
						If @error Then ExitLoop
						$Log &= $sDat1 & $sDat2
					WEnd
					GUICtrlSetData($idLogedit, $Log)
					
					$Log &= @CRLF & @CRLF & "Time：" & _NowTime() & @CRLF
					Global $iValue = _AD_BindCompuer(GUICtrlRead($idGongHinput), GUICtrlRead($idDianNMinput))
					If $iValue = 1 Then
						$Log &= GUICtrlRead($idGongHinput) & " 綁定 " & GUICtrlRead($idDianNMinput) & "成功！" & @CRLF
					ElseIf @error = 1 Then
						$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
					ElseIf @error = 2 Then
						$Log &= "電腦名 " & GUICtrlRead($idDianNMinput) & " 不存在！" & @CRLF
					Else
						$Log &= "Error：" & @error & @CRLF
					EndIf
					GUICtrlSetData($idLogedit, $Log)
				EndIf
			EndIf

			;移動按鈕
		Case $YiDbutton
			$hSelection = _GUICtrlTreeView_GetSelection($hTree)
			$sSelection = _GUICtrlTreeView_GetText($hTree, $hSelection)
			For $i = 1 To $aTreeView[0][0]
				If $hSelection = $aTreeView[$i][2] Then ExitLoop
			Next
			$sOU = $aTreeView[$i][1]
			If GUICtrlRead($idGongHinput) = "" Then
				MsgBox("", "提示", "工號不能為空！", "", $hMain)
			Else
				$Log = "Time：" & _NowTime() & @CRLF
				Local $command = "dsquery user -samid " & GUICtrlRead($idGongHinput) & " -s " _
						 & '"' & $ADServer & '"' & " -o dn | dsmove -newparent " & '"' _
						 & $sOU & '"' & " -s " & '"' & $ADServer & '"'
				Local $foo = Run(@ComSpec & " /c " & $command, _
						@ScriptDir, @SW_HIDE, $STDERR_CHILD + $STDOUT_CHILD)
				While 1
					$sDat1 = StdoutRead($foo)
					$sDat2 = StderrRead($foo)
					If @error Then ExitLoop
					$Log &= $sDat1 & $sDat2
				WEnd
				GUICtrlSetData($idLogedit, $Log)
			EndIf

			;查詢電腦名按鈕
		Case $ChaXbutton
			If GUICtrlRead($idGongHinput) = "" Then
				MsgBox("", "提示", "工號不能為空！", "", $hMain)
			Else
				$Log = "Time：" & _NowTime() & @CRLF
				Local $command = "dsquery computer -o samid -name " & GUICtrlRead($idGongHinput) _
						 & "*" & " -s " & '"' & $ADServer & '"'
				Local $foo = Run(@ComSpec & " /c " & $command, _
						@ScriptDir, @SW_HIDE, $STDERR_CHILD + $STDOUT_CHILD)
				While 1
					$sDat1 = StdoutRead($foo)
					$sDat2 = StderrRead($foo)
					If @error Then ExitLoop
					$Log &= $sDat1 & $sDat2
				WEnd
				GUICtrlSetData($idLogedit, $Log)
			EndIf
			
			;重置密碼按鈕
		Case $ChongZbutton
			If GUICtrlRead($idGongHinput) = "" Then
				MsgBox("", "提示", "工號不能為空！", "", $hMain)
			ElseIf GUICtrlRead($idMiMinput) = "" Then
				MsgBox("", "提示", "密碼不能為空！", "", $hMain)
			Else
				$Log = "Time：" & _NowTime() & @CRLF
				Global $iValue = _AD_SetPassword(GUICtrlRead($idGongHinput), GUICtrlRead($idMiMinput))
				If $iValue = 1 Then
					$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 密碼修改成功！" & @CRLF
					If _AD_IsObjectLocked(GUICtrlRead($idGongHinput)) Then
						Global $iValue2 = _AD_UnlockObject(GUICtrlRead($idGongHinput))
						If $iValue2 = 1 Then
							$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 之前已鎖定，現已解鎖！" & @CRLF
						ElseIf @error = 1 Then
							$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
						Else
							$Log &= "Error：" & @error & @CRLF
						EndIf
					EndIf
					If _AD_IsObjectDisabled(GUICtrlRead($idGongHinput)) Then
						Global $iValue3 = _AD_EnableObject(GUICtrlRead($idGongHinput))
						If $iValue3 = 1 Then
							$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 之前已禁用，現已啟用！" _
									 & @CRLF & _AD_GetObjectAttribute(GUICtrlRead($idGongHinput), "distinguishedname") & @CRLF
						ElseIf @error = 1 Then
							$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
						Else
							$Log &= "Error：" & @error & @CRLF
						EndIf
					EndIf
					Local $UserLog = "\\10.244.170.206\limitloginlogs$\" & GUICtrlRead($idGongHinput) & ".txt"
					If FileExists($UserLog) Then
						Local $iDelete = FileDelete($UserLog)
						If $iDelete Then
							$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 登錄日誌刪除成功！" & @CRLF
						Else
							$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 登錄日誌刪除失敗！" & @CRLF
						EndIf
					Else
						$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 無登錄日誌！" & @CRLF
					EndIf
				ElseIf @error = 1 Then
					$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
				Else
					$Log &= "Error：" & @error & @CRLF
				EndIf
				GUICtrlSetData($idLogedit, $Log)
			EndIf
			
			;查詢帳戶信息
		Case $ChaXUserbutton
			$Log = "Time：" & _NowTime() & @CRLF
			If _AD_ObjectExists(GUICtrlRead($idGongHinput)) Then
				$aProperties = _AD_GetObjectProperties(GUICtrlRead($idGongHinput))
				$UserProperties[1][1] = _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "sAMAccountName")[1][1]
				$UserProperties[2][1] = _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "displayname")[1][1]
				For $i = 1 To $aProperties[0][0]
					If $aProperties[$i][0] = "description" Then
						$UserProperties[3][1] = _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "description")[1][1]
					EndIf
				Next
				If _AD_GetComputer(GUICtrlRead($idGongHinput)) = "" Then
					$UserProperties[4][1] = "All"
				Else
					$UserProperties[4][1] = _AD_GetComputer(GUICtrlRead($idGongHinput))
				EndIf
				$UserProperties[5][1] = _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "distinguishedName")[1][1]
				If _AD_IsObjectDisabled(GUICtrlRead($idGongHinput)) Then
					$UserProperties[6][1] = "禁用"
				Else
					$UserProperties[6][1] = "啟用"
				EndIf
				If _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "accountExpires")[1][1] = "1601/01/01 00:00:00" Or _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "accountExpires")[1][1] = "0000/00/00 00:00:00" Then
					$UserProperties[7][1] = "從不"
				Else
					$UserProperties[7][1] = _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "accountExpires")[1][1]
				EndIf
				If _AD_IsObjectLocked(GUICtrlRead($idGongHinput)) Then
					$UserProperties[8][1] = "鎖定"
				Else
					$UserProperties[8][1] = "未鎖定"
				EndIf
				$UserProperties[9][1] = _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "pwdLastSet")[1][1]
				If _AD_GetPasswordInfo(GUICtrlRead($idGongHinput))[9] = "" Then
					$UserProperties[10][1] = "從不"
				Else
					$UserProperties[10][1] = _AD_GetPasswordInfo(GUICtrlRead($idGongHinput))[9]
				EndIf
				
				For $i = 1 To $aProperties[0][0]
					If $aProperties[$i][0] = "lastLogon" Then
						$UserProperties[11][1] = _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "lastLogon")[1][1]
					EndIf
				Next
				For $i = 1 To $aProperties[0][0]
					If $aProperties[$i][0] = "lastLogonTimestamp" Then
						$UserProperties[11][1] = _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "lastLogonTimestamp")[1][1]
					EndIf
				Next
				If $UserProperties[11][1] = "1601/01/01 00:00:00" Then
					$UserProperties[11][1] = "從未登錄"
				EndIf

				$UserProperties[12][1] = _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "whenCreated")[1][1]
				$UserProperties[13][1] = _DateAdd('h', 8, _AD_GetObjectProperties(GUICtrlRead($idGongHinput), "whenChanged")[1][1])
				$Log &= _ArrayToString($UserProperties, "：", 1, 13, @CRLF, 0, 1)
			Else
				$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
			EndIf
			GUICtrlSetData($idLogedit, $Log)
			
			;刪除登錄日誌
		Case $ShanCbutton
			If GUICtrlRead($idGongHinput) = "" Then
				MsgBox("", "提示", "工號不能為空！", "", $hMain)
			Else
				$Log = "Time：" & _NowTime() & @CRLF
				Local $UserLog = "\\10.244.170.206\limitloginlogs$\" & GUICtrlRead($idGongHinput) & ".txt"
				If FileExists($UserLog) Then
					Local $iDelete = FileDelete($UserLog)
					If $iDelete Then
						$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 登錄日誌刪除成功！" & @CRLF
					Else
						$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 登錄日誌刪除失敗！" & @CRLF
					EndIf
				Else
					$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 無登錄日誌！" & @CRLF
				EndIf
				GUICtrlSetData($idLogedit, $Log)
			EndIf
			
			;刪除SuperNotes緩存
		Case $DelSPbutton
			If GUICtrlRead($idGongHinput) = "" Then
				MsgBox("", "提示", "工號不能為空！", "", $hMain)
			ElseIf GUICtrlRead($idIPinput) = "" Then
				MsgBox("", "提示", "IP地址不能為空！", "", $hMain)
			Else
				$Log = "Time：" & _NowTime() & @CRLF
				Local $iPing = Ping(GUICtrlRead($idIPinput), 1500)
				If $iPing Then
					Local $SPLogPath
					Local $CheckXPWPath = "\\" & GUICtrlRead($idIPinput) & "\C$\Users"
					If FileExists($CheckXPWPath) Then
						$SPLogPath = "\\" & GUICtrlRead($idIPinput) & "\C$\Users\" & GUICtrlRead($idGongHinput) & "\AppData\Local\SuperNotes"
					Else
						$SPLogPath = "\\" & GUICtrlRead($idIPinput) & "\C$\Documents and Settings\" & GUICtrlRead($idGongHinput) & "\Local Settings\Application Data\SuperNotes"
					EndIf
					If FileExists($SPLogPath) Then
						Local $iDelete = FileDelete($SPLogPath)
						If $iDelete Then
							$Log &= "電腦 " & GUICtrlRead($idIPinput) & " 上帳戶 " & GUICtrlRead($idGongHinput) & " SuperNotes緩存刪除成功！" & @CRLF
						Else
							$Log &= "電腦 " & GUICtrlRead($idIPinput) & " 上帳戶 " & GUICtrlRead($idGongHinput) & " SuperNotes緩存刪除失敗！" & @CRLF
						EndIf
					Else
						$Log &= "電腦 " & GUICtrlRead($idIPinput) & " 上帳戶 " & GUICtrlRead($idGongHinput) & " 無SuperNotes緩存" & @CRLF
					EndIf
				Else
					$Log &= "電腦 " & GUICtrlRead($idIPinput) & " 網絡不通！" & @CRLF
				EndIf
				GUICtrlSetData($idLogedit, $Log)
			EndIf
			
			;添加USB權限
		Case $AddUSBbutton
			If GUICtrlRead($idGongHinput) = "" Then
				MsgBox("", "提示", "工號不能為空！", "", $hMain)
			ElseIf GUICtrlRead($idDianNMinput) = "" Or GUICtrlRead($idDianNMinput) = $DEFAULTINPUTDATA Or GUICtrlRead($idDianNMinput) = "多個電腦名請以英文逗號間隔(,)" Then
				MsgBox("", "提示", "電腦名不能為空！", "", $hMain)
			ElseIf GUICtrlRead($idMiaoSinput) = "" Then
				MsgBox("", "提示", "描述不能為空！", "", $hMain)
			Else
				$Log = "Time：" & _NowTime() & @CRLF
				Global $iValue1 = _AD_AddUserToGroup($USBGroup, GUICtrlRead($idGongHinput))
				If $iValue1 = 1 Then
					$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 加入到群組 " & GUICtrlRead($PrintCombo) & " 成功！" & @CRLF
				ElseIf @error = 1 Then
					$Log &= "群組 " & GUICtrlRead($PrintCombo) & " 不存在！" & @CRLF
				ElseIf @error = 2 Then
					$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
				ElseIf @error = 3 Then
					$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 已經在群組 " & GUICtrlRead($PrintCombo) & " 裡！" & @CRLF
				Else
					$Log &= "Error：" & @error & @CRLF
				EndIf
				
				Global $iValue2 = _AD_MoveObject($USBOU, _AD_SamAccountNameToFQDN(GUICtrlRead($idDianNMinput) & "$"))
				If $iValue2 = 1 Then
					$Log &= "電腦名 " & GUICtrlRead($idDianNMinput) & " 移動到OU " & $USBOU & " 成功！" & @CRLF
				ElseIf @error = 1 Then
					$Log &= "OU " & $USBOU & " 不存在！" & @CRLF
				ElseIf @error = 2 Then
					$Log &= "電腦名 " & GUICtrlRead($idDianNMinput) & " 不存在！" & @CRLF
				Else
					$Log &= "Error：" & @error & @CRLF
				EndIf
				
				Global $iValue3 = _AD_ModifyAttribute(GUICtrlRead($idDianNMinput) & "$", "description", GUICtrlRead($idMiaoSinput))
				If $iValue3 = 1 Then
					$Log &= "電腦名 " & GUICtrlRead($idDianNMinput) & " 修改描述 " & GUICtrlRead($idMiaoSinput) & " 成功！" & @CRLF
				ElseIf @error = 1 Then
					$Log &= "電腦名 " & GUICtrlRead($idDianNMinput) & " 不存在！" & @CRLF
				Else
					$Log &= "Error：" & @error & @CRLF
				EndIf
				
				If $iValue1 = 1 And $iValue2 = 1 And $iValue3 = 1 Then
					$Log &= @CRLF & "帳戶 " & GUICtrlRead($idGongHinput) & " 電腦名 " & GUICtrlRead($idDianNMinput) & " 開通USB權限成功！" & @CRLF
				EndIf
			EndIf
			GUICtrlSetData($idLogedit, $Log)
			
			;打印機列表
		Case $PrintCombo
			Switch GUICtrlRead($PrintCombo)
				Case "C10-1F-C"
					$PrintGroup = "CDC101F-C-Printer_Group"
				Case "C25-4F-C"
					$PrintGroup = "CDC254FC-Printer_Group"
				Case "D01-3F-C"
					$PrintGroup = "CDD013F-C-Printer_Group"
				Case "D02-3F-C"
					$PrintGroup = "CD-D02-3F-C Print_Group"
				Case "D45-1F"
					$PrintGroup = "CD-D45-1F-Print_Group"
			EndSwitch
			
			;添加打印機權限
		Case $Printbutton
			If GUICtrlRead($PrintCombo) = "" Then
				MsgBox("", "提示", "請選擇打印機！", "", $hMain)
			Else
				$Log = "Time：" & _NowTime() & @CRLF
				Global $iValue = _AD_AddUserToGroup($PrintGroup, GUICtrlRead($idGongHinput))
				If $iValue = 1 Then
					$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 加入到群組 " & GUICtrlRead($PrintCombo) & " 成功！" & @CRLF
				ElseIf @error = 1 Then
					$Log &= "群組 " & GUICtrlRead($PrintCombo) & " 不存在！" & @CRLF
				ElseIf @error = 2 Then
					$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 不存在！" & @CRLF
				ElseIf @error = 3 Then
					$Log &= "帳戶 " & GUICtrlRead($idGongHinput) & " 已經在群組 " & GUICtrlRead($PrintCombo) & " 裡！" & @CRLF
				Else
					$Log &= "Error：" & @error & @CRLF
				EndIf
			EndIf
			GUICtrlSetData($idLogedit, $Log)
	EndSwitch
WEnd

#Region  ***獲取AD目錄樹***
Func _AD_GetOUTreeView($sAD_OU, $hAD_TreeView, $bAD_IsADOpen = False, $sAD_Category = "", $sAD_Text = " (%)", $iAD_SearchScope = 1)
	Local $iAD_Count
	;If $bAD_IsADOpen = False Then
	;_AD_Open("", "", "", $ADServer)
	;If @error Then Return SetError(@error, @extended, 0)
	;EndIf
	$sSeparator = "\"
	Local $aAD_OUs = _AD_GetAllOUs($sAD_OU, $sSeparator)
	If @error <> 0 Then Return SetError(@error, @extended, 0)
	Local $aAD_TreeView[$aAD_OUs[0][0] + 1][3] = [[$aAD_OUs[0][0], 3]]
	For $i = 1 To $aAD_OUs[0][0]
		$aAD_Temp = StringSplit($aAD_OUs[$i][0], $sSeparator)
		$aAD_TreeView[$i][0] = StringFormat("%" & $aAD_Temp[0] - 1 & "s", "") & "#" & $aAD_Temp[$aAD_Temp[0]]
		$aAD_TreeView[$i][1] = $aAD_OUs[$i][1]
	Next

	_GUICtrlTreeView_BeginUpdate($hAD_TreeView)
	
	Local $ahAD_Node[50], $sAD_LDAPString
	If StringIsAlNum($sAD_Category) Then
		$sAD_LDAPString = "(objectcategory=" & $sAD_Category & ")"
	Else
		$sAD_LDAPString = $sAD_Category
	EndIf
	For $iAD_Index = 1 To $aAD_TreeView[0][0]
		$sAD_Line = StringSplit(StringStripCR($aAD_TreeView[$iAD_Index][0]), @TAB)
		$iAD_Level = StringInStr($sAD_Line[1], "#")
		If $iAD_Level = 0 Then ExitLoop
		If $sAD_Category <> "" Then $iAD_Count = _AD_GetObjectsInOU($aAD_TreeView[$iAD_Index][1], $sAD_LDAPString, $iAD_SearchScope, "samaccountname", "", True)
		If $iAD_Level = 1 Then
			$sAD_Temp = ""
			If $sAD_Category <> "" Then $sAD_Temp = StringReplace($sAD_Text, "%", $iAD_Count)
			$ahAD_Node[$iAD_Level] = _GUICtrlTreeView_Add($hAD_TreeView, 0, StringMid($sAD_Line[1], $iAD_Level + 1) & $sAD_Temp)
			$aAD_TreeView[$iAD_Index][2] = $ahAD_Node[$iAD_Level]
		Else
			$sAD_Temp = ""
			If $sAD_Category <> "" Then $sAD_Temp = StringReplace($sAD_Text, "%", $iAD_Count)
			$ahAD_Node[$iAD_Level] = _GUICtrlTreeView_AddChild($hAD_TreeView, $ahAD_Node[$iAD_Level - 1], StringMid($sAD_Line[1], $iAD_Level + 1) & $sAD_Temp)
			$aAD_TreeView[$iAD_Index][2] = $ahAD_Node[$iAD_Level]
		EndIf
	Next
	;If $bAD_IsADOpen = False Then _AD_Close()
	_GUICtrlTreeView_EndUpdate($hAD_TreeView)
	Return $aAD_TreeView
EndFunc   ;==>_AD_GetOUTreeView
#EndRegion ***獲取AD目錄樹***

#Region  ***用於獲取服務器列表***
Func GetDC()
	Global $DCArray[0]
	GUICtrlSetState($ADCombo, $GUI_DISABLE)
	_AD_Open("", "", "", "IDSBG-DC05.IDSBG.LH.COM")
	If @error Then _AD_Open()
	If @error Then Exit MsgBox(16, $sTitle, "Function _AD_Open encountered a problem. @error = " & @error & ", @extended = " & @extended, "", $hMain)
	Local $aDC = _AD_ListDomainControllers()
	Local $aSite = _ArrayUnique($aDC, 2, 1)
	_ArraySort($aSite, 0, 1, 0, 1)
	For $i = 0 To UBound($aSite) - 1
		If StringInStr($aSite[$i], "idsbg-") Then
			_ArrayAdd($DCArray, StringUpper($aSite[$i]))
		EndIf
	Next
	;_AD_Close()
EndFunc   ;==>GetDC

Func CheckDC()
	For $j = 0 To UBound($DCArray) - 1
		If StringInStr($DCArray[$j], "IDSBG-DC05") Then
			$ADServer = "IDSBG-DC05.IDSBG.LH.COM"
		EndIf
	Next
	If $ADServer <> "IDSBG-DC05.IDSBG.LH.COM" Then
		For $k = 0 To UBound($DCArray) - 1
			If StringInStr($DCArray[$k], "IDSBG-DC06") Then
				$ADServer = "IDSBG-DC06.IDSBG.LH.COM"
			EndIf
		Next
	ElseIf $ADServer <> "IDSBG-DC05.IDSBG.LH.COM" And $ADServer <> "IDSBG-DC06.IDSBG.LH.COM" Then
		For $k = 0 To UBound($DCArray) - 1
			If StringInStr($DCArray[$k], "IDSBG-DC07") Then
				$ADServer = "IDSBG-DC07.IDSBG.LH.COM"
			EndIf
		Next
	ElseIf $ADServer <> "IDSBG-DC05.IDSBG.LH.COM" And $ADServer <> "IDSBG-DC06.IDSBG.LH.COM" And $ADServer <> "IDSBG-DC07.IDSBG.LH.COM" Then
		$ADServer = $DCArray[Random(0, UBound($DCArray), 1)]
	EndIf
	GUICtrlSetState($ADCombo, $GUI_ENABLE)
	GUICtrlSetData($ADCombo, "")
	GUICtrlSetData($ADCombo, _ArrayToString($DCArray, "|", 0, UBound($DCArray) - 1), $ADServer)
EndFunc   ;==>CheckDC
#EndRegion ***用於獲取服務器列表***

#Region  ***托盤菜單函數***
Func About()
	MsgBox($MB_SYSTEMMODAL, "關於", $sTitle & @CRLF & @CRLF & _
			"版本: " & $sVer & @CRLF & @CRLF & _
			"作者: 劍無道", "", $hMain)
EndFunc   ;==>About

Func idExit()
	Exit
EndFunc   ;==>idExit
#EndRegion ***托盤菜單函數***

#Region  ***檢測輸入框函數***
Func _CheckInput($hWnd, $ID, $InputDefText, ByRef $DEFAULTINPUTDATA, ByRef $MARk)
	If $MARk = 0 And _IsFocused($hWnd, $ID) And $DEFAULTINPUTDATA = $InputDefText Then
		$MARk = 1
		GUICtrlSetData($ID, "")
		GUICtrlSetColor($ID, 0x000000)
		$DEFAULTINPUTDATA = ""
	ElseIf $MARk = 1 And Not _IsFocused($hWnd, $ID) And $DEFAULTINPUTDATA = "" And GUICtrlRead($ID) = "" Then
		$MARk = 0
		$DEFAULTINPUTDATA = $InputDefText
		GUICtrlSetData($ID, $DEFAULTINPUTDATA)
		GUICtrlSetColor($ID, $NONEAACTIVECOLOR)
	EndIf
EndFunc   ;==>_CheckInput

Func _IsFocused($hWnd, $nCID)
	Return ControlGetHandle($hWnd, '', $nCID) = ControlGetHandle($hWnd, '', ControlGetFocus($hWnd))
EndFunc   ;==>_IsFocused
#EndRegion ***檢測輸入框函數***
