﻿using Hachodromo.Shared.Interfaces;

namespace Hachodromo.Maui.Services
{
	public class FormFactor : IFormFactor
	{
		public string GetFormFactor() => DeviceInfo.Idiom.ToString();

		public string GetPlatform() =>
			DeviceInfo.Platform.ToString() + " - " + DeviceInfo.VersionString;
	}
}
