#加载 Winform 程序集,使用Out-Null抑制输出
[system.Reflection.Assembly]::LoadWithPartialName('System.Windows.Forms') | Out-Null
 
#创建 NotifyIcon 对象
$balloon = New-Object System.Windows.Forms.NotifyIcon
$path = Get-Process -id $pid | Select-Object -ExpandProperty Path
$icon = [System.Drawing.Icon]::ExtractAssociatedIcon($path)
$balloon.Icon = $icon
$balloon.BalloonTipIcon = 'Info'
$balloon.BalloonTipText = "该喝水了！`n保护眼睛！`n休息一下吧！"
$balloon.BalloonTipTitle = '温馨提示'
$balloon.Visible = $true
 
#显示气球提示框
$balloon.ShowBalloonTip(1000)
sleep -Seconds 5
$balloon.Dispose()