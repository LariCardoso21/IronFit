using MySql.Data.MySqlClient;
using IronFit.Models;

namespace IronFit.Data
{
    public class UsuarioRepository
    {
        private readonly Conexao _conexao = new();

        public async Task<Usuarios?> BuscarPorEmail(string email)
        {
            using var conn = _conexao.CriarConexao();
            await conn.OpenAsync();

            string sql = "SELECT * FROM usuarios WHERE email = @email";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@email", email);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Usuarios
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Email = reader.GetString("email"),
                    Senha = reader.GetString("senha")
                };
            }

            return null;
        }
    }
}