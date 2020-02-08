using ICZProject.Models;
using ICZProject.ServiceModels;
using ICZProject.Services;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity;
using Serilog;
using System.Collections.Generic;

namespace ICZProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Init

        private readonly IProjectService ProjectService;
        private readonly ILogger Log;
        private readonly IAuthenticationManager AuthenticationManager;
        private readonly IConfigService ConfigService;

        public HomeController()
        {
        }

        public HomeController(IProjectService projectService, ILogger log, IAuthenticationManager authenticationManager, IConfigService configService)
        {
            this.ProjectService = projectService;
            this.Log = log;
            this.AuthenticationManager = authenticationManager;
            this.ConfigService = configService;
        }

        #endregion

        #region Index - Login

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            if (Request?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(Projects));

            var viewModel = new IndexViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(IndexViewModel model)
        {
            if (model.Password != ConfigService.MasterPassword)
            {
                ModelState.AddModelError(nameof(IndexViewModel.Password), "Nespravne heslo");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var userIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, userIdentity);
                    return RedirectToAction(nameof(Projects));
                }
                catch (Exception ex)
                {
                    var message = "Something went wrong with your login, please try again later";
                    ModelState.AddModelError(string.Empty, message);
                    Log.Error(ex, message);
                }
            }

            var viewModel = new IndexViewModel();
            return View(viewModel);
        }

        #endregion

        #region Create Project

        [HttpGet]
        public ActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
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
                catch (ArgumentException)
                {
                    ModelState.AddModelError(nameof(ProjectViewModel.ProjectId), "Project with same name already exists");
                }
                catch (Exception ex)
                {
                    var message = "Something went wrong with project create, please try again later";
                    Log.Error(ex, message);
                    return RedirectToAction(nameof(CreateProject));
                }
            }
            return View();
        }

        #endregion

        #region List Projects

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
            })?.ToList() ?? new List<ProjectViewModel>();
        }

        #endregion

        #region Delete Project

        [HttpPost]
        public ActionResult DeleteProject(DeleteProjectViewModel model)
        {
            if (ProjectService.Get(model.ProjectId) == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    ProjectService.Delete(model.ProjectId);
                }
                catch (Exception ex)
                {
                    var message = "Something went wrong with project delete, please try again later";
                    Log.Error(ex, message);
                }
            }
            // TODO: pass success message
            return RedirectToAction(nameof(Projects));
        }

        #endregion

        #region Update Project

        [HttpGet]
        public ActionResult UpdateProject(string id)
        {
            var model = ProjectService.Get(id);
            if (model == null)
                return HttpNotFound();

            var viewModel = Populate(model);
            return View(viewModel);
        }

        private UpdateProjectViewModel Populate(ProjectModel model)
        {
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
        public ActionResult UpdateProject(UpdateProjectViewModel model)
        {
            var storedModel = ProjectService.Get(model.ProjectId);
            if (storedModel == null)
                return HttpNotFound();

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
                catch (Exception ex)
                {
                    var message = "Something went wrong with project update, please try again later";
                    Log.Error(ex, message);
                    return RedirectToAction(nameof(UpdateProject));
                }
            }
            var viewModel = Populate(storedModel);
            return View(viewModel);
        }

        #endregion
    }
}