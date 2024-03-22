using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace TelemtryUsingJaeger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestTelemetryController : ControllerBase
    {
        //You can move this into a helper or anyother class
        public const string TelemtrySource = "TestTelemetry";

        [HttpGet]
        public void Get()
        {
            //Create the Activiy source
            ActivitySource _source = new ActivitySource(TelemtrySource);
           
            using (var activity = _source.StartActivity("FirstCallActivity"))
            {
                //Add a tag to store the data in the activity
                //For example the time before the current call
                DateTime d1 = DateTime.Now;
                activity?.SetTag("TimeOfCall", d1.ToString());

                //This sleep will simulate the processing of your method
                //It could be a third party that you want to monitor
                Thread.Sleep(1000);
                DateTime d2 = DateTime.Now;
                //Add a tag to store the data in the activity
                //For example the time after the current call
                activity?.SetTag("TimeAfterCall", d2.ToString());
                //A calculation can be added alsoS
                activity?.SetTag("CallDuration", d2 - d1);
            }

            //Do the same previous thing with a scond activity
            using (var activity = _source.StartActivity("SecondCallActivity"))
            {
                DateTime d1 = DateTime.Now;
                activity?.SetTag("TimeOfCall", d1.ToString());

                Thread.Sleep(1500);

                DateTime d2 = DateTime.Now;
                activity?.SetTag("TimeAfterCall", d2.ToString());
                activity?.SetTag("CallDuration", d2 - d1);
            }

        }
    }
}
