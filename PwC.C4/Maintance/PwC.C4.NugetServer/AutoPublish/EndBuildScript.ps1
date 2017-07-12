Param
(
[string] $PackageVersion = "1.0.J.B",
[string] $NugetServer = "http://alkampfernuget.azurewebsites.net/",
[string] $NugetServerPassword = "This_is_my_password"
)
 
Write-Host "Running Pre Build Scripts"
 
$scriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
Import-Module $scriptRoot\BuildFunctions
 
Publish-NugetPackage $scriptRoot\..\..\bin $scriptRoot $PackageVersion $NugetServer $NugetServerPassword
