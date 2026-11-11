namespace IronFit.Models
{
    public class Alunos
    {
        public int Id { get; set; }
        public int PersonalId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;

       
        public Personal? Personal { get; set; }

        internal async Task Salvar(Alunos aluno)
        {
            throw new NotImplementedException();
        }
    }
}