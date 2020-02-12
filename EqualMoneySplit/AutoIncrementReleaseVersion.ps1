##############################################################################
##############################################################################
##     _  _   _ _____ ___     ___ _  _  ___ ___ ___ __  __ ___ _  _ _____   ##
##    /_\| | | |_   _/ _ \ __|_ _| \| |/ __| _ \ __|  \/  | __| \| |_   _|  ##
##   / _ \ |_| | | || (_) |___| || .` | (__|   / _|| |\/| | _|| .` | | |    ##
##  /_/ \_\___/  |_| \___/   |___|_|\_|\___|_|_\___|_|  |_|___|_|\_| |_|    ##
##                                                                          ##
##############################################################################
##############################################################################

# Declare the path to the manifest
$PathToManifest =  $PSScriptRoot + "\manifest.json"

# Load the manifest.json file as a JSON object 
$Manifest = Get-Content $PathToManifest | ConvertFrom-Json

# Get the old minor build version
[String]$OldBuildVersion = $Manifest.Version
$BuildVersionArray = @($OldBuildVersion.Split('.'))

# Increment the last build version number by 1
[Int]$NewMinorBuildNumber = $BuildVersionArray[-1]
$BuildVersionArray[-1] = $NewMinorBuildNumber + 1

# Overwrite the version number with the new one we just wrote
$Manifest.Version = $BuildVersionArray -Join "."

# Output this object back into the original manifest
$Manifest | ConvertTo-Json | Set-Content $PathToManifest