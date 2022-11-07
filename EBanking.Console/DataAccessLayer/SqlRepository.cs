using EBanking.Console.Model;
using System.Data.SqlClient;


namespace EBanking.Console.DataAccessLayer
{
    public class SqlRepository
    {
        public const string CONNECTION_STRING =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EBankingDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static async Task<User> CreateUser(User user)
        {
            var connection = new SqlConnection(CONNECTION_STRING);

            await connection.OpenAsync();
            var transanction = (SqlTransaction)await connection.BeginTransactionAsync();
            try
            {
                var command = connection.CreateCommand();
                command.Transaction = transanction;
                command.CommandText = "insert into [dbo].[User](FirstName, LastName, Email, Password) output inserted.ID values (@firstname, @lastname, @email, @password)";
                command.Parameters.AddWithValue("@firstname", user.FirstName);
                command.Parameters.AddWithValue("@lastname", user.LastName);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@password", user.Password);

                var id = (int?)(await command.ExecuteScalarAsync());

                if (id.HasValue == false)
                    throw new Exception("Error creating User.");

                await transanction.CommitAsync();
                user.Id = id.Value;
                return user;
            }
            catch                                 
            {
                await transanction.RollbackAsync();
                throw;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public static async Task<List<User>> GetAllUsers()
        {
            var users = new List<User>();

            var connection = new SqlConnection(CONNECTION_STRING);

            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = $"select * from [dbo].[User]";
            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(GetUser(reader));
            }

            await reader.CloseAsync();
            await connection.CloseAsync();
            return users;
        }

        public static async Task<User> DeleteUser(int id)
        {
            var existingUser = await GetUserById(id);
            if (existingUser == null)
                throw new Exception("User does not exist.");

            var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "delete from [dbo].[User] where id = @id";
            command.Parameters.AddWithValue("@id", id);

            int affectedCount = await command.ExecuteNonQueryAsync();
         
            if (affectedCount == 0)
                throw new Exception("User does not exist.");

            return existingUser;
        }

        public static async Task<User?> GetUserById(int id)
        {
            var connection = new SqlConnection(CONNECTION_STRING);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $"select * from [dbo].[User] where id=@id";
            command.Parameters.AddWithValue("@id", id);

            var reader = await command.ExecuteReaderAsync();

            User? user = null;

            if (await reader.ReadAsync())
            {
                user = GetUser(reader);
            }

            await reader.CloseAsync();
            await connection.CloseAsync();
            return user;
        }

        private static User GetUser(SqlDataReader reader)
        {
            return new User()
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                Password = reader.GetString(4)
            };
        }
    }
















}
