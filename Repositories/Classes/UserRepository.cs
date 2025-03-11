using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using contact_management.Repositories.Services;
using Npgsql;

namespace contact_management.Repositories.Classes
{
    public class UserRepository : IUserInterface
    {
        private readonly CommonServices _common;
        private readonly NpgsqlConnection _conn;

        public UserRepository(CommonServices common, NpgsqlConnection conn)
        {
            _common = common;
            _conn = conn;
        }


        #region  Email Alredy Exist
        public async Task<bool> IsExist(User user)
        {
            string query = "SELECT 1 FROM t_user WHERE c_email=@email LIMIT 1";
            try
            {
                await _conn.CloseAsync();
                await _conn.OpenAsync();
                using (NpgsqlCommand cd = new NpgsqlCommand(query, _conn))
                {
                    cd.Parameters.AddWithValue("@email", user.c_email);
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


        #region Register
        public async Task<int> AddUser(User user)
        {
            string query = "CALL spr_AddUser(@username,@email,@password,@address,@image,@gender,@mobile)";
            try
            {
                if (await IsExist(user))
                {
                    return 0;
                }
                else
                {

                    if (user.ProfileImage != null && user.ProfileImage.Length > 0)
                    {
                        user.c_image = _common.ImageName(user.ProfileImage, user.c_email);
                    }
                    else
                    {
                        user.c_image = "default.jpg";
                    }
                    await _conn.CloseAsync();
                    await _conn.OpenAsync();

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, _conn))
                    {
                        cmd.Parameters.AddWithValue("@username", user.c_name);
                        cmd.Parameters.AddWithValue("@email", user.c_email);
                        cmd.Parameters.AddWithValue("@password", user.c_password);
                        cmd.Parameters.AddWithValue("@address", user.c_address);
                        cmd.Parameters.AddWithValue("@gender", user.c_gender);
                        cmd.Parameters.AddWithValue("@mobile", user.c_mobile);
                        cmd.Parameters.AddWithValue("@image", user.c_image != null ? user.c_image : DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();//this is use for perform insert update delete query;
                        await _common.UploadImage(user.ProfileImage, user.c_image);
                        await _conn.CloseAsync();
                        return 1;
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Register:-{ex.Message}");
                return -1;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    await _conn.CloseAsync();
                }
            }
        }
        #endregion

        #region Login
        public async Task<User> GetUser(Login login)
        {
            User detail = new User();
            try
            {
                _conn.Close();
                await _conn.OpenAsync();
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM t_user WHERE c_email=@email AND c_password = @password", _conn))
                {
                    cmd.Parameters.AddWithValue("@email", login.c_email);
                    cmd.Parameters.AddWithValue("@password", login.c_password);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            detail.c_id = reader.GetInt32(0);
                            detail.c_email = reader.GetString(1);
                            detail.c_password = reader.GetString(2);
                            detail.c_address = reader.GetString(3);
                            detail.c_image = reader.GetString(4);
                            detail.c_gender = reader.GetString(5);
                            detail.c_mobile = reader.GetString(6);
                            detail.c_name = reader.GetString(7);
                        }
                    }
                }
                _conn.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Login:-{ex.Message}");

            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    await _conn.CloseAsync();
                }
            }
            return detail;
        }
        #endregion Login


    }
}