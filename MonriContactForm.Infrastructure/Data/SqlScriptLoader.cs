using System.Reflection;

namespace MonriContactForm.Infrastructure.Data;

public static class SqlScriptLoader
{
    public static string LoadScript(string folder, string scriptName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = $"{assembly.GetName().Name}.Data.SqlScripts.{folder}.{scriptName}.sql" ;

        using var stream = assembly.GetManifestResourceStream(resourcePath) ?? throw new FileNotFoundException($"SQL script not found: {resourcePath}"); ;
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
