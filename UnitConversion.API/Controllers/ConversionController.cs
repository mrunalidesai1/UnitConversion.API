using Microsoft.AspNetCore.Mvc;
using UnitConversion.API.Services;

namespace UnitConversion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversionController : ControllerBase
{
    private readonly IUnitConversionService _service;

    public ConversionController(IUnitConversionService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string from, [FromQuery] string to, [FromQuery] double value)
    {
        if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
            return BadRequest("'from' and 'to' query parameters are required.");

        var result = _service.Convert(from, to, value);
        if (result == null)
            return BadRequest("Unable to convert between the specified units.");

        return Ok(result);
    }

    public record ConversionRequest(string From, string To, double Value);

    [HttpPost]
    public IActionResult Post([FromBody] ConversionRequest req)
    {
        if (req == null)
            return BadRequest("Request body is required.");

        var result = _service.Convert(req.From, req.To, req.Value);
        if (result == null)
            return BadRequest("Unable to convert between the specified units.");

        return Ok(result);
    }
}
