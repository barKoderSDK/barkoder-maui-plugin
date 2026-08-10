Set-Location "c:\Users\User\Desktop\MAUI multiplatform\barkoder-maui5\BarkoderSample"
$output = dotnet build -f net8.0-windows10.0.19041.0 2>&1 | Out-String
$lines = $output -split "`n"
foreach ($line in $lines) {
    if ($line -match "error CS|FAILED|succeeded") {
        Write-Host $line.Trim()
    }
}
