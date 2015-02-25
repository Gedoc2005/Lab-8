using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Kontakter.Models;
using Kontakter.Models.Repository;


namespace Kontakter.Controllers
{
    public class ContactController : Controller
    {
        private readonly IRepository _repository;

        public ContactController()
            : this(new XmlRepository())
        {
        }

        public ContactController(IRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View(_repository.GetContacts());
        }

        // GET: contact/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName, LastName, Email")]Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repository.AddContact(contact);
                    _repository.Save();

                    TempData["success"] = "Kontakter sparad";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["error"] = "Misslyckades att spara kontakter. Försök igen!";
            }
            return View(contact);
        }

        // GET: /Kontakter/Edit
        public ActionResult Edit(Guid? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = _repository.GetContact(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }


        // POST /kontakter/Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_POST(Guid id)
        {
            var contactToUpdate = _repository.GetContact(id);

            if (contactToUpdate == null)
            {
                return HttpNotFound();
            }
            if (TryUpdateModel(contactToUpdate, string.Empty, new string[] { "FirstName", "LastName", "Email" }))
            {
                try
                {
                    _repository.UpdateContact(contactToUpdate);
                    _repository.Save();
                    TempData["success"] = "Ändringarna sparade";

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["error"] = "Misslyckades spara ändringarna. Försök igen!";
                }
            }

            return View(contactToUpdate);
        }


        // GET: /kontakt/Delete
        public ActionResult Delete(Guid? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var contact = _repository.GetContact(id.Value);
            if (contact == null)
            {
                return HttpNotFound();
            }

            return View(contact);
        }

        // POST: /kontakt/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var contactToDelete = new Contact { ContactId = id };
                _repository.DeleteContact(contactToDelete);
                _repository.Save();
                TempData["success"] = "Kontakter togs bort.";
            }
            catch (Exception)
            {
                TempData["error"] = "Misslyckades att ta bort kontakter. Försök igen!";
                return RedirectToAction("Delete", new { id = id });
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }

        public Contact contactToDelete { get; set; }
    }
}


