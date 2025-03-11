using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using contact_management.Models;
using contact_management.Repositories.Interfaces;
using contact_management.Repositories.Services;
using Npgsql;

namespace contact_management.Repositories.Classes
{
    public class ClassRepository : IContactInterface
    {
        private readonly NpgsqlConnection _conn;
        private readonly CommonServices _common;

        public ClassRepository(NpgsqlConnection conn, CommonServices common)
        {
            _conn = conn;
            _common = common;
        }

        #region  Email Alredy Exist
        public async Task<bool> IsExist(Contact contact)
        {
            string query = "SELECT 1 FROM t_contact WHERE c_email=@email LIMIT 1";
            try
            {
                await _conn.CloseAsync();
                await _conn.OpenAsync();
                using (NpgsqlCommand cd = new NpgsqlCommand(query, _conn))
                {
                    cd.Parameters.AddWithValue("@email", contact.c_email);
                    using (NpgsqlDataReader reader = await cd.ExecuteReaderAsync())
                    {
                        return reader.Read();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error{ex.Message}");
                return false;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    await _conn.CloseAsync(); // Ensure connection is closed
                }
            }
        }
        #endregion
        #region Add Contact
        public async Task<int> AddUser(Contact contact)
        {
            if (await IsExist(contact))
            {
                return 0;
            }
            else
            {
                if (contact.ProfileImage != null && contact.ProfileImage.Length > 0)
                {
                    contact.c_image = _common.ImageName(contact.ProfileImage, contact.c_email);
                }
                else
                {
                    contact.c_image = "default.jpg";
                }
                _conn.Close();
                using (NpgsqlCommand cd = new NpgsqlCommand("CALL spr_AddContact(@userid,@contactname,@email,@address,@mobile,@group,@image,@status)"))
                {
                    _conn.Open();
                    cd.Parameters.AddWithValue("@userid",);
                    cd.Parameters.AddWithValue("@contactname", contact.c_contactname);
                    cd.Parameters.AddWithValue("@email", contact.c_email);
                    cd.Parameters.AddWithValue("@address", contact.c_address);
                    cd.Parameters.AddWithValue("@mobile", contact.c_mobile);
                    cd.Parameters.AddWithValue("@group", contact.c_group);
                    cd.Parameters.AddWithValue("@image", contact.c_image == null ? DBNull.Value : contact.c_image);
                    cd.Parameters.AddWithValue("@status", contact.c_status);

                    cd.ExecuteNonQuery();
                    await _common.UploadImage(contact.ProfileImage, contact.c_image);
                    _conn.Close();
                    return 1;
                }
            }
        }
        #endregion

        public Task<int> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Contact>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Contact>> GetAllByUser()
        {
            throw new NotImplementedException();
        }

        public Task<Contact> GetOne(string id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateUser(Contact contact)
        {
            throw new NotImplementedException();
        }
    }
}