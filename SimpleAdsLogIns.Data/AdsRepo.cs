using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SimpleAdsLogIns.Data
{
    public class AdsRepository
    {
        private string _connection;
        public AdsRepository(string connect)
        {
            _connection = connect;
        }

        public void AddUser(User user, string password)
        {
            using var connection = new SqlConnection(_connection);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @" insert into USERS (Name, Email, PwHash)" +
                " values (@name, @email, @hash)";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@hash", BCrypt.Net.BCrypt.HashPassword(password));
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public User Login(string email, string pw)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(pw, user.PwHash);
            return isValid ? user : null;
        }

        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connection);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"select top 1 * from users where email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                Email = (string)reader["Email"],
                Name = (string)reader["Name"],
                PwHash = (string)reader["PwHash"]
            };
        }

        public void Add(Ad ad, int userid, string name)
        {

            using SqlConnection connect = new SqlConnection(_connection);
            using SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = @"insert into ads(Name, Number, Text, Date, UserId)
                        values(@name, @number, @text, @date, @UserId) ";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@number", ad.Number);
            cmd.Parameters.AddWithValue("@text", ad.Text);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@UserId", userid);
            connect.Open();
            cmd.ExecuteNonQuery();

        }

        public List<Ad> GetAds()
        {
            using SqlConnection connect = new SqlConnection(_connection);
            using SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = @"select * from ads order by date desc";

            var result = new List<Ad>();
            connect.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Ad
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["name"],
                    Number = (string)reader["number"],
                    Text = (string)reader["text"],
                    Date = (DateTime)reader["date"],
                    UserId=(int)reader["userId"]

                });
            }
            return result;
        }

        public void DeleteAd(int id)
        {
            using SqlConnection connect = new SqlConnection(_connection);
            using SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = @"delete from ads where id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            connect.Open();
            cmd.ExecuteNonQuery();


        }
        public int GetIdByEmail(string email)
        {
            using SqlConnection connect = new SqlConnection(_connection);
            using SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = @"select Id from users where email=@email";
            cmd.Parameters.AddWithValue("@email", email);
            connect.Open();
            return (int)cmd.ExecuteScalar();
        }

    }
}
