using ICZProject.ServiceModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ICZProject.Services
{
    public class ProjectService : IProjectService
    {
        public Lazy<IFileService> FileServiceLazy { get; set; }
        public IFileService FileService => FileServiceLazy.Value;

        public const string projectsFilePath = @"C:\Users\Tomas\source\repos\ICZProject\ICZProject\projects.xml";

        public void Create(ProjectModel model)
        {
            model.ProjectId = "ass";

            XDocument doc = XDocument.Load(projectsFilePath);
            XElement projects = doc.Element("projects");
            projects.Add(new XElement("project", 
                new XAttribute("id", model.ProjectId),
                new XElement("name", model.Name),
                new XElement("abbreviation", model.Abbreviation),
                new XElement("customer", model.Customer)));
            doc.Save(projectsFilePath);
        }

        public List<ProjectModel> ListProjects()
        {
            var fileInput = FileService.Read(projectsFilePath); // TODO: get from config
            var projects = FileService.Deserialize<ProjectList>(fileInput);
            //xmlOutputData = FileService.Serialize<List<ProjectModel>>(projects);

            return projects.Items?.ToList();
        }
    }
}