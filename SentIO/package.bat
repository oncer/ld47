Pushd "%~dp0"
rd /S /Q SentIO\bin
dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false /p:PublishTrimmed=false /p:PublishSingleFile=true --self-contained
cd SentIO\bin\Release\netcoreapp3.1\win-x64
xcopy interface.lock publish\
del publish\SentIO.pdb
rename publish SentIO
"C:\Program Files\7-Zip\7z.exe" a SentIO.zip SentIO
copy SentIO.zip ..\..\..\..\..
pause
