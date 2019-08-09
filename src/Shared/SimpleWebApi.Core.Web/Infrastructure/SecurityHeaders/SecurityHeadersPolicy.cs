using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWebApi.Core.Web.Infrastructure
{
	public class SecurityHeadersPolicy
	{
		public IDictionary<string, string> SetHeaders { get; } 
			= new Dictionary<string, string>();

		public ISet<string> RemoveHeaders { get; }
			= new HashSet<string>();
	}
}
