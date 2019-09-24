@echo off

title Set Proxy

reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Internet Settings" /v ProxyEnable /t REG_DWORD /d 1 /f
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Internet Settings" /v ProxyServer /d "proxy.wdf.sap.corp:8080" /f
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Internet Settings" /v ProxyOverride /t REG_SZ /d "localhost;127.0.0.1;*.local;*.sap.corp;10.*;*.corp.sap;*.co.sap.com;*.sap.biz;*.dayanzai.me;*.gamersky.com;*.3dmgame.com;apj-guestvoucher.wlan.sap.com;<local>" /f

Exit