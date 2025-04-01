using Microsoft.EntityFrameworkCore;
using Project.MyWebApplicationServer.Models;
using System.Diagnostics;

namespace MyWebApplicationServer.Data
{
    /// <summary>
    /// Класс, представляющий контекст для Entity Framework
    /// </summary>
    public class LibraryContext : DbContext
    {
        /// <summary>
		/// Таблица - "пользователь"
		/// </summary>
		public DbSet<Users> Users { get; set; }

        /// <summary>
		/// Конфигурационный файл
		/// </summary>
		private readonly IConfiguration _configuration;

        /// <summary>
		/// Конструктор по умолчанию
		/// </summary>
		public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        /// <summary>
		/// Настройки генерации моделей
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Users>()
                .Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Login)
                .IsUnique();
        }
    }
}
