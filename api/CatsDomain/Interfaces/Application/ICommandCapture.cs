using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TheCatsDomain.Interfaces.Application
{
    public interface ICommandCapture
    {
        Task CapureAllBreedsWithImages();
        Task CaptureImagesByCategory();
    }
}
