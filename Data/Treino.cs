using MySql.Data.MySqlClient;
using IronFit.Models;

namespace IronFit.Data
{
    public class TreinoRepository
    {
        private readonly Conexao _conexao = new();

        public async Task<List<Treinos>> ListarPorData()
        {
            var lista = new List<Treinos>();
            using var conn = _conexao.CriarConexao();
            await conn.OpenAsync();

            string sql = @"
                SELECT t.id AS treino_id, t.data_agendamento, t.status,
                       a.id AS aluno_id, a.nome AS aluno_nome, a.email AS aluno_email,
                       p.id AS personal_id, p.nome AS personal_nome, p.especialidade
                FROM treinos t
                INNER JOIN alunos a ON t.aluno_id = a.id
                INNER JOIN personals p ON t.personal_id = p.id
                ORDER BY t.data_agendamento ASC";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Treinos
                {
                    Id = reader.GetInt32("treino_id"),
                    Data = reader.GetDateTime("data_agendamento"),
                    Status = reader.GetString("status"),
                    Aluno = new Alunos
                    {
                        Id = reader.GetInt32("aluno_id"),
                        Nome = reader.GetString("aluno_nome"),
                        Email = reader.GetString("aluno_email")
                    },
                    Personal = new Personal
                    {
                        Id = reader.GetInt32("personal_id"),
                        Nome = reader.GetString("personal_nome"),
                        Especialidade = reader.GetString("especialidade")
                    }
                });
            }

            return lista;
        }

        public async Task Salvar(Treinos treino)
        {
            using var conn = _conexao.CriarConexao();
            await conn.OpenAsync();

            string sql = "INSERT INTO treinos (aluno_id, personal_id, data_agendamento, status) VALUES (@alunoId, @personalId, @data, @status)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@alunoId", treino.AlunoId);
            cmd.Parameters.AddWithValue("@personalId", treino.PersonalId);
            cmd.Parameters.AddWithValue("@data", treino.Data);
            cmd.Parameters.AddWithValue("@status", treino.Status);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AtualizarData(int id, DateTime novaData)
        {
            using var conn = _conexao.CriarConexao();
            await conn.OpenAsync();

            string sql = "UPDATE treinos SET data_agendamento = @novaData WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@novaData", novaData);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}