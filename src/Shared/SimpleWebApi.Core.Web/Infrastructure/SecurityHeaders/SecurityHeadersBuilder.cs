using System;

namespace SimpleWebApi.Core.Web.Infrastructure
{
	public class SecurityHeadersBuilder
	{
		public SecurityHeadersBuilder()
		{
			_policy = new SecurityHeadersPolicy();
		}
		private readonly SecurityHeadersPolicy _policy;

		public const int ONE_YEAR = 360 * 24 * 365;

		public SecurityHeadersBuilder AddDefaultSecureHeadersPolicy()
		{
			AddStrictTransportSecurityMaxAge();
			AddFrameOptionsDeny();
			AddXssProtectionBlock();
			AddContentTypeOptionsNoSniff();
			AddContentSecurityPolicySelf();
			RemoveServerHeader();
			RemovePoweredByHeader();
			return this;
		}

		public SecurityHeadersBuilder AddXssProtectionEnabled()
		{
			_policy.SetHeaders[XssProtectionHeader.Name] = XssProtectionHeader.Enabled;
			return this;
		}
		public SecurityHeadersBuilder AddXssProtectionDisabled()
		{
			_policy.SetHeaders[XssProtectionHeader.Name] = XssProtectionHeader.Disabled;
			return this;
		}
		public SecurityHeadersBuilder AddXssProtectionBlock()
		{
			_policy.SetHeaders[XssProtectionHeader.Name] = XssProtectionHeader.BlockMode;
			return this;
		}


		public SecurityHeadersBuilder AddStrictTransportSecurityMaxAgeIncludeSubDomains(int maxAge = ONE_YEAR)
		{
			_policy.SetHeaders[StrictTransportSecurityHeader.Name] = string.Format(StrictTransportSecurityHeader.MaxAgeSecondsIncludeSubdomains, maxAge);
			return this;
		}
		public SecurityHeadersBuilder AddStrictTransportSecurityMaxAge(int maxAge = ONE_YEAR)
		{
			_policy.SetHeaders[StrictTransportSecurityHeader.Name] = string.Format(StrictTransportSecurityHeader.MaxAge, maxAge);
			return this;
		}
		public SecurityHeadersBuilder AddStrictTransportSecurityNoCache()
		{
			_policy.SetHeaders[StrictTransportSecurityHeader.Name] = StrictTransportSecurityHeader.NoCache;
			return this;
		}

		public SecurityHeadersBuilder AddContentSecurityPolicySelf()
		{
			_policy.SetHeaders[ContentSecurityPolicyHeader.Name] = ContentSecurityPolicyHeader.Self;
			return this;
		}
		public SecurityHeadersBuilder AddContentSecurityPolicyScriptAndStyleSourceSelf()
		{
			_policy.SetHeaders[ContentSecurityPolicyHeader.Name] = string.Format("{0}; {1}",
				ContentSecurityPolicyHeader.ScriptSourceSelf, ContentSecurityPolicyHeader.StyleSourceSelf);
			return this;
		}

		public SecurityHeadersBuilder AddFrameOptionsDeny()
		{
			_policy.SetHeaders[FrameOptionsHeader.Name] = FrameOptionsHeader.Deny;
			return this;
		}
		public SecurityHeadersBuilder AddFrameOptionsSameOrigin()
		{
			_policy.SetHeaders[FrameOptionsHeader.Name] = FrameOptionsHeader.SameOrigin;
			return this;
		}
		public SecurityHeadersBuilder AddFrameOptionsSameOrigin(string uri)
		{
			_policy.SetHeaders[FrameOptionsHeader.Name] = string.Format(FrameOptionsHeader.AllowFromUri, uri);
			return this;
		}


		public SecurityHeadersBuilder AddContentTypeOptionsNoSniff()
		{
			_policy.SetHeaders[ContentTypeOptionsHeader.Name] = ContentTypeOptionsHeader.NoSniff;
			return this;
		}

		public SecurityHeadersBuilder RemoveServerHeader()
		{
			_policy.RemoveHeaders.Add(ServerHeader.Name);
			return this;
		}
		public SecurityHeadersBuilder RemovePoweredByHeader()
		{
			_policy.RemoveHeaders.Add(PoweredByHeader.Name);
			return this;
		}

		public SecurityHeadersBuilder AddCustomHeader(string header, string value)
		{
			if (string.IsNullOrEmpty(header))
			{
				throw new ArgumentNullException(nameof(header));
			}
			_policy.SetHeaders[header] = value ?? "";
			return this;
		}
		public SecurityHeadersBuilder RemoveHeader(string header)
		{
			if (string.IsNullOrEmpty(header))
			{
				throw new ArgumentNullException(nameof(header));
			}
			_policy.RemoveHeaders.Add(header);
			return this;
		}

		/// <summary>
		/// Builds a new <see cref="SecurityHeadersPolicy"/> using the entries added.
		/// </summary>
		/// <returns>The constructed <see cref="SecurityHeadersPolicy"/>.</returns>
		public SecurityHeadersPolicy Build()
		{
			return _policy;
		}
	}
}

