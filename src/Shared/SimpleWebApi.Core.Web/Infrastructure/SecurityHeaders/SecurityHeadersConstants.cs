using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWebApi.Core.Web.Infrastructure
{
	public static class XssProtectionHeader
	{
		public static readonly string Name = "X-XSS-Protection";

		public static readonly string Enabled = "1";
		public static readonly string Disabled = "0";

		public static readonly string BlockMode = "1; mode=block";
	}

	public static class StrictTransportSecurityHeader
	{
		public static readonly string Name = "Strict-Transport-Security";
		public static readonly string MaxAge = "max-age={0}";

		public static readonly string MaxAgeSecondsIncludeSubdomains = "max-age={0}; includeSubDomains";
		public static readonly string NoCache = "max-age=0";
	}
	public static class ContentSecurityPolicyHeader
	{
		public static readonly string Name = "Content-Security-Policy";
		public static readonly string Self = "'self'";
		public static readonly string ScriptSourceSelf = "script-src 'self'";
		public static readonly string StyleSourceSelf = "style-src 'self'";
		public static readonly string ImageSourceSelf = "img-src 'self'";
	}
	public static class FrameOptionsHeader
	{
		public static readonly string Name = "X-Frame-Options";
		public static readonly string Deny = "DENY";
		public static readonly string SameOrigin = "SAMEORIGIN";
		public static readonly string AllowFromUri = "ALLOW-FROM {0}";
	}
	public static class ContentTypeOptionsHeader
	{
		public static readonly string Name = "X-Content-Type-Options";
		public static readonly string NoSniff = "nosniff";
	}
	public static class ServerHeader
	{
		public static readonly string Name = "Server";
	}
	public static class PoweredByHeader
	{
		public static readonly string Name = "X-Powered-By";
	}
}
