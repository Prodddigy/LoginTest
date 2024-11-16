using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace LoginTest.Components.MiddleWare
{
	public class Middleware
	{
		private readonly RequestDelegate next;

		public static IDictionary<Guid, ClaimsPrincipal> Logins { get; private set; } = new ConcurrentDictionary<Guid, ClaimsPrincipal>();

		public Middleware(RequestDelegate next)
		{

			this.next = next ?? throw new ArgumentException(nameof(next));
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
			{
				var key = Guid.Parse(context.Request.Query["key"]);
				var claim = Logins[key];
				await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claim);

				context.Response.Redirect("/");
			}
			else
			{
				await next(context);
			}


		}
	}
}
