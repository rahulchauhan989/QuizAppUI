// using Microsoft.EntityFrameworkCore;
// using QuizApp.Domain.DataModels;

// namespace QuizApp.Domain.DataContext;

// public class QuizAppContext : DbContext
// {
//     public QuizAppContext(DbContextOptions<QuizAppContext> options)
//         : base(options)
//     {
//     }

//     public DbSet<User> Users { get; set; } = null!;

//     public DbSet<Quiz> Quizzes { get; set; } = null!;
//     public DbSet<Category> Categories { get; set; } = null!;
//     public DbSet<Question> Questions { get; set; } = null!;
//     public DbSet<Option> Options { get; set; } = null!;

//     public DbSet<Userquizattempt> Userquizattempts { get; set; } = null!;

//     public DbSet<Useranswer> Useranswers { get; set; } = null!;
//     public DbSet<Quizquestion> Quizquestions { get; set; } = null!;

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder.Entity<Category>()
//             .HasOne(c => c.Creator)
//             .WithMany()
//             .HasForeignKey(c => c.Createdby);

//         modelBuilder.Entity<Category>()
//             .HasOne(c => c.Updater)
//             .WithMany()
//             .HasForeignKey(c => c.Updatedby);

//         modelBuilder.Entity<Question>()
//            .HasOne(q => q.Creator)
//            .WithMany()
//            .HasForeignKey(q => q.Createdby);

//         modelBuilder.Entity<Quiz>()
//      .HasOne(q => q.Creator)
//      .WithMany()
//      .HasForeignKey(q => q.Createdby);

//         modelBuilder.Entity<Quiz>()
//             .HasOne(q => q.Updater)
//             .WithMany()
//             .HasForeignKey(q => q.Updatedby);

//         modelBuilder.Entity<Userquizattempt>().HasKey(uqa => uqa.Id);

//         modelBuilder.Entity<Useranswer>()
//         .HasOne(ua => ua.Attempt)
//         .WithMany()
//         .HasForeignKey(ua => ua.UserquizattemptId);

//     }

// }


using Microsoft.EntityFrameworkCore;
using QuizApp.Domain.DataModels;

namespace QuizApp.Domain.DataContext;

public class QuizAppContext : DbContext
{
    public QuizAppContext(DbContextOptions<QuizAppContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Quiz> Quizzes { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Option> Options { get; set; } = null!;
    public DbSet<Userquizattempt> Userquizattempts { get; set; } = null!;
    public DbSet<Useranswer> Useranswers { get; set; } = null!;
    public DbSet<Quizquestion> Quizquestions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasOne(c => c.Creator)
            .WithMany()
            .HasForeignKey(c => c.Createdby);

        modelBuilder.Entity<Category>()
            .HasOne(c => c.Updater)
            .WithMany()
            .HasForeignKey(c => c.Updatedby);

        modelBuilder.Entity<Question>()
            .HasOne(q => q.Creator)
            .WithMany()
            .HasForeignKey(q => q.Createdby);

        modelBuilder.Entity<Quiz>()
            .HasOne(q => q.Creator)
            .WithMany()
            .HasForeignKey(q => q.Createdby);

        modelBuilder.Entity<Quiz>()
            .HasOne(q => q.Updater)
            .WithMany()
            .HasForeignKey(q => q.Updatedby);

        modelBuilder.Entity<Userquizattempt>()
            .HasKey(uqa => uqa.Id); 

        modelBuilder.Entity<Userquizattempt>()
            .HasMany(uqa => uqa.Useranswers) 
            .WithOne(ua => ua.Attempt)
            .HasForeignKey(ua => ua.UserquizattemptId); 

        modelBuilder.Entity<Useranswer>()
            .HasKey(ua => ua.Id); 

            
    }
}