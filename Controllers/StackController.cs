using ClaculatorAPI.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;



namespace ClaculatorAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StackController : ControllerBase
    {
        private static readonly Logger StackLogger = LogManager.GetLogger("stack-logger");
        private static readonly Logger RequestLogger = LogManager.GetLogger("request-logger");
        

        [HttpGet("size")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetStackSize()
        {
            Utils.timer = Stopwatch.StartNew();
            Utils.requestCounter++;

            List<int> args = ArgsStack.Instance.getStackArgsInAList();
            int result = ArgsStack.Instance.Stack.Count;
            RequestLogger.Info($"Incoming request | #{Utils.requestCounter} | resource: {"/stack/size"} | HTTP Verb GET | request #{Utils.requestCounter}");
            StackLogger.Info($"Stack size is {result} | request #{Utils.requestCounter}");
            
            Utils.timer.Stop();
            Utils.timeSpan = Utils.timer.Elapsed;

            RequestLogger.Debug($"request #{Utils.requestCounter} duration: {Utils.timeSpan.Milliseconds}ms | request #{Utils.requestCounter}");
            StackLogger.Debug($"Stack content (first == top): [{string.Join<int>(", ", args)}] | request #{Utils.requestCounter}");

            return JsonConvert.SerializeObject(new Dictionary<string, int>() { ["result"] = result});
        }

        [HttpGet("operate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetCalculation([FromQuery(Name = "operation")] string operation)
        {
            Utils.timer = Stopwatch.StartNew();
            Utils.requestCounter++;
            try
            {
                int result, numOfArgsAfterCalc;
                ArgsStack.Instance.validateOperation(operation);
                List<int> argsToOperate = ArgsStack.Instance.popArgsBasedOnOperation();
                result = Utils.calculate(operation.ToLower(), argsToOperate);
                numOfArgsAfterCalc = ArgsStack.Instance.Stack.Count;
                StackLogger.Info($"Performing operation {operation}. Result is { result } | stack size: {numOfArgsAfterCalc} | request #{Utils.requestCounter}");
                StackLogger.Debug($"Performing operation: {operation}({string.Join<int>(",", argsToOperate)}) = {result} | request #{Utils.requestCounter}");

                return JsonConvert.SerializeObject(new Dictionary<string, int>() { ["result"] = result });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 409;
                StackLogger.Error($"Server encountered an error ! message:{ex.Message} | request #{Utils.requestCounter}");
                return JsonConvert.SerializeObject(new Dictionary<string, string>() { ["error-message"] = ex.Message });
            }
            finally
            {
                RequestLogger.Info($"Incoming request | #{Utils.requestCounter} | resource: {"/stack/operate"} | HTTP Verb GET | request #{Utils.requestCounter}");
                Utils.timer.Stop();
                Utils.timeSpan = Utils.timer.Elapsed;
                RequestLogger.Debug($"request #{Utils.requestCounter} duration: {Utils.timeSpan.Milliseconds}ms | request #{Utils.requestCounter}");
            }
        }

        [HttpPut("arguments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string Put([FromBody] Arguments args)
        {
            Utils.timer = Stopwatch.StartNew();
            Utils.requestCounter++;

            int numOfArgsBeforePut = ArgsStack.Instance.Stack.Count;
            ArgsStack.Instance.pushArgsToStack(args);
            int numOfArgsAfterPut = ArgsStack.Instance.Stack.Count;
            RequestLogger.Info($"Incoming request | #{Utils.requestCounter} | resource: {"/stack/arguments"} | HTTP Verb PUT | request #{Utils.requestCounter}");
            StackLogger.Info($"Adding total of {args.arguments.Count} argument(s) to the stack | Stack size: {numOfArgsAfterPut} | request #{Utils.requestCounter}");
            
            Utils.timer.Stop();
            Utils.timeSpan = Utils.timer.Elapsed;

            RequestLogger.Debug($"request #{Utils.requestCounter} duration: {Utils.timeSpan.Milliseconds}ms | request #{Utils.requestCounter}");
            StackLogger.Debug($"Adding arguments: {string.Join<int>(",", args.arguments)} | Stack size before {numOfArgsBeforePut} | stack size after {numOfArgsAfterPut} | request #{Utils.requestCounter}");
            int result = ArgsStack.Instance.Stack.Count;

            return JsonConvert.SerializeObject(new Dictionary<string, int>() { ["result"] = result });
        }

        [HttpDelete("arguments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string Delete([FromQuery(Name ="count")] int numToPop)
        {
            Utils.timer = Stopwatch.StartNew();
            Utils.requestCounter++;

            try
            {
                ArgsStack.Instance.popArgsBasedOnUsersChoice(numToPop);
                int result = ArgsStack.Instance.Stack.Count;
                StackLogger.Info($"Removing total {numToPop} argument(s) from the stack | Stack size: {result} | request #{Utils.requestCounter}");

                return JsonConvert.SerializeObject(new Dictionary<string, int>() { ["result"] = result });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 409;
                StackLogger.Error($"Server encountered an error ! message: {ex.Message} | request #{Utils.requestCounter}");
                return JsonConvert.SerializeObject(new Dictionary<string, string>() { ["error-message"] = ex.Message });
            }
            finally
            {
                RequestLogger.Info($"Incoming request | #{Utils.requestCounter} | resource: {"/stack/arguments"} | HTTP Verb DELETE | request #{Utils.requestCounter}");
                Utils.timer.Stop();
                Utils.timeSpan = Utils.timer.Elapsed;
                RequestLogger.Debug($"request #{Utils.requestCounter} duration: {Utils.timeSpan.Milliseconds}ms | request #{Utils.requestCounter}");
            }
        }
    }
}
