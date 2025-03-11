using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using contact_management.Models;

namespace contact_management.Repositories.Interfaces
{
    public interface IContactInterface
    {
        public Task<int> AddUser(Contact contact);
        public Task<int> UpdateUser(Contact contact);
        public Task<int> DeleteUser(int id);

        public Task<List<Contact>> GetAll();
        public Task<List<Contact>> GetAllByUser();
        public Task<Contact> GetOne(string id);
    }
}