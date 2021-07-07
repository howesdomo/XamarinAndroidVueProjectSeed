@echo off
:: Author      : Howe
:: Version     : 1.4
:: CreateTime  : 2019-10-16 11:07:46
:: Description : RAMDisk link �����ű�, �޸� Step 1 �� 2 ����, �����ִ��

:: ******* Step 1 �����ڴ��̵�·�� *******
set RamDiskRootPath="X:\Cache_SC\XamarinAndroidVueProjectSeed"

:: ******* Step 2 ���ð�����ƽ̨ *******
set AndroidProjectContain=1
set iOSProjectContain=0

echo ɾ������Ŀ��bin��obj�ļ���
::Android
if %AndroidProjectContain% EQU 1 (
rd /s/q "%~dp0\Client.AndroiD\bin"
rd /s/q "%~dp0\Client.AndroiD\obj"
rd /s/q "%~dp0\Client.AndroiD\buildAPK"
)
::iOS
if %iOSProjectContain% EQU 1 (
rd /s/q "%~dp0\Client.iOS\bin"
rd /s/q "%~dp0\Client.iOS\obj"
)

echo ����mklink
::Android
set RamDiskAndroidPath="%RamDiskRootPath%\Client.Android\"

if %AndroidProjectContain% EQU 1 (
mklink /D "%~dp0\Client.AndroiD\bin" "%RamDiskAndroidPath%\bin"
mklink /D "%~dp0\Client.AndroiD\obj" "%RamDiskAndroidPath%\obj"
mklink /D "%~dp0\Client.AndroiD\buildAPK" "%RamDiskAndroidPath%\buildAPK"
)

::iOS
set RamDiskiOSPath="%RamDiskRootPath%\Client.iOS\"
if %iOSProjectContain% EQU 1 (
mklink /D "%~dp0\Client.iOS\bin" "%RamDiskiOSPath%\bin"
mklink /D "%~dp0\Client.iOS\obj" "%RamDiskiOSPath%\obj"
)

echo ����RAMDISK��bin��obj�ļ���
::Android
if %AndroidProjectContain% EQU 1 (
md "%RamDiskAndroidPath%\bin"
md "%RamDiskAndroidPath%\obj"
md "%RamDiskAndroidPath%\buildAPK"
)

::iOS
if %iOSProjectContain% EQU 1 (
md "%RamDiskiOSPath%\bin"
md "%RamDiskiOSPath%\obj"
)

pause