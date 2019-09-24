if A_Hour = 12
	SoundGet, master_mute, , mute
	if master_mute = Off
		SoundSet, +1, , mute
if A_Hour = 13
	SoundGet, master_mute, , mute
	if master_mute = On
		SoundSet, +1, , mute