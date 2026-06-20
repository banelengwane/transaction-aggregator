// File: Attributes/ApiKeyAuthAttribute.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TransactionAggregator.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private const string ApiKeyHeaderName = "X-API-KEY";
        private const string SecureMockKey = "PortfolioAggregatorKey2026";

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;

            if (!request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Authentication Failed: X-API-KEY header is missing."
                };
                return Task.CompletedTask;
            }

            if (!SecureMockKey.Equals(extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403,
                    Content = "Forbidden: Invalid API Key."
                };
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}