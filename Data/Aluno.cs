using MySql.Data.MySqlClient;
using IronFit.Models;

namespace IronFit.Data
{
    public class AlunoRepository
    {
        private readonly Conexao _conexao = new();

        public async Task<List<Alunos>> Listar()
        {
            var lista = new List<Alunos>();
            using var conn = _conexao.CriarConexao();
            await conn.OpenAsync();

            string sql = "SELECT * FROM alunos";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Alunos
                {
                    Id = reader.GetInt32("id"),
                    PersonalId = reader.GetInt32("personal_id"),
                    Nome = reader.GetString("nome"),
                    Email = reader.GetString("email"),
                    Telefone = reader.GetString("telefone")
                });
            }

            return lista;
        }

        public async Task<Alunos?> BuscarPorId(int id)
        {
            using var conn = _conexao.CriarConexao();
            await conn.OpenAsync();

            string sql = "SELECT * FROM alunos WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Alunos
                {
                    Id = reader.GetInt32("id"),
                    PersonalId = reader.GetInt32("personal_id"),
                    Nome = reader.GetString("nome"),
                    Email = reader.GetString("email"),
                    Telefone = reader.GetString("telefone")
                };
            }

            return null;
        }

        public async Task Salvar(Alunos aluno)
        {
            using var conn = _conexao.CriarConexao();
            await conn.OpenAsync();

            string sql = "INSERT INTO alunos (personal_id, nome, email, telefone) VALUES (@personalId, @nome, @email, @telefone)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@personalId", aluno.PersonalId); 
            cmd.Parameters.AddWithValue("@nome", aluno.Nome);
            cmd.Parameters.AddWithValue("@email", aluno.Email);
            cmd.Parameters.AddWithValue("@telefone", aluno.Telefone);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}