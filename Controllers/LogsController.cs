using ClaculatorAPI.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Diagnostics;

namespace ClaculatorAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private static readonly Logger RequestLogger = LogManager.GetLogger("request-logger");

        [HttpGet("level")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetLogLevel([FromQuery(Name = "logger-name")] string loggerName)
        {
            Stopwatch timer = Stopwatch.StartNew();
            Utils.requestCounter++;
            try
            {
                return $"Current Logger Level is: {Utils.getLoggerCurrentLevel(loggerName)}";
            }
            catch (Exception ex)
            {
                Response.StatusCode = 409;
                return $"Get Method Failed!" + ex.Message;
            }
            finally
            {
                RequestLogger.Info($"Incoming request | #{Utils.requestCounter} | resource: {"/logs/level"} | HTTP Verb GET | request #{Utils.requestCounter}");
                timer.Stop();
                TimeSpan timeSpan = timer.Elapsed;
                RequestLogger.Debug($"request #{Utils.requestCounter} duration: {Utils.timeSpan.Milliseconds}ms | request #{Utils.requestCounter}");
            }
        }

        [HttpPut("level")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string SetLogLevel([FromQuery(Name = "logger-name")] string loggerName, [FromQuery(Name = "logger-level")] string loggerLevel)
        {
            Stopwatch timer = Stopwatch.StartNew();
            Utils.requestCounter++;
            try
            {
                return $"Current Logger Level is: {Utils.setLoggerLevel(loggerName , loggerLevel)}";
            }
            catch (Exception ex)
            {
                Response.StatusCode = 409;
                return $"Put Method Failed!" + ex.Message;
            }
            finally
            {
                RequestLogger.Info($"Incoming request | #{Utils.requestCounter} | resource: {"/logs/level"} | HTTP Verb PUT | request #{Utils.requestCounter}");
                timer.Stop();
                TimeSpan timeSpan = timer.Elapsed;
                RequestLogger.Debug($"request #{Utils.requestCounter} duration: {Utils.timeSpan.Milliseconds}ms | request #{Utils.requestCounter}");
            }
        }

    }
}
