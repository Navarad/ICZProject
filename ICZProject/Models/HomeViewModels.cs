using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ICZProject.Models
{
    public class IndexViewModel
    {
        public string ProjectId { get; set; }
    }

    public class ProjectViewModel
    {
        [Display(Name = "ID")]
        [Required]
        public string ProjectId { get; set; }

        [Display(Name = "Nazov projektu")] // TODO: Resources
        [Required]
        public string Name { get; set; }

        [Display(Name = "Skratka")]
        [Required]
        public string Abbreviation { get; set; }

        [Display(Name = "Zakaznik")]
        [Required]
        public string Customer { get; set; }

    }

    public class ProjectsViewModel
    {
        public List<ProjectViewModel> Projects { get; set; }
    }

    public class DeleteProjectViewModel
    {
        public string ProjectId { get; set; }
    }

    public class UpdateProjectViewModel
    {
        [Display(Name = "ID")]
        [Required]
        public string ProjectId { get; set; }

        [Display(Name = "Nazov projektu")] // TODO: Resources
        [Required]
        public string Name { get; set; }

        [Display(Name = "Skratka")]
        [Required]
        public string Abbreviation { get; set; }

        [Display(Name = "Zakaznik")]
        [Required]
        public string Customer { get; set; }

    }
}