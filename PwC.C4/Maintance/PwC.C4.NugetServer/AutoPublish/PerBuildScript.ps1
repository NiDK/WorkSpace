function Publish-NugetPackage
{
  Param
  (
    [string]$SrcPath,
    [string]$NugetPath,
    [string]$PackageVersion, 
    [string]$NugetServer,
    [string]$NugetServerPassword
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
     
    Write-Host "Executing Publish-NugetPackage in path $SrcPath, PackageVersion is $PackageVersion"
     
    $jdate = Get-JulianDate
    $PackageVersion = $PackageVersion.Replace("J", $jdate).Replace("B", $buildIncrementalNumber)
     
    Write-Host "Transformed PackageVersion is $PackageVersion "
  
    $AllNuspecFiles = Get-ChildItem $SrcPath\*.nuspec
   
    #Remove all previous packed packages in the directory
      
    $AllNugetPackageFiles = Get-ChildItem $SrcPath\*.nupkg
   
    foreach ($file in $AllNugetPackageFiles)
    { 
        Remove-Item $file
    }
 
    foreach ($file in $AllNuspecFiles)
    { 
        Write-Host "Modifying file " + $file.FullName
        #save the file for restore
        $backFile = $file.FullName + "._ORI"
        $tempFile = $file.FullName + ".tmp"
        Copy-Item $file.FullName $backFile -Force
        #now load all content of the original file and rewrite modified to the same file
        Get-Content $file.FullName |
        %{$_ -replace '<version>[0-9]+(\.([0-9]+|\*)){1,3}</version>', "<version>$PackageVersion</version>" } > $tempFile
        Move-Item $tempFile $file.FullName -force
 
        #Create the .nupkg from the nuspec file
        $ps = new-object System.Diagnostics.Process
        $ps.StartInfo.Filename = "$NugetPath\nuget.exe"
        $ps.StartInfo.Arguments = "pack `"$file`""
        $ps.StartInfo.WorkingDirectory = $file.Directory.FullName
        $ps.StartInfo.RedirectStandardOutput = $True
        $ps.StartInfo.RedirectStandardError = $True
        $ps.StartInfo.UseShellExecute = $false
        $ps.start()
        if(!$ps.WaitForExit(30000)) 
        {
            $ps.Kill()
        }
        [string] $Out = $ps.StandardOutput.ReadToEnd();
        [string] $ErrOut = $ps.StandardError.ReadToEnd();
        Write-Host "Nuget pack Output of commandline " + $ps.StartInfo.Filename + " " + $ps.StartInfo.Arguments
        Write-Host $Out
        if ($ErrOut -ne "") 
        {
            Write-Error "Nuget pack Errors"
            Write-Error $ErrOut
        }
        #Restore original file
        #Move-Item $backFile $file -Force
    }
     
    $AllNugetPackageFiles = Get-ChildItem $SrcPath\*.nupkg
   
    foreach ($file in $AllNugetPackageFiles)
    { 
        #Create the .nupkg from the nuspec file
        $ps = new-object System.Diagnostics.Process
        $ps.StartInfo.Filename = "$NugetPath\nuget.exe"
        $ps.StartInfo.Arguments = "push `"$file`" -s $NugetServer $NugetServerPassword"
        $ps.StartInfo.WorkingDirectory = $file.Directory.FullName
        $ps.StartInfo.RedirectStandardOutput = $True
        $ps.StartInfo.RedirectStandardError = $True
        $ps.StartInfo.UseShellExecute = $false
        $ps.start()
        if(!$ps.WaitForExit(30000)) 
        {
            $ps.Kill()
        }
        [string] $Out = $ps.StandardOutput.ReadToEnd();
        [string] $ErrOut = $ps.StandardError.ReadToEnd();
        Write-Host "Nuget push Output of commandline " + $ps.StartInfo.Filename + " " + $ps.StartInfo.Arguments
        Write-Host $Out
        if ($ErrOut -ne "") 
        {
            Write-Error "Nuget push Errors"
            Write-Error $ErrOut
        }
 
    }
}