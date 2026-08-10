$file = "c:\Users\User\Desktop\MAUI multiplatform\barkoder-maui5\Plugin.Maui.Barkoder\Handlers\BarkoderViewHandler.Windows.cs"
$content = Get-Content $file -Raw
$content = $content.Replace('using Barkoder.Windows', 'using global::Barkoder.Windows')
$content = $content.Replace('= Barkoder.Windows', '= global::Barkoder.Windows')
$content = $content.Replace('using WinDecoderType = WinDecoderType;', 'using WinDecoderType = global::Barkoder.Windows.Models.DecoderType;')
Set-Content $file $content -NoNewline
Write-Host "Done"
