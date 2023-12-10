using GraphQL;
using GraphQL.Instrumentation;
using Microsoft.AspNetCore.Http;
using Time.Commerce.Application.Constants;
using Time.Commerce.GraphQl.Constants;

namespace Time.Commerce.GraphQl.Middleware
{
    public class FieldsValidateMiddleware : IFieldMiddleware
    {
        private readonly IHttpContextAccessor _accessor;
        public FieldsValidateMiddleware(IHttpContextAccessor accessor)
            => _accessor = accessor;
        public async Task<object> Resolve(IResolveFieldContext context, FieldMiddlewareDelegate next)
        {
            var arguments = context.Arguments;
            string store = string.Empty;
            if (arguments is not null && !string.IsNullOrWhiteSpace(arguments.FirstOrDefault(x => x.Key.ToLower() == TimeGraphQlConstants.STORE).Key))
            {
                store = context.GetArgument<string>(TimeGraphQlConstants.STORE)?.ToString();
                if (string.IsNullOrWhiteSpace(store))
                    store = _accessor.HttpContext.Request.Headers[TimeGraphQlConstants.STORE].ToString();
                
                if (string.IsNullOrWhiteSpace(store))
                {
                    context.Errors.Add(new ExecutionError(CommonErrors.INVALID_STORE_ID));
                    return null;
                }
            }
          
            context.SetExtension($"{context.Path}.{TimeGraphQlConstants.STORE}", store);
            return await next(context).ConfigureAwait(false);
        }
    }
}
