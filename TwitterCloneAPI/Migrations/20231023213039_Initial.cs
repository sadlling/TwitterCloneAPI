using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterCloneAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hashtag",
                columns: table => new
                {
                    hashtagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashtag_hashtagId", x => x.hashtagId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationType",
                columns: table => new
                {
                    typeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationType_typeId", x => x.typeId);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthentification",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    passwordHash = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    refreshToken = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    tokenCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    tokenExpires = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthentification_Id", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Follower",
                columns: table => new
                {
                    followerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    followerUserId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follower_folowerId", x => x.followerId);
                    table.ForeignKey(
                        name: "FK_Follower_UserAuthentification_followeruserId",
                        column: x => x.followerUserId,
                        principalTable: "UserAuthentification",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "FK_Follower_UserAuthentification_userId",
                        column: x => x.userId,
                        principalTable: "UserAuthentification",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "Tweet",
                columns: table => new
                {
                    tweetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    tweetImage = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    isPublic = table.Column<bool>(type: "bit", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tweet_tweetId", x => x.tweetId);
                    table.ForeignKey(
                        name: "FK_Tweet_UserAuthentification_userIdT",
                        column: x => x.userId,
                        principalTable: "UserAuthentification",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    profileId = table.Column<int>(type: "int", nullable: false),
                    userName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fullName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    bio = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    profilePicture = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    backPicture = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile_Id", x => x.profileId);
                    table.ForeignKey(
                        name: "FK_UserProfile_UserAuthentification_userId",
                        column: x => x.profileId,
                        principalTable: "UserAuthentification",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    commentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    tweetId = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    commentImage = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment_commentId", x => x.commentId);
                    table.ForeignKey(
                        name: "FK_Comment_Tweet_tweetId",
                        column: x => x.tweetId,
                        principalTable: "Tweet",
                        principalColumn: "tweetId");
                    table.ForeignKey(
                        name: "FK_Comment_UserAuthentification_userId",
                        column: x => x.userId,
                        principalTable: "UserAuthentification",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "Like",
                columns: table => new
                {
                    likeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    tweetId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Like_likeId", x => x.likeId);
                    table.ForeignKey(
                        name: "FK_Like_Tweet_tweetId",
                        column: x => x.tweetId,
                        principalTable: "Tweet",
                        principalColumn: "tweetId");
                    table.ForeignKey(
                        name: "FK_Like_UserAuthentification_userId",
                        column: x => x.userId,
                        principalTable: "UserAuthentification",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    notificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    notificationType = table.Column<int>(type: "int", nullable: false),
                    sourseUserId = table.Column<int>(type: "int", nullable: false),
                    tweetId = table.Column<int>(type: "int", nullable: false),
                    isReading = table.Column<bool>(type: "bit", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification_notificationId", x => x.notificationId);
                    table.ForeignKey(
                        name: "FK_Notification_NotificationType_typeId",
                        column: x => x.notificationType,
                        principalTable: "NotificationType",
                        principalColumn: "typeId");
                    table.ForeignKey(
                        name: "FK_Notification_Tweet_tweetId",
                        column: x => x.tweetId,
                        principalTable: "Tweet",
                        principalColumn: "tweetId");
                    table.ForeignKey(
                        name: "FK_Notification_UserAuthentification_sourseUserId",
                        column: x => x.sourseUserId,
                        principalTable: "UserAuthentification",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "FK_Notification_UserAuthentification_userId",
                        column: x => x.userId,
                        principalTable: "UserAuthentification",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "Retweet",
                columns: table => new
                {
                    retweetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    tweetId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retweet_retweetId", x => x.retweetId);
                    table.ForeignKey(
                        name: "FK_Retweet_Tweet_tweetId",
                        column: x => x.tweetId,
                        principalTable: "Tweet",
                        principalColumn: "tweetId");
                    table.ForeignKey(
                        name: "FK_Retweet_UserAuthentification_userId",
                        column: x => x.userId,
                        principalTable: "UserAuthentification",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "SavedTweet",
                columns: table => new
                {
                    savedTweetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    tweetId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedTweet_savedTweetId", x => x.savedTweetId);
                    table.ForeignKey(
                        name: "FK_SavedTweet_Tweet_tweetId",
                        column: x => x.tweetId,
                        principalTable: "Tweet",
                        principalColumn: "tweetId");
                    table.ForeignKey(
                        name: "FK_SavedTweet_UserAuthentification_userId",
                        column: x => x.userId,
                        principalTable: "UserAuthentification",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "TweetHashtags",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hashtagId = table.Column<int>(type: "int", nullable: false),
                    tweetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TweetHashtags_id", x => x.id);
                    table.ForeignKey(
                        name: "FK_TweetHashtags_Hashtag_hashtagId",
                        column: x => x.hashtagId,
                        principalTable: "Hashtag",
                        principalColumn: "hashtagId");
                    table.ForeignKey(
                        name: "FK_TweetHashtags_Tweet_tweetId",
                        column: x => x.tweetId,
                        principalTable: "Tweet",
                        principalColumn: "tweetId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_tweetId",
                table: "Comment",
                column: "tweetId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_userId",
                table: "Comment",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Follower_followerUserId",
                table: "Follower",
                column: "followerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Follower_userId",
                table: "Follower",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Like_tweetId",
                table: "Like",
                column: "tweetId");

            migrationBuilder.CreateIndex(
                name: "IX_Like_userId",
                table: "Like",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_notificationType",
                table: "Notification",
                column: "notificationType");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_sourseUserId",
                table: "Notification",
                column: "sourseUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_tweetId",
                table: "Notification",
                column: "tweetId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_userId",
                table: "Notification",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Retweet_tweetId",
                table: "Retweet",
                column: "tweetId");

            migrationBuilder.CreateIndex(
                name: "IX_Retweet_userId",
                table: "Retweet",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedTweet_tweetId",
                table: "SavedTweet",
                column: "tweetId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedTweet_userId",
                table: "SavedTweet",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Tweet_userId",
                table: "Tweet",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_TweetHashtags_hashtagId",
                table: "TweetHashtags",
                column: "hashtagId");

            migrationBuilder.CreateIndex(
                name: "IX_TweetHashtags_tweetId",
                table: "TweetHashtags",
                column: "tweetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Follower");

            migrationBuilder.DropTable(
                name: "Like");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Retweet");

            migrationBuilder.DropTable(
                name: "SavedTweet");

            migrationBuilder.DropTable(
                name: "TweetHashtags");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "NotificationType");

            migrationBuilder.DropTable(
                name: "Hashtag");

            migrationBuilder.DropTable(
                name: "Tweet");

            migrationBuilder.DropTable(
                name: "UserAuthentification");
        }
    }
}
