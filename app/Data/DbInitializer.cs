using D_real_social_app.Data;
using D_real_social_app.Models;
using System;
using System.Linq;

namespace D_real_social_app.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SocialAppContext context)
        {
            context.Database.EnsureCreated();

            // Look for any user.
            if (context.User.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User{Photo="", FirstName="Lojze", LastName="Novak"},
                new User{Photo="", FirstName="Janez", LastName="Komel"}
            };

            context.User.AddRange(users);
            context.SaveChanges();

            var posts = new Post[]
            {
                new Post{ UserID=1, Text="Hello World, first post."}
            };

            context.Post.AddRange(posts);
            context.SaveChanges();

            var comments = new Comment[]
            {
                new Comment{UserID=1, PostID=1, Text="First comment."}
            };

            context.Comment.AddRange(comments);
            context.SaveChanges();

            var connections = new Connection[]
            {
                new Connection{UserID=1, UserID2=2}
            };

            context.Connection.AddRange(connections);
            context.SaveChanges();
        }
    }
}