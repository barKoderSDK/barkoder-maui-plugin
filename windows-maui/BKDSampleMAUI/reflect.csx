using System;
using System.Reflection;
var asm = Assembly.LoadFrom(@"C:\Github\windowsMAUI\nuget_cache\barkodernet\1.7.1\lib\net8.0\BarkoderNET.dll");
foreach (var t in asm.GetTypes())
{
    if (t.Name == "Config" || t.Name == "BKGlobalOption")
    {
        Console.WriteLine($"=== {t.FullName} (IsEnum={t.IsEnum}) ===");
        if (t.IsEnum)
        {
            foreach (var n in Enum.GetNames(t)) Console.WriteLine($"  {n} = {(int)Enum.Parse(t, n)}");
        }
        else
        {
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                Console.WriteLine($"  prop: {p.PropertyType.Name} {p.Name}");
            foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                if (!m.IsSpecialName) Console.WriteLine($"  method: {m.Name}({string.Join(",", Array.ConvertAll(m.GetParameters(), p2 => p2.ParameterType.Name))})");
        }
    }
}
