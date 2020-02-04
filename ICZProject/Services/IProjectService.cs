using ICZProject.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICZProject.Services
{
    public interface IProjectService
    {
        List<ProjectModel> ListProjects();
        void Create(ProjectModel model);
        void Delete(string projectId);
        void Update(ProjectModel model);
        ProjectModel Get(string id);
    }
}
