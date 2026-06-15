using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace UnitConversion.API.Services;

internal record UnitDefinition(string Name, string Category, double ToBaseFactor, double Offset = 0);

public class UnitConversionService : IUnitConversionService
{
    private readonly Dictionary<string, UnitDefinition> _units;

    public UnitConversionService()
    {
        _units = CreateDefaults();
    }

    public UnitConversionService(IWebHostEnvironment? env)
    {
        // env not currently used but kept for test compatibility
        _units = CreateDefaults();
    }

    private static Dictionary<string, UnitDefinition> CreateDefaults()
    {
        return new Dictionary<string, UnitDefinition>(StringComparer.OrdinalIgnoreCase)
        {
            // Length
            { "meter", new UnitDefinition("meter", "length", 1.0) },
            { "meters", new UnitDefinition("meter", "length", 1.0) },
            { "m", new UnitDefinition("meter", "length", 1.0) },
            { "kilometer", new UnitDefinition("kilometer", "length", 1000.0) },
            { "km", new UnitDefinition("kilometer", "length", 1000.0) },
            { "centimeter", new UnitDefinition("centimeter", "length", 0.01) },
            { "cm", new UnitDefinition("centimeter", "length", 0.01) },
            { "millimeter", new UnitDefinition("millimeter", "length", 0.001) },
            { "mm", new UnitDefinition("millimeter", "length", 0.001) },
            { "foot", new UnitDefinition("foot", "length", 0.3048) },
            { "feet", new UnitDefinition("foot", "length", 0.3048) },
            { "ft", new UnitDefinition("foot", "length", 0.3048) },
            { "inch", new UnitDefinition("inch", "length", 0.0254) },
            { "in", new UnitDefinition("inch", "length", 0.0254) },
            { "mile", new UnitDefinition("mile", "length", 1609.344) },
            { "mi", new UnitDefinition("mile", "length", 1609.344) },

            // Weight
            { "kilogram", new UnitDefinition("kilogram", "weight", 1.0) },
            { "kg", new UnitDefinition("kilogram", "weight", 1.0) },
            { "gram", new UnitDefinition("gram", "weight", 0.001) },
            { "g", new UnitDefinition("gram", "weight", 0.001) },
            { "milligram", new UnitDefinition("milligram", "weight", 0.000001) },
            { "mg", new UnitDefinition("milligram", "weight", 0.000001) },
            { "pound", new UnitDefinition("pound", "weight", 0.45359237) },
            { "lb", new UnitDefinition("pound", "weight", 0.45359237) },
            { "ounce", new UnitDefinition("ounce", "weight", 0.028349523125) },
            { "oz", new UnitDefinition("ounce", "weight", 0.028349523125) },

            // Temperature
            { "celsius", new UnitDefinition("celsius", "temperature", 1.0) },
            { "c", new UnitDefinition("celsius", "temperature", 1.0) },
            { "fahrenheit", new UnitDefinition("fahrenheit", "temperature", 1.0) },
            { "f", new UnitDefinition("fahrenheit", "temperature", 1.0) },
            { "kelvin", new UnitDefinition("kelvin", "temperature", 1.0) },
            { "k", new UnitDefinition("kelvin", "temperature", 1.0) }
        };
    }

    public ConversionResult? Convert(string from, string to, double value)
    {
        if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
            return null;

        if (!_units.TryGetValue(from, out var fromDef) || !_units.TryGetValue(to, out var toDef))
            return null;

        if (!string.Equals(fromDef.Category, toDef.Category, StringComparison.OrdinalIgnoreCase))
            return null;

        double result;
        if (string.Equals(fromDef.Category, "temperature", StringComparison.OrdinalIgnoreCase))
        {
            double valueInC = fromDef.Name.ToLowerInvariant() switch
            {
                "celsius" => value,
                "fahrenheit" => (value - 32) * 5.0 / 9.0,
                "kelvin" => value - 273.15,
                _ => throw new InvalidOperationException("Unknown temperature unit")
            };

            result = toDef.Name.ToLowerInvariant() switch
            {
                "celsius" => valueInC,
                "fahrenheit" => (valueInC * 9.0 / 5.0) + 32,
                "kelvin" => valueInC + 273.15,
                _ => throw new InvalidOperationException("Unknown temperature unit")
            };
        }
        else
        {
            double valueInBase = value * fromDef.ToBaseFactor;
            result = valueInBase / toDef.ToBaseFactor;
        }

        return new ConversionResult(from, to, Math.Round(value, 8), Math.Round(result, 8), fromDef.Category);
    }
}
