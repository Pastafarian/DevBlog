using System.Linq;
using System.Security.Claims;
using DevBlog.Application.Auth;

namespace DevBlog.Application.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		public static SiteUser BuildSiteUser(this ClaimsPrincipal principle)
		{
			if (principle == null || !principle.Identity.IsAuthenticated) return new SiteUser();

			var siteUser = new SiteUser
			{
				IsAuthenticated = true,
				IsAdmin = principle.HasClaim(Claims.IsAdmin)
			};

			return siteUser;
		}

		private static bool HasClaim(this ClaimsPrincipal principle, string type)
		{
			return principle.Claims.Any(x => x.Type == type);
		}
	}
}