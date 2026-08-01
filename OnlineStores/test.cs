using System;
using System.Reflection;
using Microsoft.OpenApi.Models;

public class Program {
    public static void Main() {
        foreach (var prop in typeof(OpenApiSecurityScheme).GetProperties()) {
            Console.WriteLine(prop.Name);
        }
    }
}
