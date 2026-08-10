$file = "c:\Users\User\Desktop\MAUI multiplatform\barkoder-maui5\Plugin.Maui.Barkoder\Handlers\BarkoderViewHandler.Windows.cs"
$content = Get-Content $file -Raw

# Fix inline code references (not in using statements)
$content = $content.Replace('new Barkoder.Windows.BarkoderView()', 'new global::Barkoder.Windows.BarkoderView()')
$content = $content.Replace('await Barkoder.Windows.BarkoderView.ScanImage', 'await global::Barkoder.Windows.BarkoderView.ScanImage')
$content = $content.Replace('(Barkoder.Windows.Models.ScanningIndicatorAnimation)', '(global::Barkoder.Windows.Models.ScanningIndicatorAnimation)')

Set-Content $file $content -NoNewline
Write-Host "Done"
