$ErrorActionPreference = 'Stop'

$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$solutionPath = Join-Path $projectRoot 'Student Manager.sln'
$installerScript = Join-Path $projectRoot 'installer\StudentManager.iss'
$msbuildPath = 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe'
$innoCompiler = 'C:\Program Files (x86)\Inno Setup 6\ISCC.exe'

if (-not (Test-Path -LiteralPath $msbuildPath)) {
    throw "MSBuild was not found at: $msbuildPath"
}

if (-not (Test-Path -LiteralPath $innoCompiler)) {
    throw "Inno Setup was not found at: $innoCompiler"
}

& $msbuildPath $solutionPath /t:Restore,Rebuild /p:Configuration=Release /p:Platform='Any CPU' /p:SignManifests=false /m /v:minimal
if ($LASTEXITCODE -ne 0) {
    throw "The Release build failed with exit code $LASTEXITCODE."
}

& $innoCompiler $installerScript
if ($LASTEXITCODE -ne 0) {
    throw "The installer build failed with exit code $LASTEXITCODE."
}

$installerPath = Join-Path $projectRoot 'dist\Student Manager Setup.exe'
if (-not (Test-Path -LiteralPath $installerPath)) {
    throw "The installer compiler finished, but the expected file was not created: $installerPath"
}

Write-Host "Installer created: $installerPath"
