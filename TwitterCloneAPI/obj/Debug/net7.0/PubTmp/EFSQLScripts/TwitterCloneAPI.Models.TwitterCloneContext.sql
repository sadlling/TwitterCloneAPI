IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [Hashtag] (
        [hashtagId] int NOT NULL IDENTITY,
        [name] varchar(50) NOT NULL,
        [createdAt] datetime2 NULL,
        CONSTRAINT [PK_Hashtag_hashtagId] PRIMARY KEY ([hashtagId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [NotificationType] (
        [typeId] int NOT NULL IDENTITY,
        [name] varchar(50) NULL,
        CONSTRAINT [PK_NotificationType_typeId] PRIMARY KEY ([typeId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [UserAuthentication] (
        [userId] int NOT NULL IDENTITY,
        [email] varchar(50) NOT NULL,
        [passwordHash] varchar(max) NOT NULL,
        [refreshToken] varchar(max) NOT NULL,
        [tokenCreated] datetime NOT NULL,
        [tokenExpires] datetime NOT NULL,
        CONSTRAINT [PK_UserAuthentification_Id] PRIMARY KEY ([userId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [Follower] (
        [followerId] int NOT NULL IDENTITY,
        [userId] int NOT NULL,
        [followerUserId] int NOT NULL,
        [createdAt] datetime2 NULL,
        CONSTRAINT [PK_Follower_folowerId] PRIMARY KEY ([followerId]),
        CONSTRAINT [FK_Follower_UserAuthentification_followeruserId] FOREIGN KEY ([followerUserId]) REFERENCES [UserAuthentication] ([userId]),
        CONSTRAINT [FK_Follower_UserAuthentification_userId] FOREIGN KEY ([userId]) REFERENCES [UserAuthentication] ([userId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [Tweet] (
        [tweetId] int NOT NULL IDENTITY,
        [userId] int NOT NULL,
        [content] varchar(max) NOT NULL,
        [tweetImage] varchar(max) NULL,
        [isPublic] bit NOT NULL,
        [createAt] datetime NULL,
        [updatedAt] datetime2 NULL,
        CONSTRAINT [PK_Tweet_tweetId] PRIMARY KEY ([tweetId]),
        CONSTRAINT [FK_Tweet_UserAuthentification_userIdT] FOREIGN KEY ([userId]) REFERENCES [UserAuthentication] ([userId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [UserProfile] (
        [profileId] int NOT NULL IDENTITY,
        [userId] int NOT NULL,
        [userName] varchar(50) NULL,
        [fullName] varchar(100) NULL,
        [bio] varchar(max) NULL,
        [profilePicture] varchar(max) NULL,
        [backPicture] varchar(max) NULL,
        [createdAt] datetime2 NULL,
        CONSTRAINT [PK_UserProfile_Id] PRIMARY KEY ([profileId]),
        CONSTRAINT [FK_UserProfile_UserAuthentication_userId] FOREIGN KEY ([userId]) REFERENCES [UserAuthentication] ([userId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [Comment] (
        [commentId] int NOT NULL IDENTITY,
        [userId] int NOT NULL,
        [tweetId] int NOT NULL,
        [content] varchar(max) NULL,
        [commentImage] varchar(50) NULL,
        CONSTRAINT [PK_Comment_commentId] PRIMARY KEY ([commentId]),
        CONSTRAINT [FK_Comment_Tweet_tweetId] FOREIGN KEY ([tweetId]) REFERENCES [Tweet] ([tweetId]),
        CONSTRAINT [FK_Comment_UserAuthentification_userId] FOREIGN KEY ([userId]) REFERENCES [UserAuthentication] ([userId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [Like] (
        [likeId] int NOT NULL IDENTITY,
        [userId] int NOT NULL,
        [tweetId] int NOT NULL,
        [createdAt] datetime2 NULL,
        CONSTRAINT [PK_Like_likeId] PRIMARY KEY ([likeId]),
        CONSTRAINT [FK_Like_Tweet_tweetId] FOREIGN KEY ([tweetId]) REFERENCES [Tweet] ([tweetId]),
        CONSTRAINT [FK_Like_UserAuthentification_userId] FOREIGN KEY ([userId]) REFERENCES [UserAuthentication] ([userId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [Notification] (
        [notificationId] int NOT NULL IDENTITY,
        [userId] int NOT NULL,
        [notificationType] int NOT NULL,
        [sourseUserId] int NOT NULL,
        [tweetId] int NOT NULL,
        [isReading] bit NOT NULL,
        [createdAt] datetime2 NULL,
        CONSTRAINT [PK_Notification_notificationId] PRIMARY KEY ([notificationId]),
        CONSTRAINT [FK_Notification_NotificationType_typeId] FOREIGN KEY ([notificationType]) REFERENCES [NotificationType] ([typeId]),
        CONSTRAINT [FK_Notification_Tweet_tweetId] FOREIGN KEY ([tweetId]) REFERENCES [Tweet] ([tweetId]),
        CONSTRAINT [FK_Notification_UserAuthentification_sourseUserId] FOREIGN KEY ([sourseUserId]) REFERENCES [UserAuthentication] ([userId]),
        CONSTRAINT [FK_Notification_UserAuthentification_userId] FOREIGN KEY ([userId]) REFERENCES [UserAuthentication] ([userId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [Retweet] (
        [retweetId] int NOT NULL IDENTITY,
        [userId] int NOT NULL,
        [tweetId] int NOT NULL,
        [createdAt] datetime2 NULL,
        CONSTRAINT [PK_Retweet_retweetId] PRIMARY KEY ([retweetId]),
        CONSTRAINT [FK_Retweet_Tweet_tweetId] FOREIGN KEY ([tweetId]) REFERENCES [Tweet] ([tweetId]),
        CONSTRAINT [FK_Retweet_UserAuthentification_userId] FOREIGN KEY ([userId]) REFERENCES [UserAuthentication] ([userId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [SavedTweet] (
        [savedTweetId] int NOT NULL IDENTITY,
        [userId] int NOT NULL,
        [tweetId] int NOT NULL,
        [createdAt] datetime2 NULL,
        CONSTRAINT [PK_SavedTweet_savedTweetId] PRIMARY KEY ([savedTweetId]),
        CONSTRAINT [FK_SavedTweet_Tweet_tweetId] FOREIGN KEY ([tweetId]) REFERENCES [Tweet] ([tweetId]),
        CONSTRAINT [FK_SavedTweet_UserAuthentification_userId] FOREIGN KEY ([userId]) REFERENCES [UserAuthentication] ([userId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE TABLE [TweetHashtags] (
        [id] int NOT NULL IDENTITY,
        [hashtagId] int NOT NULL,
        [tweetId] int NOT NULL,
        CONSTRAINT [PK_TweetHashtags_id] PRIMARY KEY ([id]),
        CONSTRAINT [FK_TweetHashtags_Hashtag_hashtagId] FOREIGN KEY ([hashtagId]) REFERENCES [Hashtag] ([hashtagId]),
        CONSTRAINT [FK_TweetHashtags_Tweet_tweetId] FOREIGN KEY ([tweetId]) REFERENCES [Tweet] ([tweetId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Comment_tweetId] ON [Comment] ([tweetId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Comment_userId] ON [Comment] ([userId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Follower_followerUserId] ON [Follower] ([followerUserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Follower_userId] ON [Follower] ([userId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Like_tweetId] ON [Like] ([tweetId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Like_userId] ON [Like] ([userId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Notification_notificationType] ON [Notification] ([notificationType]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Notification_sourseUserId] ON [Notification] ([sourseUserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Notification_tweetId] ON [Notification] ([tweetId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Notification_userId] ON [Notification] ([userId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Retweet_tweetId] ON [Retweet] ([tweetId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Retweet_userId] ON [Retweet] ([userId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_SavedTweet_tweetId] ON [SavedTweet] ([tweetId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_SavedTweet_userId] ON [SavedTweet] ([userId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_Tweet_userId] ON [Tweet] ([userId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_TweetHashtags_hashtagId] ON [TweetHashtags] ([hashtagId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE INDEX [IX_TweetHashtags_tweetId] ON [TweetHashtags] ([tweetId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE UNIQUE INDEX [UQ_UserAuthentification_Email] ON [UserAuthentication] ([email]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    CREATE UNIQUE INDEX [UQ_UserProfile_UserId] ON [UserProfile] ([userId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231024084652_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231024084652_Initial', N'7.0.12');
END;
GO

COMMIT;
GO

