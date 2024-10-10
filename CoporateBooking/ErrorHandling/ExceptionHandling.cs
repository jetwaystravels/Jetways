using DomainLayer.Model;
using System.Net;
using System.Text.Json;

namespace OnionConsumeWebAPI.ErrorHandling
{
    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandling> _logger;

        public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            context.Response.ContentType = "text/html";
           // context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorResponse
            {
                Success = false
            };
            switch (exception)
            {

             


                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid Token"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.Message = ex.Message;
                        break;
                    }
                    if (context.Response.StatusCode == 404)
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("Session expired");
                    }
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Internal server error!";
                    break;
            }
            _logger.LogError(exception.Message);
            //var result = JsonSerializer.Serialize(errorResponse);
            var result = $@"
                <html>
                    <body>
                        <div class=""row"" style=""text-align:center;padding:30px;"">
            <div class=""col-12"">
            <h3 style=""color:red; font-size:28px;"">{errorResponse.Message}</h3>
             </div>
            <div class=""col-12"">
            <button onclick=""window.location.href='http://localhost:5201'"">Go to Home Page</button>
             </div>
            </div>
        </body>
    </html>";
            await context.Response.WriteAsync(result);
        }
    }
}
