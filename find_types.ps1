$bc = "E:\Users\Documents\GitHub\CryptoTool\WpfApp1\bin\Debug\net10.0-windows\BouncyCastle.Cryptography.dll"
$asm = [System.Reflection.Assembly]::LoadFrom($bc)
$asm.GetTypes() | Where-Object { $_.Name -like "*NamedCurve*" } | ForEach-Object { Write-Output $_.FullName }
Write-Output "----"
$asm.GetTypes() | Where-Object { $_.Namespace -like "*Asn1*" -and $_.Name -like "*X9*" } | Select-Object -First 20 | ForEach-Object { Write-Output $_.FullName }
