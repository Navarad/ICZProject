using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICZProject.Services
{
    public interface IConfigService
    {
        string MasterPassword { get; }
        string ProjectsFilePath { get; }
    }
}
