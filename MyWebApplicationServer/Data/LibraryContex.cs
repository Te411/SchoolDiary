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
		/// Таблица - "Пользователь"
		/// </summary>
		public DbSet<User> Users { get; set; }

        /// <summary>
        /// Таблица - "Роль"
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Связь-таблица - "роль-пользователь"
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

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
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
                entity.HasIndex(u => u.Email)
                .IsUnique();
                entity.Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(255);
                entity.HasIndex(u => u.Login)
                .IsUnique();

            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.RoleId);
                entity.Property(r => r.RoleName)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.HasIndex(r => r.RoleName)
                    .IsUnique();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
                entity.HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId);
                entity.HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId);
            });
        }
    }
}
