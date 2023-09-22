using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rater.Data;
using Rater.Models;
using Rater.ViewModels;
using System.Diagnostics;

namespace Rater.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminPanelController: Controller
    {
        private const string admin = "Administrator";
        private readonly UserManager<User> userManager;
        private readonly ApplicationDbContext context;
        private readonly ILogger<HomeController> logger;

        public AdminPanelController(UserManager<User> userManager, ILogger<HomeController> logger, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.context = context;
        }

        [OutputCache(Duration = 600)]
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            Dictionary<int, bool> admins = await GetAdmins();
            AdminPanelViewModel model = new()
            {
                UserRows = searchString.IsNullOrEmpty()
                    ? await GetUsers()
                    : await GetFilteredUsers(searchString),
                Admins = admins
            };
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangeUserStatus(string username, string isBlocked) {
            logger.LogError($"ChangeUserStatus username: {username}, isBlocked: {isBlocked}");
            if (!bool.TryParse(isBlocked, out bool blocked))
            {
                logger.LogError($"ChangeUserStatus parsing: FAIL");
                return RedirectToAction("Index");
            }
            logger.LogWarning($"ChangeUserStatus parsing: succeeded:{blocked}");

            DateTime newDate;
            newDate = blocked ? DateTime.UtcNow : DateTime.MaxValue;
            User user = await GetUser(username);
            await userManager.SetLockoutEndDateAsync(user, newDate);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SetAdministrator(string username)
        {
            User user = await GetUser(username);
            var addResult = await userManager.AddToRoleAsync(user, admin);
            ThrowOnFail(addResult);
            user.LockoutEnabled = false;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string username)
        {
            User user = await GetUser(username);
            var deleteResult = await userManager.DeleteAsync(user);
            ThrowOnFail(deleteResult);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<User> GetUser(string username)
        {
            return await userManager.FindByNameAsync(username)
                            ?? throw new KeyNotFoundException();
        }

        private static void ThrowOnFail(IdentityResult addResult)
        {
            if (!addResult.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred.");
            }
        }

        private async Task<Dictionary<int, bool>> GetAdmins()
        {
            return await context.UserRoles
                .Select(user => user.UserId)
                .ToDictionaryAsync(id => id, id => true);
        }

        private async Task<IEnumerable<UserRow>> GetUsers()
        {
            IEnumerable<UserRow> users = await userManager.Users
               .OrderBy(user => user.UserName)
               .Select(user => new UserRow(
                   user.Id,
                   user.UserName,
                   user.Email!,
                   user.LockoutEnd > DateTime.Now))
                   .ToListAsync<UserRow>();
            return users;
        }

        private async Task<IEnumerable<UserRow>> GetFilteredUsers(string searchString)
        {
            IEnumerable<UserRow> users = await userManager.Users
               .Where(user => user.UserName.Contains(searchString))
               .OrderBy(user => user.UserName)
               .Select(user => new UserRow(
                   user.Id,
                   user.UserName,
                   user.Email!,
                   user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.Now))
                   .ToListAsync<UserRow>();
            return users;
        }
    }
}