$ErrorActionPreference = "Stop"

$assemblyVersion = "2.0.0"
$assemblyFileVersion = "2.0.0.19028"
$assemblyInformationalVersion = "2.0.0-rc01"

Write-Host "Updating project versions..."

$assemblyVersionText = "<AssemblyVersion>$($assemblyVersion)</AssemblyVersion>"
$assemblyFileVersionText = "<FileVersion>$($assemblyFileVersion)</FileVersion>"
$assemblyInformationalVersionText = "<VersionPrefix>$($assemblyInformationalVersion)</VersionPrefix>"

Get-ChildItem -Path ".." -Recurse | 
Where-Object {$_.FullName -like "*src*" -and $_.Name -like "*.csproj"} | 
ForEach-Object {

    $csprojFilePath = $_.FullName

    Write-Host "Changing versions in file $($csprojFilePath)..."
    $csprojContent = Get-Content $csprojFilePath -Raw -Encoding UTF8

    $csprojContent = $csprojContent -replace '<AssemblyVersion>[0-9.]+</AssemblyVersion>', $assemblyVersionText
    $csprojContent = $csprojContent -replace '<FileVersion>[0-9.]+</FileVersion>', $assemblyFileVersionText 
    $csprojContent = $csprojContent -replace '<VersionPrefix>[0-9a-zA-Z.-]+</VersionPrefix>', $assemblyInformationalVersionText

    $csprojContent | Set-Content $csprojFilePath -Encoding UTF8
}