using System;
using System.Collections.Generic;
using System.Text;

namespace TheCatsDomain.Models
{
	public class AppSettings
	{
		public string ConnectionString { get; set; }
		public TheCatSettings TheCatSettings { get; set; }
		public ElasticSettings ElasticSettings { get; set; }
	}
}
