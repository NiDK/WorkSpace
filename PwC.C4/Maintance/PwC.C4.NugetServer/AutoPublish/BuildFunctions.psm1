function Update-SourceVersion
{
  Param
  (
    [string]$SrcPath,
    [string]$assemblyVersion, 
    [string]$fileAssemblyVersion
  )
     
    $buildNumber = $env:TF_BUILD_BUILDNUMBER
    if ($buildNumber -eq $null)
    {
        $buildIncrementalNumber = 0
    }
    else
    {
        $splitted = $buildNumber.Split('.')
        $buildIncrementalNumber = $splitted[$splitted.Length - 1]
    }
 
    if ($fileAssemblyVersion -eq "")
    {
        $fileAssemblyVersion = $assemblyVersion
    }
     
    Write-Host "Executing Update-SourceVersion in path $SrcPath, Version is $assemblyVersion and File Version is $fileAssemblyVersion"
  
 
    $AllVersionFiles = Get-ChildItem $SrcPath AssemblyInfo.cs -recurse
 
     
    $jdate = Get-JulianDate
    $assemblyVersion = $assemblyVersion.Replace("J", $jdate).Replace("B", $buildIncrementalNumber)
    $fileAssemblyVersion = $fileAssemblyVersion.Replace("J", $jdate).Replace("B", $buildIncrementalNumber)
     
    Write-Host "Transformed Version is $assemblyVersion and Transformed File Version is $fileAssemblyVersion"
  
 
    foreach ($file in $AllVersionFiles) 
    { 
        Write-Host "Modifying file " + $file.FullName
        #save the file for restore
        $backFile = $file.FullName + "._ORI"
        $tempFile = $file.FullName + ".tmp"
        Copy-Item $file.FullName $backFile
        #now load all content of the original file and rewrite modified to the same file
        Get-Content $file.FullName |
        %{$_ -replace 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', "AssemblyVersion(""$assemblyVersion"")" } |
        %{$_ -replace 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', "AssemblyFileVersion(""$fileAssemblyVersion"")" }  > $tempFile
        Move-Item $tempFile $file.FullName -force
    }
  
}