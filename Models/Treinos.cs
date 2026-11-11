namespace IronFit.Models
{
    public class Treinos
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int PersonalId { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; } 

       
        public Aluno? Aluno { get; set; }
        public Personal? Personal { get; set; }
    }
}