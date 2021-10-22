using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Blog
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

            var context = new MyDbContext(loggerFactory);
            context.Database.EnsureCreated();
            InitializeData(context);

            var posts = context.BlogPosts
                .Select(post => new
                {
                    Title = post.Title,
                    Id = post.Id,
                    Comment = post.Comments
                        .OrderByDescending(comment => comment.CreatedDate).FirstOrDefault()
                }).AsEnumerable()
                .OrderByDescending(result => result.Comment.CreatedDate)
                .ToList(); 
            

            foreach (var post in posts)
            {
                var date =  post.Comment.CreatedDate.ToString("d MMMM");
                Console.WriteLine($"=> {post.Title} - {post.Comment.Text} ({date})");
            }

           
           
        }


      

        private static void InitializeData(MyDbContext context)
        {
            context.BlogPosts.Add(new BlogPost("Post1")
            {
                Comments = new List<BlogComment>()
                {
                    new BlogComment("1", new DateTime(2020, 3, 4)),
                    new BlogComment("2", new DateTime(2020, 3, 1)),
                }
            });
            context.BlogPosts.Add(new BlogPost("Post2")
            {
                Comments = new List<BlogComment>()
                {
                    new BlogComment("3", new DateTime(2020, 3, 5)),
                    new BlogComment("4", new DateTime(2020, 3, 6)),
                }
            });
            context.BlogPosts.Add(new BlogPost("Post3")
            {
                Comments = new List<BlogComment>()
                {
                    new BlogComment("5", new DateTime(2020, 2, 7)),
                    new BlogComment("6", new DateTime(2020, 2, 9)),
                    new BlogComment("7", new DateTime(2020, 2, 8)),
                }
            });
            context.SaveChanges();
        }

      
    }
}