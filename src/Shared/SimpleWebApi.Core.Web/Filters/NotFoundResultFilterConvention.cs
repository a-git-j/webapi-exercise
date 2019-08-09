using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;

namespace SimpleWebApi.Core.Web.Filters
{
	public class NotFoundResultFilterConvention : IControllerModelConvention
	{
		public void Apply(ControllerModel controller)
		{
			if (IsApiController(controller))
			{
				controller.Filters.Add(new NotFoundResultFilterAttribute());
			}
		}

		private static bool IsApiController(ControllerModel controller)
		{
			if (controller.Attributes.OfType<IApiBehaviorMetadata>().Any())
			{
				return true;
			}
			return controller.ControllerType.Assembly.GetCustomAttributes().OfType<IApiBehaviorMetadata>().Any();
		}
	}
}
