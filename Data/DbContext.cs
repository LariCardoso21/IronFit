using IronFit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace IronFit.Data
{
    
    public class BancoContext : DbContext
    {
       
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) { }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Personal> Personals { get; set; }
        public DbSet<Alunos> Alunos { get; set; }
        public DbSet<Treinos> Treinos { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Usuarios>().ToTable("usuarios");
            modelBuilder.Entity<Personal>().ToTable("personals");
            modelBuilder.Entity<Alunos>().ToTable("alunos");
            modelBuilder.Entity<Treinos>().ToTable("treinos");

           
            modelBuilder.Entity<Personal>()
                .Property(p => p.CpfCriptografado)
                .HasColumnName("cpf_encrypted");

           
            modelBuilder.Entity<Alunos>()
                .HasOne(a => a.Personal)
                .WithMany()
                .HasForeignKey(a => a.PersonalId)
                .IsRequired();
        }
    }
}