@echo off
:: Author      : Howe
:: Version     : 1.4
:: CreateTime  : 2019-10-16 11:07:46
:: Description : RAMDisk link 创建脚本, 修改 Step 1 与 2 参数, 保存后执行

:: ******* Step 1 设置内存盘的路径 *******
set RamDiskRootPath="X:\Cache_SC\XamarinAndroidVueProjectSeed"

:: ******* Step 2 设置包含的平台 *******
set AndroidProjectContain=1
set iOSProjectContain=0

echo 删除本项目的bin和obj文件夹
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

echo 创建mklink
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

echo 创建RAMDISK的bin和obj文件夹
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