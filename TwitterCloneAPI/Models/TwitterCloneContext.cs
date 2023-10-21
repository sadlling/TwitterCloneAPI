﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TwitterCloneAPI.Models;

public partial class TwitterCloneContext : DbContext
{
    private readonly IConfiguration _configuration;
    public TwitterCloneContext()
    {
    }

    public TwitterCloneContext(DbContextOptions<TwitterCloneContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Follower> Followers { get; set; }

    public virtual DbSet<Hashtag> Hashtags { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationType> NotificationTypes { get; set; }

    public virtual DbSet<Retweet> Retweets { get; set; }

    public virtual DbSet<SavedTweet> SavedTweets { get; set; }

    public virtual DbSet<Tweet> Tweets { get; set; }

    public virtual DbSet<TweetHashtag> TweetHashtags { get; set; }

    public virtual DbSet<UserAuthentification> UserAuthentifications { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK_Comment_commentId");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("commentId");
            entity.Property(e => e.CommentImage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("commentImage");
            entity.Property(e => e.Content)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.TweetId).HasColumnName("tweetId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Tweet).WithMany(p => p.Comments)
                .HasForeignKey(d => d.TweetId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Follower>(entity =>
        {
            entity.HasKey(e => e.FollowerId).HasName("PK_Follower_folowerId");

            entity.ToTable("Follower");

            entity.Property(e => e.FollowerId).HasColumnName("followerId");
            entity.Property(e => e.CreatedAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("createdAt");
            entity.Property(e => e.FollowerUserId).HasColumnName("followerUserId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.FollowerUser).WithMany(p => p.FollowerFollowerUsers)
                .HasForeignKey(d => d.FollowerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Follower_UserAuthentification_followeruserId");

            entity.HasOne(d => d.User).WithMany(p => p.FollowerUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Hashtag>(entity =>
        {
            entity.HasKey(e => e.HashtagId).HasName("PK_Hashtag_hashtagId");

            entity.ToTable("Hashtag");

            entity.Property(e => e.HashtagId).HasColumnName("hashtagId");
            entity.Property(e => e.CreatedAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("createdAt");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("PK_Like_likeId");

            entity.ToTable("Like");

            entity.Property(e => e.LikeId).HasColumnName("likeId");
            entity.Property(e => e.CreatedAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("createdAt");
            entity.Property(e => e.TweetId).HasColumnName("tweetId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Tweet).WithMany(p => p.Likes)
                .HasForeignKey(d => d.TweetId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK_Notification_notificationId");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("notificationId");
            entity.Property(e => e.CreatedAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("createdAt");
            entity.Property(e => e.IsReading).HasColumnName("isReading");
            entity.Property(e => e.NotificationType).HasColumnName("notificationType");
            entity.Property(e => e.SourseUserId).HasColumnName("sourseUserId");
            entity.Property(e => e.TweetId).HasColumnName("tweetId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.NotificationTypeNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.NotificationType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_NotificationType_typeId");

            entity.HasOne(d => d.SourseUser).WithMany(p => p.NotificationSourseUsers)
                .HasForeignKey(d => d.SourseUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Tweet).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.TweetId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.NotificationUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<NotificationType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK_NotificationType_typeId");

            entity.ToTable("NotificationType");

            entity.Property(e => e.TypeId).HasColumnName("typeId");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Retweet>(entity =>
        {
            entity.HasKey(e => e.RetweetId).HasName("PK_Retweet_retweetId");

            entity.ToTable("Retweet");

            entity.Property(e => e.RetweetId).HasColumnName("retweetId");
            entity.Property(e => e.CreatedAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("createdAt");
            entity.Property(e => e.TweetId).HasColumnName("tweetId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Tweet).WithMany(p => p.Retweets)
                .HasForeignKey(d => d.TweetId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Retweets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SavedTweet>(entity =>
        {
            entity.HasKey(e => e.SavedTweetId).HasName("PK_SavedTweet_savedTweetId");

            entity.ToTable("SavedTweet");

            entity.Property(e => e.SavedTweetId).HasColumnName("savedTweetId");
            entity.Property(e => e.CreatedAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("createdAt");
            entity.Property(e => e.TweetId).HasColumnName("tweetId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Tweet).WithMany(p => p.SavedTweets)
                .HasForeignKey(d => d.TweetId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.SavedTweets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Tweet>(entity =>
        {
            entity.HasKey(e => e.TweetId).HasName("PK_Tweet_tweetId");

            entity.ToTable("Tweet");

            entity.Property(e => e.TweetId)
                .ValueGeneratedOnAdd()
                .HasColumnName("tweetId");
            entity.Property(e => e.Content)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("createAt");
            entity.Property(e => e.IsPublic).HasColumnName("isPublic");
            entity.Property(e => e.TweetImage)
                .IsUnicode(false)
                .HasColumnName("tweetImage");
            entity.Property(e => e.UpdatedAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.TweetNavigation).WithOne(p => p.TweetTweetNavigation)
                .HasForeignKey<Tweet>(d => d.TweetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tweet_UserAuthentification_userId");

            entity.HasOne(d => d.User).WithMany(p => p.TweetUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tweet_UserAuthentification_userIdT");
        });

        modelBuilder.Entity<TweetHashtag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TweetHashtags_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HashtagId).HasColumnName("hashtagId");
            entity.Property(e => e.TweetId).HasColumnName("tweetId");

            entity.HasOne(d => d.Hashtag).WithMany(p => p.TweetHashtags)
                .HasForeignKey(d => d.HashtagId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Tweet).WithMany(p => p.TweetHashtags)
                .HasForeignKey(d => d.TweetId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<UserAuthentification>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_UserAuthentification_Id");

            entity.ToTable("UserAuthentification");

            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("userId");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash)
                .IsUnicode(false)
                .HasColumnName("passwordHash");
            entity.Property(e => e.RefreshToken)
                .IsUnicode(false)
                .HasColumnName("refreshToken");
            entity.Property(e => e.TokenCreated)
                .HasColumnType("datetime")
                .HasColumnName("tokenCreated");
            entity.Property(e => e.TokenExpires)
                .HasColumnType("datetime")
                .HasColumnName("tokenExpires");

            entity.HasOne(d => d.User).WithOne(p => p.UserAuthentification)
                .HasForeignKey<UserAuthentification>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAuthentification_UserProfile_profileId");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK_UserProfile_Id");

            entity.ToTable("UserProfile");

            entity.Property(e => e.ProfileId).HasColumnName("profileId");
            entity.Property(e => e.BackPicture)
                .IsUnicode(false)
                .HasColumnName("backPicture");
            entity.Property(e => e.Bio)
                .IsUnicode(false)
                .HasColumnName("bio");
            entity.Property(e => e.CreatedAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("createdAt");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("fullName");
            entity.Property(e => e.ProfilePicture)
                .IsUnicode(false)
                .HasColumnName("profilePicture");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
