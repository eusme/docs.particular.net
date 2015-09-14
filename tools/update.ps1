Clear-Host
$docsDiretory = (get-item $PSScriptRoot ).parent.FullName
$toolsDiretory = (get-item $PSScriptRoot ).FullName
$sharedSettings =$toolsDiretory +"\Shared.DotSettings"
$nuget =$toolsDiretory +"\nuget.exe"
$layeredSettings =$toolsDiretory +"\Layered.DotSettings"
$packagesDirectory  =$docsDiretory +"\packages"

#$snippetsFile =$docsDiretory +"\Snippets\NSBDoco.sln.DotSettings"

$solutions = Get-ChildItem $docsDiretory -Filter *.sln -Recurse
foreach ($solution in $solutions) 
{
	$solution.FullName
	Set-Location $solution.DirectoryName
	$relativePath = Get-Item $sharedSettings | Resolve-Path -Relative
	$relativePath 
	$targetFile = $solution.FullName + ".DotSettings"
	Remove-Item $targetFile
	(Get-Content $layeredSettings) | 
	Foreach-Object 	{ $_ -replace 'SharedDotSettings', $relativePath }  | 
	Out-File $targetFile
	
	Set-Location $docsDiretory
	& $nuget restore $solution.FullName -packagesDirectory $packagesDirectory | Out-Null
	& $nuget update  $solution.FullName -safe -repositoryPath $packagesDirectory
} 