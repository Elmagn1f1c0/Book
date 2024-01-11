using Books.Data.Enums;
using Books.Data.Static;
using Books.Models;
using Microsoft.AspNetCore.Identity;

namespace Books.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                //Store
                if (!context.Stores.Any())
                {
                    context.Stores.AddRange(new List<Store>()
                    {
                        new Store()
                        {
                            Name = "Store 1",
                            Logo = "https://dotnethow.net/images/cinemas/cinema-1.jpeg",
                            Description = "This is the description of the first store"
                        },
                        new Store()
                        {
                            Name = "Store 2",
                            Logo = "https://dotnethow.net/images/cinemas/cinema-2.jpeg",
                            Description = "This is the description of the second store"
                        },
                        new Store()
                        {
                            Name = "Store 3",
                            Logo = "https://dotnethow.net/images/cinemas/cinema-3.jpeg",
                            Description = "This is the description of the Third store"
                        },
                        new Store()
                        {
                            Name = "Store 4",
                            Logo = "https://dotnethow.net/images/cinemas/cinema-4.jpeg",
                            Description = "This is the description of the fourth store"
                        },
                        new Store()
                        {
                            Name = "Store 5",
                            Logo = "https://dotnethow.net/images/cinemas/cinema-5.jpeg",
                            Description = "This is the description of the fifth store"
                        },
                    });
                    context.SaveChanges();
                }
                //Authors
                if (!context.Authors.Any())
                {
                    context.Authors.AddRange(new List<Author>()
                    {
                        new Author()
                        {
                            FullName = "Author 1",
                            Bio = "This is the Bio of the first Author",
                            ProfilePictureURL = "https://dotnethow.net/images/actors/actor-1.jpeg"

                        },
                        new Author()
                        {
                            FullName = "Author 2",
                            Bio = "This is the Bio of the second Author",
                            ProfilePictureURL = "https://dotnethow.net/images/actors/actor-2.jpeg"
                        },
                        new Author()
                        {
                            FullName = "Author 3",
                            Bio = "This is the Bio of the third Author",
                            ProfilePictureURL = "https://dotnethow.net/images/actors/actor-3.jpeg"
                        },
                        new Author()
                        {
                            FullName = "Author 4",
                            Bio = "This is the Bio of the fourth Author",
                            ProfilePictureURL = "https://dotnethow.net/images/actors/actor-4.jpeg"
                        },
                        new Author()
                        {
                            FullName = "Author 5",
                            Bio = "This is the Bio of the fifth Author",
                            ProfilePictureURL = "https://dotnethow.net/images/actors/actor-5.jpeg"
                        }
                    });
                    context.SaveChanges();
                }
                //Publishers
                if (!context.Publishers.Any())
                {
                    context.Publishers.AddRange(new List<Publisher>()
                    {
                        new Publisher()
                        {
                            FullName = "Publisher 1",
                            Bio = "This is the Bio of the first Publisher",
                            ProfilePictureURL = "https://dotnethow.net/images/producers/producer-1.jpeg"

                        },
                        new Publisher()
                        {
                            FullName = "Publisher 2",
                            Bio = "This is the Bio of the second Publisher",
                            ProfilePictureURL = "https://dotnethow.net/images/producers/producer-2.jpeg"
                        },
                        new Publisher()
                        {
                            FullName = "Publisher 3",
                            Bio = "This is the Bio of the third Publisher",
                            ProfilePictureURL = "https://dotnethow.net/images/producers/producer-3.jpeg"
                        },
                        new Publisher()
                        {
                            FullName = "Publisher 4",
                            Bio = "This is the Bio of the fourth Publisher",
                            ProfilePictureURL = "https://dotnethow.net/images/producers/producer-4.jpeg"
                        },
                        new Publisher()
                        {
                            FullName = "Publisher 5",
                            Bio = "This is the Bio of the fifth Publisher",
                            ProfilePictureURL = "https://dotnethow.net/images/producers/producer-5.jpeg"
                        }
                    });
                    context.SaveChanges();
                }
                //Books
                if (!context.Books.Any())
                {
                    context.Books.AddRange(new List<Book>()
                    {
                        new Book()
                        {
                            Title = "Lion King",
                            ISBN = "222-333-XY",
                            Description = "This is an interesting book",
                            Price = 39.50,
                            ImageURL = "https://lumiere-a.akamaihd.net/v1/images/p_thelionking_19752_1_0b9de87b.jpeg?region=0%2C0%2C540%2C810",
                            YearPublished = DateTime.Now.AddDays(-10),
                            EndDate = DateTime.Now.AddDays(10),
                            StoreId = 3,
                            PublisherId = 3,
                            BookCategory = BookCategory.Documentary
                        },
                        new Book()
                        {
                            Title = "Harry Porter",
                            ISBN = "200-353-AB",
                            Description = "This is a magical book",
                            Price = 29.50,
                            ImageURL = "data:image/jpeg;base64,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",
                            YearPublished = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(3),
                            StoreId = 1,
                            PublisherId = 1,
                            BookCategory = BookCategory.Action
                        },
                        
                    });
                    context.SaveChanges();
                }
                //Authors & Books
                if (!context.Authors_Books.Any())
                {
                    context.Authors_Books.AddRange(new List<Author_Book>()
                    {
                        new Author_Book()
                        {
                            AuthorId = 1,
                            BookId = 1
                        },
                        new Author_Book()
                        {
                            AuthorId = 2,
                            BookId = 1
                        },

                        
                    });
                    context.SaveChanges();
                }
            }

        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "admin-user",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }


                string appUserEmail = "user@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Application User",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
