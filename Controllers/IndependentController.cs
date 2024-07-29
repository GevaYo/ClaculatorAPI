using ClaculatorAPI.Logic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using System.Diagnostics;
using System.IO;

namespace ClaculatorAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class IndependentController : ControllerBase
    {
        private static readonly Logger IndependentLogger = LogManager.GetLogger("independent-logger");
        private static readonly Logger RequestLogger = LogManager.GetLogger("request-logger");

        // POST <IndependentController>
        [HttpPost("calculate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string Post([FromBody] Independent i_Independent)
        {
            Utils.timer = Stopwatch.StartNew();
            Utils.requestCounter++;
            int result;
            try
            {
                i_Independent.validateInputParameters();
                string operation = i_Independent.Operation.ToLower();
                result = Utils.calculate(operation, i_Independent.Arguments);
                IndependentLogger.Info($"Performing operation {operation}. Result is {result} | request #{Utils.requestCounter}");
                IndependentLogger.Debug($"Performing operation: {operation}({string.Join<int>(",", i_Independent.Arguments)}) = {result} | request #{Utils.requestCounter}");

                return JsonConvert.SerializeObject(new Dictionary<string, int>() { ["result"] = result});
            }
            catch (Exception ex)
            {
                Response.StatusCode = 409;
                IndependentLogger.Error($"Server encountered an error ! message:{ex.Message} | request #{Utils.requestCounter}");
                return JsonConvert.SerializeObject(new Dictionary<string, string>() { ["error-message"] = ex.Message });
            }
            finally
            {
                RequestLogger.Info($"Incoming request | #{Utils.requestCounter} | resource: {"/independent/calculate"} | HTTP Verb POST | request #{Utils.requestCounter}");
                Utils.timer.Stop();
                Utils.timeSpan = Utils.timer.Elapsed;
                RequestLogger.Debug($"request #{Utils.requestCounter} duration: {Utils.timeSpan.Milliseconds}ms | request #{Utils.requestCounter}");
            }
        }
    }
}
