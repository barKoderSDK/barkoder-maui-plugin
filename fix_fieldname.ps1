$file = "c:\Users\User\Desktop\MAUI multiplatform\barkoder-maui5\Plugin.Maui.Barkoder\Handlers\BarkoderViewHandler.Windows.cs"
$content = Get-Content $file -Raw

# Replace field name throughout
$content = $content.Replace('_barkoderView', '_winBarkoderView')

Set-Content $file $content -NoNewline
Write-Host "Done"
