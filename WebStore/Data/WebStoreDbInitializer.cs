using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WebStore.DAL.Context;
using WebStore.Domain.Identity;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _Logger;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;

        public WebStoreDbInitializer(WebStoreDB db,
            ILogger<WebStoreDbInitializer> Logger,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager
            )
        {
            _db = db;
            _Logger = Logger;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
        }

        public void Initialize()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инитифлизация базы данных...");

            //_db.Database.EnsureDeleted();
            //_db.Database.EnsureCreated(); 

            var db = _db.Database;

            if (db.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("Выполнение Миграции...");

                db.Migrate();

                _Logger.LogInformation("Выполнение миграции выполнено успешно");
            }
            else

                _Logger.LogInformation("Базы данных находятся в актуальной версии  ({0:0.0###}) c )",
                    timer.Elapsed.TotalSeconds);

            try
            {
                InitializeProducts();
                InitializeIdentityAsync().Wait();
            }
            catch (Exception error)
            {

                _Logger.LogError(error, "Ошибка при выполнении инициализации БД");
                throw;
            }

            _Logger.LogInformation("Инициализация базы данных успешна {0}",
                  timer.Elapsed.TotalSeconds);
        }
        private void InitializeProducts()
        {
            var timer = Stopwatch.StartNew();


            if (_db.Products.Any())
            {
                _Logger.LogInformation("Инициализация БД товарами не требуется");
                return;
            }



            _Logger.LogInformation("Инициализация товаров...");

            _Logger.LogInformation("Добавление секций...");

            using (_db.Database.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                _db.Database.CommitTransaction();
            }

            using (_db.Database.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Бренды успешно добавлены в БД");

            _Logger.LogInformation("Добавление товаров...");
            using (_db.Database.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Товары успешно добавлены в БД");

            _Logger.LogInformation("Инициализация товаров выполнена успешно ({0:0.0###})",
                timer.Elapsed.TotalSeconds);
        }

        // Метод который будет добавлять данные системы Identity 

        private async Task InitializeIdentityAsync()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация системы Identity...");

            //Добаление роллей
            async Task CheckRole(string RoleName)
            {
                if (!await _RoleManager.RoleExistsAsync(RoleName))//Уточняем роль 
                {
                    _Logger.LogInformation("Роль {0} отсутствует. Создаю...");
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
                    _Logger.LogInformation("Роль {0} создана успешно");
                }                               
            }

            await CheckRole(Role.Administrator);
            await CheckRole(Role.Users);

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                _Logger.LogInformation("Отсутствует учетная запись администратора");
                var admin = new User
                {
                    UserName = User.Administrator
                };
                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded)
                {
                    _Logger.LogInformation("Учетная запись администратора создана успешно.");
                    await _UserManager.AddToRoleAsync(admin, Role.Administrator);
                    _Logger.LogInformation("Учетная запись администратора наделена ролью {0}", Role.Administrator);
                }
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"Ошибка при учетной записи администратора: {string.Join(",", errors)}");
                }
            }

            _Logger.LogInformation("Инициализация сиситемы Identity завершено успешно за {0.0.0###}c", timer.Elapsed.Seconds);
        }
    }
}