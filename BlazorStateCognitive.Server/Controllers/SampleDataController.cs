using BlazorStateCognitive.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace BlazorStateCognitive.Server.Controllers
{

    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private IConfiguration _configuration;
        public SampleDataController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        async public Task<ImageAnalysis> Save()
        {
            var subscriptionKey = _configuration["AzureVision"];

            List<VisualFeatureTypes> features =
             new List<VisualFeatureTypes>()
                        {
                            VisualFeatureTypes.Color,
                            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                            VisualFeatureTypes.Tags
                        };

            ComputerVisionClient computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(subscriptionKey),
                new System.Net.Http.DelegatingHandler[] { });

            // Specify the Azure region
            computerVision.Endpoint = "https://westcentralus.api.cognitive.microsoft.com";


            ImageAnalysis analysis;
            // Get the data from body
            using (MemoryStream stream = new MemoryStream())
            {
                await Request.Body.CopyToAsync(stream);
                stream.Position = 0;
                // Call Azure Vision
                analysis = await computerVision.AnalyzeImageInStreamAsync(stream
                    , features);

            }
            // Return prediction data

            Console.Write(analysis);

            return analysis;
        }


    }
}
