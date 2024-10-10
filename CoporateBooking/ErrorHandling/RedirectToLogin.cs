using DomainLayer.Model;

namespace OnionConsumeWebAPI.ErrorHandling
{
    public class RedirectToLogin
    {

        private readonly RequestDelegate _next1;

        public RedirectToLogin(RequestDelegate next1)
        {
            _next1 = next1;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next1(context);

            if (context.Response.StatusCode == 404 )
            {
                context.Response.Clear();
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html";
                var result = $@"
                <html>
                    <body>
                        <div class=""row"" style=""text-align:center;padding:30px;"">
            <div class=""col-12"">
            <h3 style=""color:red; font-size:28px;"">Session timeout plz try again.</h3>
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
}
