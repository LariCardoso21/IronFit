using MySql.Data.MySqlClient;
using IronFit.Models;

namespace IronFit.Data
{
    public class PersonalRepository
    {
        private readonly Conexao _conexao = new();

        public async Task<List<Personal>> Listar(string busca = "")
        {
            var lista = new List<Personal>();
            using var conn = _conexao.CriarConexao();
            await conn.OpenAsync();

            string sql = "SELECT * FROM personals";
            if (!string.IsNullOrEmpty(busca))
            {
                sql += " WHERE nome LIKE @busca OR especialidade LIKE @busca";
            }

            using var cmd = new MySqlCommand(sql, conn);
            if (!string.IsNullOrEmpty(busca))
            {
                cmd.Parameters.AddWithValue("@busca", $"%{busca}%");
            }

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Personal
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Cpf = reader.GetString("cpf"), 
                    Telefone = reader.GetString("telefone"),
                    Especialidade = reader.GetString("especialidade")
                });
            }

            return lista;
        }

        public async Task<Personal?> BuscarPorId(int id)
        {
            using var conn = _conexao.CriarConexao();
            await conn.OpenAsync();

            string sql = "SELECT * FROM personals WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Personal
                {
                    Id = reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Cpf = reader.GetString("cpf"),
                    Telefone = reader.GetString("telefone"),
                    Especialidade = reader.GetString("especialidade")
                };
            }

            return null;
        }

        public async Task Salvar(Personal personal)
        {
            using var conn = _conexao.CriarConexao();
            await conn.OpenAsync();

            string sql = "INSERT INTO personals (nome, cpf, telefone, especialidade) VALUES (@nome, @cpf, @telefone, @especialidade)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", personal.Nome);
            cmd.Parameters.AddWithValue("@cpf", personal.Cpf); 
            cmd.Parameters.AddWithValue("@telefone", personal.Telefone);
            cmd.Parameters.AddWithValue("@especialidade", personal.Especialidade);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Atualizar(Personal personal)
        {
            using var conn = _conexao.CriarConexao();
            await conn.OpenAsync();

            string sql = "UPDATE personals SET nome = @nome, telefone = @telefone, especialidade = @especialidade WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", personal.Id);
            cmd.Parameters.AddWithValue("@nome", personal.Nome);
            cmd.Parameters.AddWithValue("@telefone", personal.Telefone);
            cmd.Parameters.AddWithValue("@especialidade", personal.Especialidade);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}