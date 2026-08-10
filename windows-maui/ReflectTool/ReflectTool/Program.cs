using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

var dllPath = @"C:\Github\windowsMAUI\windows-maui\BKDSampleMAUI\bin\Debug\net8.0-windows10.0.19041.0\win10-x64\BarkoderNET.OCR.dll";
var ctx = new AssemblyLoadContext("test", true);
var asm = ctx.LoadFromAssemblyPath(dllPath);

var types = asm.GetTypes();
Console.WriteLine($"Total types: {types.Length}\n");

// Show ALL types
foreach (var t in types.OrderBy(t => t.FullName))
{
    Console.WriteLine($"\n=== {t.FullName} (IsEnum={t.IsEnum}) ===");
    
    if (t.IsEnum)
    {
        foreach (var n in Enum.GetNames(t))
            Console.WriteLine($"  {n} = {Convert.ToInt32(Enum.Parse(t, n))}");
        continue;
    }
    
    foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        Console.WriteLine($"  prop: {p.PropertyType} {p.Name}");
    
    foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
    {
        if (m.IsSpecialName) continue;
        var pars = string.Join(", ", m.GetParameters().Select(p2 => $"{p2.ParameterType.Name} {p2.Name}"));
        Console.WriteLine($"  method: {m.ReturnType} {m.Name}({pars})");
    }
    
    // Also static methods
    foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
    {
        if (m.IsSpecialName) continue;
        var pars = string.Join(", ", m.GetParameters().Select(p2 => $"{p2.ParameterType.Name} {p2.Name}"));
        Console.WriteLine($"  static: {m.ReturnType} {m.Name}({pars})");
    }
}

ctx.Unload();
