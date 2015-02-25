using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Kontakter.Models.Repository
{
    public class XmlRepository : IRepository
    {
        private static readonly string PhysicalPath;
        private XDocument _document;

        private XDocument Document
        {
            get
            {
                return _document ?? (_document = XDocument.Load(PhysicalPath));
            }
        }

        static XmlRepository()
        {
            PhysicalPath = Path.Combine(
                AppDomain.CurrentDomain.GetData("DataDirectory").ToString(),
                "contacts.xml");
        }

        public List<Contact> GetContacts()
        {
            return (from contact in Document.Descendants("Contact")
                    select new Contact
                    {
                        ContactId = Guid.Parse(contact.Element("Id").Value),
                        FirstName = contact.Element("FirstName").Value,
                        LastName = contact.Element("LastName").Value,
                        Email = contact.Element("Email").Value,
                    }).OrderBy(p => p.FirstName).ToList();
        }

        public void AddContact(Contact contact)
        {
            var element = new XElement("Contact",
                      new XElement("Id", contact.ContactId.ToString()),
                      new XElement("FirstName", contact.FirstName),
                      new XElement("LastName", contact.LastName),
                      new XElement("Email", contact.Email));

            Document.Root.Add(element);
        }

        public Contact GetContact(Guid id)
        {
            return (from contact in Document.Descendants("Contact")
                    where Guid.Parse(contact.Element("Id").Value).Equals(id)
                    select new Contact
                    {
                        ContactId = Guid.Parse(contact.Element("Id").Value),
                        FirstName = contact.Element("FirstName").Value,
                        LastName = contact.Element("LastName").Value,
                        Email = contact.Element("Email").Value,
                    })
              .FirstOrDefault();
        }

        public void UpdateContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException("contact");
            }

            var element = (from p in Document.Descendants("Contact")
                           where Guid.Parse(p.Element("Id").Value).Equals(contact.ContactId)
                           select p)
                                .FirstOrDefault();

            if (element != null)
            {
                element.Element("FirstName").Value = contact.FirstName;
                element.Element("LastName").Value = contact.LastName;
                element.Element("Email").Value = contact.Email;

            }
        }

        public void DeleteContact(Contact contact)
        {
            var element = (from p in Document.Descendants("Contact")
                           where Guid.Parse(p.Element("Id").Value).Equals(contact.ContactId)
                           select p)
                              .FirstOrDefault();

            if (element != null)
            {
                element.Remove();
            }
        }

        public void Save()
        {
            Document.Save(PhysicalPath);
        }

        #region IDisposable

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // här skall resursen tas bort - anropa dispose
                    // XDocument hanterar det själv...
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}






