using Microsoft.EntityFrameworkCore;
using ProjectDBZ.Models;

namespace ProjectDBZ.Data
{
    public class AppDbContext: DbContext
    {
        // Construtor para configurar o DbContext com as opções fornecidas
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        // DbSet para a entidade Personagem, representando a tabela de personagens no banco de dados
        public DbSet<Personagem> DBZ { get; set; }
    }
}