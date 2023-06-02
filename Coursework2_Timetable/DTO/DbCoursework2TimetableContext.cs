using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Coursework2_Timetable.DTO;

public partial class DbCoursework2TimetableContext : DbContext
{
    public DbCoursework2TimetableContext()
    {
    }

    public DbCoursework2TimetableContext(DbContextOptions<DbCoursework2TimetableContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Participant> Participants { get; set; }

    public virtual DbSet<Participantproject> Participantprojects { get; set; }

    public virtual DbSet<PlanedActivity> PlanedActivities { get; set; }

    public virtual DbSet<PlanedResult> PlanedResults { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProtectivePlane> ProtectivePlanes { get; set; }

    public virtual DbSet<RisksProject> RisksProjects { get; set; }

    public virtual DbSet<StagesProject> StagesProjects { get; set; }

    public virtual DbSet<Statuse> Statuses { get; set; }

    public virtual DbSet<SupportingMeasure> SupportingMeasures { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=db_coursework2_timetable", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("participant");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.MiddleName).HasMaxLength(255);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<Participantproject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("participantproject");

            entity.HasIndex(e => e.Idparticipant, "FK_participantproject_participant_ID");

            entity.HasIndex(e => e.Idproject, "FK_participantproject_project_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idparticipant).HasColumnName("IDParticipant");
            entity.Property(e => e.Idproject).HasColumnName("IDProject");

            entity.HasOne(d => d.IdparticipantNavigation).WithMany(p => p.Participantprojects)
                .HasForeignKey(d => d.Idparticipant)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_participantproject_participant_ID");

            entity.HasOne(d => d.IdprojectNavigation).WithMany(p => p.Participantprojects)
                .HasForeignKey(d => d.Idproject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_participantproject_project_ID");
        });

        modelBuilder.Entity<PlanedActivity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("planed_activities");

            entity.HasIndex(e => e.Idproject, "FK_planed_activities_IDProject");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Activities).HasColumnType("text");
            entity.Property(e => e.Idproject).HasColumnName("IDProject");

            entity.HasOne(d => d.IdprojectNavigation).WithMany(p => p.PlanedActivities)
                .HasForeignKey(d => d.Idproject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_planed_activities_IDProject");
        });

        modelBuilder.Entity<PlanedResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("planed_results");

            entity.HasIndex(e => e.Idproject, "FK_planed_results_IDProject");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idproject).HasColumnName("IDProject");
            entity.Property(e => e.Result).HasColumnType("text");

            entity.HasOne(d => d.IdprojectNavigation).WithMany(p => p.PlanedResults)
                .HasForeignKey(d => d.Idproject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_planed_results_IDProject");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("projects");

            entity.HasIndex(e => e.Idtype, "FK_projects_IDType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Budget).HasPrecision(10, 2);
            entity.Property(e => e.ClientEmail).HasMaxLength(255);
            entity.Property(e => e.ClientLastName)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.ClientMiddleName).HasMaxLength(255);
            entity.Property(e => e.ClientName)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.ClientNumberCard)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.ClientPhone).HasMaxLength(255);
            entity.Property(e => e.Comment).HasColumnType("text");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Idtype).HasColumnName("IDType");
            entity.Property(e => e.QualityRequments).HasColumnType("text");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.StrategyProtect).HasColumnType("text");
            entity.Property(e => e.Target).HasColumnType("text");
            entity.Property(e => e.Theme).HasColumnType("text");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Tools).HasMaxLength(255);
            entity.Property(e => e.UserLastName).HasMaxLength(255);
            entity.Property(e => e.UserMiddleName).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(255);

            entity.HasOne(d => d.IdtypeNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Idtype)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_projects_IDType");
        });

        modelBuilder.Entity<ProtectivePlane>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("protective_plane");

            entity.HasIndex(e => e.Idproject, "FK_protective_plane_IDProject");

            entity.Property(e => e.Idproject).HasColumnName("IDProject");
            entity.Property(e => e.Protect).HasColumnType("text");

            entity.HasOne(d => d.IdprojectNavigation).WithMany(p => p.ProtectivePlanes)
                .HasForeignKey(d => d.Idproject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_protective_plane_IDProject");
        });

        modelBuilder.Entity<RisksProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("risks_project");

            entity.HasIndex(e => e.Idproject, "FK_Risks_project_IDProject");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idproject).HasColumnName("IDProject");
            entity.Property(e => e.Risk).HasColumnType("text");

            entity.HasOne(d => d.IdprojectNavigation).WithMany(p => p.RisksProjects)
                .HasForeignKey(d => d.Idproject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Risks_project_IDProject");
        });

        modelBuilder.Entity<StagesProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("stages_project");

            entity.HasIndex(e => e.Idproject, "FK_stages_project_IDProject");

            entity.HasIndex(e => e.IdresponsibleParticipant, "FK_stages_project_IDResponsibleParticipant");

            entity.HasIndex(e => e.Idstatuse, "FK_stages_project_IDStatuse");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Comment).HasColumnType("text");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ExpensesMoney).HasPrecision(10, 2);
            entity.Property(e => e.Idproject).HasColumnName("IDProject");
            entity.Property(e => e.IdresponsibleParticipant).HasColumnName("IDResponsibleParticipant");
            entity.Property(e => e.Idstatuse).HasColumnName("IDStatuse");
            entity.Property(e => e.ResoursesExpendable).HasColumnType("text");
            entity.Property(e => e.ResoursesFinance).HasColumnType("text");
            entity.Property(e => e.ResoursesRenewable).HasColumnType("text");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TaskStage).HasColumnType("text");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");

            entity.HasOne(d => d.IdprojectNavigation).WithMany(p => p.StagesProjects)
                .HasForeignKey(d => d.Idproject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_stages_project_IDProject");

            entity.HasOne(d => d.IdresponsibleParticipantNavigation).WithMany(p => p.StagesProjects)
                .HasForeignKey(d => d.IdresponsibleParticipant)
                .HasConstraintName("FK_stages_project_IDResponsibleParticipant");

            entity.HasOne(d => d.IdstatuseNavigation).WithMany(p => p.StagesProjects)
                .HasForeignKey(d => d.Idstatuse)
                .HasConstraintName("FK_stages_project_IDStatuse");
        });

        modelBuilder.Entity<Statuse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("statuse");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Statuse1)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("Statuse");
        });

        modelBuilder.Entity<SupportingMeasure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("supporting_measures");

            entity.HasIndex(e => e.Idproject, "FK_supporting_measures_IDProject");

            entity.HasIndex(e => e.Idstajes, "FK_supporting_measures_IDStajes");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idproject).HasColumnName("IDProject");
            entity.Property(e => e.Idstajes).HasColumnName("IDStajes");
            entity.Property(e => e.Support).HasColumnType("text");

            entity.HasOne(d => d.IdprojectNavigation).WithMany(p => p.SupportingMeasures)
                .HasForeignKey(d => d.Idproject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_supporting_measures_IDProject");

            entity.HasOne(d => d.IdstajesNavigation).WithMany(p => p.SupportingMeasures)
                .HasForeignKey(d => d.Idstajes)
                .HasConstraintName("FK_supporting_measures_IDStajes");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("types");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Type1)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
