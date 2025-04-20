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
        public DbSet<Role> Role { get; set; }

        /// <summary>
        /// Связь-таблица - "роль-пользователь"
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Таблица - "Завуч"
        /// </summary>
        public DbSet<Zavuch> Zavuch { get; set; }

        /// <summary>
        /// Таблица - "Студент"
        /// </summary>
        public DbSet<Student> Student { get; set; }

        /// <summary>
        /// Таблица - "Учитель"
        /// </summary>
        public DbSet<Teacher> Teacher { get; set; }

        /// <summary>
        /// Таблица - "Класс"
        /// </summary>
        public DbSet<Class> Class { get; set; }

        /// <summary>
        /// Таблица - "Предмет"
        /// </summary>
        public DbSet<Subject> Subject { get; set; }

        /// <summary>
        /// Связь - Таблица - "Предмет - класс"
        /// </summary>
        public DbSet<SubjectClasses> SubjectClasses { get; set; }

        /// <summary>
        /// Таблица - "Оценка"
        /// </summary>
        public DbSet<Grade> Grade { get; set; }

        /// <summary>
        /// Таблица - "Урок"
        /// </summary>
        public DbSet<Lesson> Lesson { get; set; }

        /// <summary>
        /// Таблица - "Расписание"
        /// </summary>
        public DbSet<Schedule> Schedule { get; set; }

        /// <summary>
        /// Таблица - "День недели"
        /// </summary>
        public DbSet<WeekDay> WeekDay { get; set; }

        /// <summary>
        /// Таблица - "Неделя"
        /// </summary>
        public DbSet<Week> Week { get; set; }

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

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.StudentId);
                entity.HasOne(s => s.Class)
                    .WithMany()
                    .HasForeignKey(s => s.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(s => s.User)
                    .WithMany()
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Zavuch>(entity =>
            {
                entity.HasKey(z => z.ZavuchId);
                entity.HasOne(z => z.User)
                    .WithMany()
                    .HasForeignKey(z => z.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(t => t.TeacherId);
                entity.HasOne(t => t.User)
                    .WithMany()
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(c => c.ClassId);
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(s => s.SubjectId);
                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<SubjectClasses>(entity =>
            {
                entity.HasKey(sc => new {sc.ClassId, sc.SubjectId});
                entity.HasOne(sc => sc.Class)
                    .WithMany()
                    .HasForeignKey(sc => sc.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(sc => sc.Subject)
                    .WithMany()
                    .HasForeignKey(sc => sc.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(g => g.GradeId);
                entity.HasOne(g => g.Subject)
                    .WithMany()
                    .HasForeignKey(g => g.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(g => g.Student)
                    .WithMany()
                    .HasForeignKey(g => g.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(g => g.Value)
                    .HasAnnotation("Range", new[] { 1, 5 });
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasKey(l => l.LessonId);
                entity.HasOne(l => l.Subject)
                    .WithMany()
                    .HasForeignKey(l => l.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(l => l.Teacher)
                    .WithMany()
                    .HasForeignKey(l => l.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(l => l.StartTime)
                    .IsRequired();
                entity.Property(l => l.EndTime)
                    .IsRequired();
                entity.Property(l => l.Homework)
                    .HasMaxLength(255);
                entity.Property(l => l.Room)
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<WeekDay>(entity =>
            {
                entity.HasKey(wd => wd.WeekDayId);
                entity.Property(wd => wd.Name)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Week>(entity =>
            {
                entity.HasKey(w => w.WeekId);
                entity.HasIndex(w => new { w.StartDate, w.EndDate });
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(s => new {s.ScheduleId, s.ClassId, s.WeekDayId, s.LessonOrder, s.WeekId});
                entity.HasIndex(s => new { s.ClassId, s.WeekDayId, s.LessonOrder, s.WeekId })
                    .IsUnique();
                entity.HasOne(s => s.Class)
                    .WithMany()
                    .HasForeignKey(s => s.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(s => s.WeekDay)
                    .WithMany()
                    .HasForeignKey(s => s.WeekDayId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(s => s.Lesson)
                    .WithMany()
                    .HasForeignKey(s => s.LessonId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(s => s.Week)
                    .WithMany()
                    .HasForeignKey(s => s.WeekId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
