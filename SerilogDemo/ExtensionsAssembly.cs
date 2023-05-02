using System.Reflection;

namespace SerilogDemo;

/// <summary>
/// Возвращает сборку метода, вызывавшего текущий исполняемый метод.
/// Returns the assembly of the method that called the current executing method.
/// </summary>
public static class ReturnsAssemblyMethodCurrent

{
public static string? GetEntryAssembly()
{
    var NameAssembly = Assembly.GetEntryAssembly().GetName().Name;
    //return NameAssembly +"-" + Guid.NewGuid().ToString().Substring(0,8);
    return NameAssembly;// +"-" + Guid.NewGuid().ToString().Substring(0,8);
}
}
