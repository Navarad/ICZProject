using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ICZProject.ServiceModels
{
    public class ProjectModel
    {
        [XmlAttribute("id")]
        public string ProjectId { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("abbreviation")]
        public string Abbreviation { get; set; }

        [XmlElement("customer")]
        public string Customer { get; set; }
    }

    [XmlRoot("projects")]
    public class ProjectList
    {
        [XmlElement("project")]
        public ProjectModel[] Items { get; set; }
    }
}