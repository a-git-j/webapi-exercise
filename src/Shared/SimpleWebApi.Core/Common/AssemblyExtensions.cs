using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleWebApi.Core.Extenstions
{
	public static class AssemblyExtensions
	{
		public static string GetDirectory(this Assembly assembly)
		{
			var uri = new UriBuilder(assembly.CodeBase);
			var path = Uri.UnescapeDataString(uri.Path);
			return System.IO.Path.GetDirectoryName(path);
		}
	}
}
