using CleanArchMvc.Domain.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchMvc.Infra.Data.Identity
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRoleInitial(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedRoles()
        {
            var rolesTemplate = new List<string> { "User", "Admin" };

            foreach (var roleTemplate in rolesTemplate)
            {
                if (!_roleManager.RoleExistsAsync(roleTemplate).Result)
                {
                    var role = new IdentityRole();
                    role.Name = roleTemplate;
                    role.NormalizedName = roleTemplate.ToUpper();

                    IdentityResult result = _roleManager.CreateAsync(role).Result;
                }
            }
        }

        public void SeedUsers()
        {
            var usersTemplate = new List<string>() { "user@localhost", "admin@localhost" };

            foreach (var userTemplate in usersTemplate)
            {
                if (_userManager.FindByEmailAsync(userTemplate).Result == null)
                {
                    var user = new ApplicationUser();
                
                    // Existem mais opções de propriedades para usar com ApplicationUser, mas somente estas foram cogitadas neste projeto.
                    user.UserName = userTemplate;
                    user.Email = userTemplate;
                    user.NormalizedUserName = userTemplate.ToUpper();
                    user.NormalizedEmail = userTemplate.ToUpper();
                    user.EmailConfirmed = true;
                    user.LockoutEnabled = false;
                    user.SecurityStamp = Guid.NewGuid().ToString();

                    IdentityResult result = _userManager.CreateAsync(user, "Numsey#2021").Result;

                    if (result.Succeeded)
                    {
                        var role = userTemplate.Split("@").First();
                        role = $"{char.ToUpper(role[0])}{role[1..]}";

                        _userManager.AddToRoleAsync(user, role).Wait();
                    }
                }
            }
        }
    }
}
