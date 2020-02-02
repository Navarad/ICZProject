using ICZProject.Models;
using ICZProject.ServiceModels;
using ICZProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICZProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProjectService ProjectService;

        public HomeController()
        {
        }

        public HomeController(IProjectService projectService)
        {
            this.ProjectService = projectService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new IndexViewModel { ProjectId = "prj3" };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProjectService.Create(new ProjectModel { 
                        Name = model.Name, 
                        Abbreviation = model.Abbreviation, 
                        Customer = model.Customer
                    });
                    ViewBag._SuccessMessage = "Uspesne ste pridali novy projekt";
                }
                catch (Exception)
                {
                    //TODO: Log
                    return RedirectToAction(nameof(CreateProject), nameof(HomeController));
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Projects()
        {
            var viewModel = new ProjectsViewModel();
            Populate(viewModel);
            return View(viewModel);
        }

        private void Populate(ProjectsViewModel viewModel)
        {
            //TODO: can use AutoMapper
            viewModel.Projects = ProjectService.ListProjects()?.Select(a => new ProjectViewModel { 
                ProjectId = a.ProjectId,
                Name = a.Name,
                Abbreviation = a.Abbreviation,
                Customer = a.Customer
            })?.ToList();
        }
    }
}