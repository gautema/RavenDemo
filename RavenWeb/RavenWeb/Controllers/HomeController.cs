﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using StructureMap;

namespace RavenWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentSession _session;

        public HomeController(IDocumentSession session)
        {
            _session = session;
        }

        public ActionResult Index()
        {
            var users = _session.Query<User>().Where(x => x.Name == "Gautes");
            var users1 = _session.Query<User>().Where(x => x.Name == users.Suggest().Suggestions.First()).ToList();

            _session.Advanced.DocumentStore.Changes()
                    .ForDocumentsStartingWith("user").Subscribe(x =>
                                                                    {
                                                                        var session = ObjectFactory.GetInstance<IDocumentSession>();
                                                                        var user = _session.Load<User>(x.Id);
                                                                    });

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            _session.Store(new User{Name = "Gaute"});
            _session.Store(new User{Name = "Jan"});
            _session.SaveChanges();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
