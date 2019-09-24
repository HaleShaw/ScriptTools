@echo off

title 一鍵加域 v2.1

REM Author: JianWudao
REM Function: 一鍵加域 v2.1
REM Date: 2016-10-07

REM 更新日誌：
REM 日期：2016-10-07
REM 內容：1、更改主頁
REM 			 2、刪除“的快捷方式”
REM 			 3、管理群組設定

setlocal enabledelayedexpansion

rem 檢測文件
set file="C:\Program Files\Support Tools\netdom.exe"
If not exist %file% (goto End0) else (goto StartS)

:End0
echo NETDOM文件不存在，請先安裝插件程式
pause>nul
exit

:StartS
rem 檢測網絡
ipconfig
echo.
ping idsbg.lh.com
pause
cls

rem 更改電腦名
echo 輸入新電腦名
set /p name=輸入新電腦名 1>nul 2>nul
echo 正在更改電腦名……
reg add "HKLM\System\ControlSet001\Control\ComputerName\ActiveComputerName" /v "ComputerName" /t REG_SZ /d "%name%" /f 1>nul 2>nul
reg add "HKLM\System\ControlSet001\Control\ComputerName\ComputerName" /v "ComputerName" /t REG_SZ /d "%name%" /f 1>nul 2>nul
reg add "HKLM\System\ControlSet001\Services\Tcpip\Parameters" /v "NV Hostname" /t REG_SZ /d "%name%" /f 1>nul 2>nul
reg add "HKLM\System\ControlSet001\Services\Tcpip\Parameters" /v "Hostname" /t REG_SZ /d "%name%" /f 1>nul 2>nul
reg add "HKLM\System\CurrentControlSet\Control\ComputerName\ActiveComputerName" /v ComputerName /t reg_sz /d "%name%" /f 1>nul 2>nul
reg add "HKLM\System\CurrentControlSet\Control\ComputerName\ComputerName" /v "ComputerName" /t REG_SZ /d "%name%" /f 1>nul 2>nul
reg add "HKLM\System\CurrentControlSet\Services\Tcpip\Parameters" /v "NV Hostname" /t reg_sz /d "%name%" /f 1>nul 2>nul
reg add "HKLM\System\CurrentControlSet\Services\Tcpip\Parameters" /v Hostname /t reg_sz /d "%name%" /f 1>nul 2>nul

cls
echo 正在加域……
netdom join %computername% /domain:idsbg.lh.com /userd:cdmisadmin /passwordd:buyaoG@imima
ping 127.0.0.1 -n 3 1>nul 2>nul

REM 更改主頁
reg add "HKLM\SOFTWARE\Microsoft\Internet Explorer\MAIN" /f /v "Default_Page_URL" /t REG_SZ /d "http://idsbg.efoxconn.com/" 1>nul 2>nul
reg add "HKLM\SOFTWARE\Microsoft\Internet Explorer\MAIN" /f /v "Start Page" /t REG_SZ /d "http://idsbg.efoxconn.com/" 1>nul 2>nul
reg add "HKCU\Software\Microsoft\Internet Explorer\Main" /f /v "Start Page" /t REG_SZ /d "http://idsbg.efoxconn.com/" 1>nul 2>nul
reg add "HKCU\Software\Microsoft\Internet Explorer\Main" /f /v "Start Page Redirect Cache" /t REG_SZ /d "http://idsbg.efoxconn.com/" 1>nul 2>nul

REM 刪除“的快捷方式”
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer" /f /v "link" /t REG_BINARY /d "00000000" 1>nul 2>nul

REM 管理群組設定
net user administrator Y0ngW@ngZ$ 1>nul 2>nul
net localgroup administrators "IDSBG\LocalAdmin" /add 1>nul 2>nul
net localgroup administrators "IDSBG\Domain Admins" /add 1>nul 2>nul
cls
rundll32.exe shell32.dll,Control_RunDLL sysdm.cpl,,1

set "tmp1=ipconfig /all|find "Physical Address""

rem 顯示在螢幕上
echo 電腦名為：
echo %name%
echo Mac地址為：

%tmp1%>>a.tmp
rem 清除冒號前的字元
for /f "tokens=2,* delims=:" %%i in (a.tmp)do echo %%i>>b.tmp
del /q /f a.tmp 1>nul 2>nul
copy /y b.tmp a.tmp 1>nul 2>nul
del /q /f b.tmp 1>nul 2>nul

rem 清除錯誤Mac
set file0=a.tmp
set "file0=%file0:"=%"
for %%i in ("%file0%") do set file0=%%~fi
set replaced=00-00-00-00-00-00
set all=
for /f "delims=" %%i in ('type "%file0%"') do (
    set str=%%i
    set "str=!str:%replaced%=%all%!"
    echo !str!>>b.tmp
)
del /q /f a.tmp 1>nul 2>nul
copy /y b.tmp a.tmp 1>nul 2>nul
del /q /f b.tmp 1>nul 2>nul

rem 清除空格並顯示Mac
for /f "tokens=*" %%c in (a.tmp) do echo %%c
del /q /f a.tmp 1>nul 2>nul

echo.
pause
cls

COLOR F4
echo 按任意鍵將關機並自我銷毀，取消請按Q 或直接關閉視窗!
set x=
set /p x= 1>nul 2>nul
if not "%x%"=="q" goto DelS
if "%x%"=="q" goto End

:End
exit

:DelS
shutdown -s -f -t 8
set f_n="%~nx0"
set f_p="%~dp0"
@ping -n 2 127.1 >nul 2>nul
@taskkill /f /im %f_n% 1>nul 2>nul
del /f /q %f_p%\%f_n% 1>nul 2>nul
RD /S /Q %Temp% 1>nul 2>nul