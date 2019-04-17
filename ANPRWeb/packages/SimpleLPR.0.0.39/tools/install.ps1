param($installPath, $toolsPath, $package, $project)

function MarkDirectoryAsCopyToOutputRecursive($item)
{
    $item.ProjectItems | ForEach-Object { MarkFileASCopyToOutputDirectory($_) }
}

function MarkFileASCopyToOutputDirectory($item)
{
    Try
    {
        Write-Host Try set $item.Name
        $item.Properties.Item("CopyToOutputDirectory").Value = 2
    }
    Catch
    {
        Write-Host RecurseOn $item.Name
        MarkDirectoryAsCopyToOutputRecursive($item)
    }
}

function myRenameToNative($item)
{
    Rename-Item -path $item -newname "native"
}
# not working
#myRenameToNative($project.ProjectItems.Item("warelogic"))

# not working
#Rename-Item $project.ProjectItems.Item("nativeCV") "native"   

#Now mark everything in the a directory as "Copy to newer"

MarkDirectoryAsCopyToOutputRecursive($project.ProjectItems.Item("warelogic"))

MarkFileASCopyToOutputDirectory($project.ProjectItems.Item("LicensePlate.bmp"))

#Write-Host $project

#Write-Host $project.ProjectItems.Item("nativee")
#Rename-Item $project.ProjectItems.Item("nativ") "native"

#MarkDirectoryAsCopyToOutputRecursive($project.ProjectItems.Item("OpenCV"))

#Open CV files to Output Directory
#MarkFileASCopyToOutputDirectory($project.ProjectItems.Item("cublas32_55.dll"))
#MarkFileASCopyToOutputDirectory($project.ProjectItems.Item("cudart32_55.dll"))
