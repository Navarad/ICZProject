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

        public const string projectsFilePath = @"C:\Users\Tomas\Documents\ICZProject\ICZProject\Files\projects.xml";

        public void Create(ProjectModel model)
        {
            // TODO: there could be assigning unique ID

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
            return projects.Items?.ToList();
        }

        public void Delete(string projectId)
        {
            XDocument doc = XDocument.Load(projectsFilePath);
            XElement project = doc.Descendants("project").FirstOrDefault(p => p.Attribute("id").Value == projectId);
            if (project != null)
            {
                project.Remove();
                doc.Save(projectsFilePath);
            }
        }

        public void Update(ProjectModel model)
        {
            XDocument doc = XDocument.Load(projectsFilePath);
            XElement project = doc.Descendants("project").FirstOrDefault(p => p.Attribute("id").Value == model.ProjectId);
            if (project != null)
            {
                project.Remove();
                doc.Save(projectsFilePath);

                project.Element("name").Value = model.Name;
                project.Element("abbreviation").Value = model.Abbreviation;
                project.Element("customer").Value = model.Customer;

                doc.Root.Add(project);
                doc.Save(projectsFilePath);
            }
        }

        public ProjectModel Get(string id)
        {
            XDocument doc = XDocument.Load(projectsFilePath);
            XElement project = doc.Descendants("project").FirstOrDefault(a => a.Attribute("id").Value == id);
            if (project != null)
            {
                return new ProjectModel
                {
                    ProjectId = project.Attribute("id").Value,
                    Name = project.Element("name").Value,
                    Abbreviation = project.Element("abbreviation").Value,
                    Customer = project.Element("customer").Value,
                };
            }
            return null;
        }
    }
}