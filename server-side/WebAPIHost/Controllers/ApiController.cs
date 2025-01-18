using Microsoft.AspNetCore.Mvc;
using WebAPIHost.Exceptions;

namespace WebAPIHost.Controllers;

[ApiController]
[Route("/Api/[controller]")]
public class ApiController() : ControllerBase
{
    private readonly Serilog.ILogger _logger = Serilog.Log.ForContext<ApiController>();

    protected IActionResult Execute(Func<IActionResult> func)
    {
        try
        {
            return func();
        }
        catch (ServiceException ex)
        {
            _logger.Debug(
                ex,
                "Expected \"{name}\" occurred when processing request",
                nameof(ServiceException)
            );
            return ex.ToObjectResult();
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Unexpected \"{name}\" occurred when processing request",
                nameof(Exception)
            );
            return StatusCode(500);
        }
    }

    protected async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> func)
    {
        try
        {
            return await func();
        }
        catch (ServiceException ex)
        {
            _logger.Debug(
                ex,
                "Expected \"{name}\" occurred when processing request",
                nameof(ServiceException)
            );
            return ex.ToObjectResult();
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Unexpected \"{name}\" occurred when processing request",
                nameof(Exception)
            );
            return StatusCode(500);
        }
    }
}
