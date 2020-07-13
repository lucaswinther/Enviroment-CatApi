using System;
using System.Collections.Generic;
using System.Text;
using TheCatsDomain.Models;

namespace TheCatsDomain.Interfaces
{
    public interface IAppConfiguration
    {   
        AppSettings GetAppSettings();
    }
}
