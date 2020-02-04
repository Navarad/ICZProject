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
                        ProjectId = model.ProjectId,
                        Name = model.Name, 
                        Abbreviation = model.Abbreviation, 
                        Customer = model.Customer
                    });
                    ViewBag._SuccessMessage = "Uspesne ste pridali novy projekt";
                }
                catch (Exception)
                {
                    //TODO: Log
                    return RedirectToAction(nameof(CreateProject));
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProject(DeleteProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProjectService.Delete(model.ProjectId);
                }
                catch (Exception)
                {
                    //TODO: Log
                }
            }
            // TODO: pass success message
            return RedirectToAction(nameof(Projects));
        }

        [HttpGet]
        public ActionResult UpdateProject(string id)
        {
            var viewModel = Populate(id);
            return View(viewModel);
        }

        private UpdateProjectViewModel Populate(string id)
        {
            var model = ProjectService.Get(id);
            var viewModel = new UpdateProjectViewModel
            {
                ProjectId = model.ProjectId,
                Name = model.Name,
                Abbreviation = model.Abbreviation,
                Customer = model.Customer
            };
            return viewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProject(UpdateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProjectService.Update(new ProjectModel
                    {
                        ProjectId = model.ProjectId,
                        Name = model.Name,
                        Abbreviation = model.Abbreviation,
                        Customer = model.Customer
                    });
                    ViewBag._SuccessMessage = "Uspesne ste upravili projekt";
                }
                catch (Exception)
                {
                    //TODO: Log
                    return RedirectToAction(nameof(UpdateProject));
                }
            }
            var viewModel = Populate(model.ProjectId);
            return View(viewModel);
        }
    }
}