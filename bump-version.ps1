# Version Bump Script for CryptoTool
# This script helps update version numbers across all project files

param(
    [Parameter(Mandatory=$true)]
    [string]$NewVersion,
    
    [Parameter(Mandatory=$false)]
    [switch]$CreateTag = $false,
    
    [Parameter(Mandatory=$false)]
    [switch]$PushTag = $false
)

# Validate version format
if ($NewVersion -notmatch '^\d+\.\d+\.\d+$') {
    Write-Error "Version must be in format X.Y.Z (e.g., 1.2.3)"
    exit 1
}

Write-Host "Updating version to: $NewVersion" -ForegroundColor Green

# Function to update project version
function Update-ProjectVersion {
    param (
        [string]$ProjectPath,
        [string]$Version
    )
    
    if (Test-Path $ProjectPath) {
        try {
            [xml]$xml = Get-Content $ProjectPath
            
            # Find the PropertyGroup that contains Version info
            $versionPropertyGroup = $xml.Project.PropertyGroup | Where-Object { $_.Version -ne $null }
            if ($versionPropertyGroup -eq $null) {
                # Create a new PropertyGroup for version info
                $newPropertyGroup = $xml.CreateElement("PropertyGroup")
                $xml.Project.AppendChild($newPropertyGroup) | Out-Null
                $versionPropertyGroup = $newPropertyGroup
            }
            
            # Update or create version elements
            if ($versionPropertyGroup.Version -eq $null) {
                $versionElement = $xml.CreateElement("Version")
                $versionPropertyGroup.AppendChild($versionElement) | Out-Null
            }
            $versionPropertyGroup.Version = $Version
            
            if ($versionPropertyGroup.AssemblyVersion -eq $null) {
                $assemblyVersionElement = $xml.CreateElement("AssemblyVersion")
                $versionPropertyGroup.AppendChild($assemblyVersionElement) | Out-Null
            }
            $versionPropertyGroup.AssemblyVersion = "$Version.0"
            
            if ($versionPropertyGroup.FileVersion -eq $null) {
                $fileVersionElement = $xml.CreateElement("FileVersion")
                $versionPropertyGroup.AppendChild($fileVersionElement) | Out-Null
            }
            $versionPropertyGroup.FileVersion = "$Version.0"
            
            $xml.Save($ProjectPath)
            Write-Host "Updated $ProjectPath" -ForegroundColor Yellow
        }
        catch {
            Write-Warning "Failed to update $ProjectPath : $($_.Exception.Message)"
        }
    } else {
        Write-Warning "Could not find $ProjectPath"
    }
}

# Update all project files
Update-ProjectVersion "CryptoTool.Win\CryptoTool.Win.csproj" $NewVersion
Update-ProjectVersion "CryptoTool.Algorithm\CryptoTool.Algorithm.csproj" $NewVersion

# Clean previous build outputs
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean CryptoTool.sln --configuration Release --verbosity quiet

# Test build to ensure everything compiles
Write-Host "Testing build..." -ForegroundColor Yellow
dotnet build CryptoTool.sln --configuration Release --verbosity quiet
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed after version update. Please check the changes."
    exit 1
}
Write-Host "Build successful!" -ForegroundColor Green

# Create git tag if requested
if ($CreateTag) {
    $tagName = "v$NewVersion"
    
    # Check if tag already exists
    $existingTag = git tag -l $tagName
    if ($existingTag) {
        Write-Warning "Tag $tagName already exists!"
        $confirm = Read-Host "Do you want to delete the existing tag and create a new one? (y/N)"
        if ($confirm -eq 'y' -or $confirm -eq 'Y') {
            git tag -d $tagName
            if ($PushTag) {
                git push origin :refs/tags/$tagName
            }
        } else {
            Write-Host "Skipping tag creation" -ForegroundColor Yellow
            exit 0
        }
    }
    
    # Commit version changes
    git add *.csproj */*.csproj
    git commit -m "Bump version to $NewVersion"
    
    # Create the tag
    git tag -a $tagName -m "Release version $NewVersion"
    Write-Host "Created git tag: $tagName" -ForegroundColor Green
    
    # Push tag if requested
    if ($PushTag) {
        git push origin master
        git push origin $tagName
        Write-Host "Pushed changes and tag to origin: $tagName" -ForegroundColor Green
        Write-Host "GitHub Actions will now build and create a release!" -ForegroundColor Cyan
    } else {
        Write-Host "To push the changes and tag, run:" -ForegroundColor Yellow
        Write-Host "  git push origin master" -ForegroundColor Cyan
        Write-Host "  git push origin $tagName" -ForegroundColor Cyan
    }
}

Write-Host ""
Write-Host "Version update completed successfully!" -ForegroundColor Green
Write-Host "New version: $NewVersion" -ForegroundColor Cyan

if (!$CreateTag) {
    Write-Host ""
    Write-Host "To create a release tag, run:" -ForegroundColor Yellow
    Write-Host "  .\bump-version.ps1 -NewVersion $NewVersion -CreateTag -PushTag" -ForegroundColor Cyan
    Write-Host "Or manually:" -ForegroundColor Yellow
    Write-Host "  git add . && git commit -m 'Bump version to $NewVersion'" -ForegroundColor Cyan
    Write-Host "  git tag v$NewVersion" -ForegroundColor Cyan
    Write-Host "  git push origin master && git push origin v$NewVersion" -ForegroundColor Cyan
}
