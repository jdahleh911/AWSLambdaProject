using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSServerlessProjectApplication
{
    public class Functions
    {
        private readonly int _biggestNumber = 180000;
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Request\n");

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonSerializer.Serialize(PrimeNumberLessThanInput(_biggestNumber)),
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

            return response;
        }

        private PrimeNumbersResult PrimeNumberLessThanInput(int input)
        {
            var result = new PrimeNumbersResult();
            result.PrimeNumbers = new List<int>();

            for (var i = 2; i < input; i++)
            {
                if (IsPrime(i))
                {
                    result.PrimeNumbers.Add(i);
                }
            }

            result.Message = $"found {result.PrimeNumbers.Count} prime numbers less than {input}";

            return result;
        }

        private static bool IsPrime(int number)
        {
            if (number <= 1)
            {
                return false;
            }

            for (var i = 2; i < number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private class PrimeNumbersResult
        {
            public string Message { get; set; }

            public List<int> PrimeNumbers { get; set; }
        }
    }
}
