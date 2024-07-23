using Manga_Omelette.Areas.Identity.Data;
using MangaASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Emit;

namespace Manga_Omelette.Data;

public class Manga_OmeletteDBContext : IdentityDbContext<User>
{
    public Manga_OmeletteDBContext(DbContextOptions<Manga_OmeletteDBContext> options)
        : base(options)
    {
    }
    public DbSet<Author> Author { get; set; }
    public DbSet<Author_Story> Author_Story { get; set; }
    public DbSet<Chapter> Chapter { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<FavoriteList> FavoriteList { get; set; }
    public DbSet<Genre> Genre { get; set; }
    public DbSet<ImageInChapter> ImageInChapter { get; set; }
    public DbSet<Rating> Rating { get; set; }

    public DbSet<Story> Story { get; set; }
    public DbSet<Story_Genre> Story_Genre { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with Chapter (optional)
        builder.Entity<Comment>()
            .HasOne(c => c.Chapter)
            .WithMany(ch => ch.Comments)
            .HasForeignKey(c => c.ChapterId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with Comic (optional)
        builder.Entity<Comment>()
            .HasOne(c => c.Story)
            .WithMany(s => s.Comments)
            .HasForeignKey(c => c.StoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Define discriminator for polymorphic relationship
        builder.Entity<Comment>()
            .HasDiscriminator<string>("CommentType")
            .HasValue<Comment>("ChapterComment")
            .HasValue<Comment>("ComicComment");

        //Cho phép xóa Comment Cha mà không xóa Replies
		builder.Entity<Comment>()
			.HasOne(c => c.ParentComment)
			.WithMany(c => c.Replies)
			.HasForeignKey(c => c.ParentCommentId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
