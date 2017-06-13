namespace Model.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ElearningDbContext : DbContext
    {
        public ElearningDbContext()
            : base("name=ElearningDbContext1")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AdminGroup> AdminGroups { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseCategory> CourseCategories { get; set; }
        public virtual DbSet<CourseTag> CourseTags { get; set; }
        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<Footer> Footers { get; set; }
        public virtual DbSet<Lecture> Lectures { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuType> MenuTypes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<RelatedCours> RelatedCourses { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<Score> Scores { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<StudentAnswer> StudentAnswers { get; set; }
        public virtual DbSet<SubmitFile> SubmitFiles { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<User_Course> User_Course { get; set; }
        public virtual DbSet<WishList> WishLists { get; set; }
        public virtual DbSet<Money> Moneys { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Answer>()
                .Property(e => e.Answer1)
                .IsFixedLength();

            modelBuilder.Entity<Assignment>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<Assignment>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Assignment>()
                .Property(e => e.DeadlineBy)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<CourseCategory>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<CourseCategory>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<CourseCategory>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Exam>()
                .Property(e => e.Type)
                .IsFixedLength();

            modelBuilder.Entity<Footer>()
                .Property(e => e.FooterID)
                .IsUnicode(false);

            modelBuilder.Entity<Lecture>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Lecture>()
                .HasMany(e => e.Assignments)
                .WithRequired(e => e.Lecture)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MenuType>()
                .HasMany(e => e.Menus)
                .WithOptional(e => e.MenuType)
                .HasForeignKey(e => e.TypeID);

            modelBuilder.Entity<Question>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.Type)
                .IsFixedLength();

            modelBuilder.Entity<Score>()
                .Property(e => e.Score1)
                .IsFixedLength();

            modelBuilder.Entity<Section>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Slide>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Slide>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<SubmitFile>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<SubmitFile>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Money>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Money>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);
        }
    }
}
