using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCatsDomain.Interfaces;

namespace TheCatsDomain.Models
{
	public class AppConfiguration : IAppConfiguration
	{
		public AppSettings GetAppSettings()
		{
			string CONFIG_FILE_PATH = Path.Combine(Environment.CurrentDirectory, ".", "appsettings.json");
			return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(CONFIG_FILE_PATH));
		}
	}
}
