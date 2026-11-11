using MySql.Data.MySqlClient;

namespace IronFit.Data
{
    public class Conexao
    {
        private readonly string _connectionString = "Server=localhost;Database=saep_db;Uid=root;Pwd=;";

        public MySqlConnection CriarConexao()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
