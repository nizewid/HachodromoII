using Hachodromo.Shared.Interfaces;

namespace Hachodromo.Web.Services
{
	public class FormFactor : IFormFactor
	{
		public string GetFormFactor() => "Web";

		public string GetPlatform() => Environment.OSVersion.ToString();
	}
}