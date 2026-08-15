$ErrorActionPreference = "Stop"
$bc = "E:\Users\Documents\GitHub\CryptoTool\WpfApp1\bin\Debug\net10.0-windows\BouncyCastle.Cryptography.dll"
$dll = "E:\Users\Documents\GitHub\CryptoTool\WpfApp1\bin\Debug\net10.0-windows\WpfApp1.dll"

$asm = [System.Reflection.Assembly]::LoadFrom($bc)
$t = $asm.GetType("Org.BouncyCastle.Asn1.X9.ECNamedCurveTable")
if ($t -eq $null) { Write-Output "BC-TYPE-MISSING"; exit 1 }

foreach ($name in @("K-163","K-233","K-283","K-409","K-571","P-256","B-571")) {
    $r = $t.GetMethod("GetByName").Invoke($null, @($name))
    if ($r -eq $null) { Write-Output "BC-MISSING: $name" } else { Write-Output "BC-OK: $name" }
}

$asm2 = [System.Reflection.Assembly]::LoadFrom($dll)
$t2 = $asm2.GetType("CryptoTool.Win.Helpers.EcdsaCurveNames")
if ($t2 -eq $null) { Write-Output "APP-TYPE-MISSING"; exit 1 }

try {
    $m = $t2.GetMethod("GetAllCurvesByCategory")
    $cat = $m.Invoke($null, $null)
    $keys = $cat.Keys
    Write-Output "STATIC-CTOR-OK, category count = $($keys.Count)"
    foreach ($k in $keys) { Write-Output "  CAT: $k" }
    if ($cat.ContainsKey("NIST Koblitz Curves")) {
        $list = $cat["NIST Koblitz Curves"].Curves
        Write-Output "KOBLITZ-CAT: OK, $($list.Count) curves"
    } else {
        Write-Output "KOBLITZ-CAT: MISSING"
    }
    $m2 = $t2.GetMethod("GetCategoryByCurveKey")
    $c = $m2.Invoke($null, @("K-571"))
    Write-Output "MAP K-571 -> $c"
} catch {
    Write-Output "ERROR: $($_.Exception.InnerException.Message)"
}
