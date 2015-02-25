using System;
using System.Collections.Generic;
using Kontakter.Models;

namespace Kontakter.Models.Repository
{
    public interface IRepository : IDisposable
    {
        List<Contact> GetContacts();
        Contact GetContact(Guid id);
        void AddContact(Contact contact);
        void UpdateContact(Contact contact);
        void DeleteContact(Contact contact);
        void Save();
    }
}

