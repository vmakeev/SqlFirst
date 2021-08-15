@echo off

rem Строго БЕЗ слеша на конце
set sourceDir=%~dp0..\bin\Debug\net462
rem Строго СО слешем на конце
set dest=%~dp0..\bin\SqlFirst_Debug\

echo Source: %sourceDir%
echo Dest: %dest%

set binDir=%dest%bin
set thirdPartyDir=%binDir%\3rdParty
set systemDir=%binDir%\System

echo Creating directory...
rmdir "%dest%" /s /q
mkdir "%dest%"

echo Copying files...
xcopy /Y "%sourceDir%" "%dest%" /S /E > nul

echo Removing debug files...
rem Удаляем ненужные в продакшн-версии файлы
del "%dest%*.pdb" /F /Q
del "%dest%*.xml" /F /Q

rem Создаем папки для сортировки библиотек
mkdir "%binDir%"
mkdir "%thirdPartyDir%"
mkdir "%systemDir%"

echo Sorting assemblies...

move "%dest%SqlFirst.*.dll" "%binDir%" > nul

move "%dest%System.*.dll" "%systemDir%" > nul
move "%dest%Microsoft.*.dll" "%systemDir%" > nul
move "%dest%netstandard.dll" "%systemDir%" > nul

move "%dest%*.dll" "%thirdPartyDir%" > nul

echo Done!
pause
explorer "%dest%"
@echo on