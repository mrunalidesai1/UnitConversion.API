namespace UnitConversion.API.Services;

public record ConversionResult(string From, string To, double Input, double Output, string Category);

public interface IUnitConversionService
{
    ConversionResult? Convert(string from, string to, double value);
}
