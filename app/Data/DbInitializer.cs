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
                new User{Photo="https://www.hostpapa.com/knowledgebase/wp-content/uploads/2018/04/1-13.png", FirstName="Lojze", LastName="Novak"},
                new User{Photo="https://ps.w.org/user-avatar-reloaded/assets/icon-256x256.png", FirstName="Janez", LastName="Komel"}
            };

            context.User.AddRange(users);
            context.SaveChanges();

            /* var posts = new Post[]
             {
                 new Post{ UserID="43bc8695-e72f-43f3-aa35-5d90fbb46cdb", Text="Hello World, first post.", Photo="https://images.pexels.com/photos/9866817/pexels-photo-9866817.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", Timestamp=DateTime.Now},
                 new Post{ UserID="43bc8695-e72f-43f3-aa35-5d90fbb46cdb", Text="Hello World, post 2.", Photo="https://images.pexels.com/photos/9866817/pexels-photo-9866817.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", Timestamp=DateTime.Now},
                 new Post{ UserID="99d413c8-333a-47d9-ad11-1d9ef830827a", Text="Hello World, post 3.", Photo="https://images.pexels.com/photos/9866817/pexels-photo-9866817.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", Timestamp=DateTime.Now},
                 new Post{ UserID="99d413c8-333a-47d9-ad11-1d9ef830827a", Text="Hello World, post 4.", Photo="https://images.pexels.com/photos/9866817/pexels-photo-9866817.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", Timestamp=DateTime.Now},
             };

             context.Post.AddRange(posts);
             context.SaveChanges();

             var comments = new Comment[]
             {
                 new Comment{UserID="43bc8695-e72f-43f3-aa35-5d90fbb46cdb", PostID=1, Text="First comment."}
             };

             context.Comment.AddRange(comments);
             context.SaveChanges();

             var connections = new Connection[]
             {
                 new Connection{UserID="43bc8695-e72f-43f3-aa35-5d90fbb46cdb", UserID2="99d413c8-333a-47d9-ad11-1d9ef830827a"}
             };

             context.Connection.AddRange(connections);
             context.SaveChanges();
             */
        }
    }
}