using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace HotelsManagementSystem.Api.Extensions
{
    public class RemoveAuthFromAnonymousOperationsTransformer : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var allowAnonymous = context.Description.ActionDescriptor.EndpointMetadata
            .OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>()
            .Any();

            if (allowAnonymous)
            {
                operation.Security?.Clear();
            }

            return Task.CompletedTask;
        }
    }
}
