using System.Threading.Tasks;
using EPiServer.Authorization;
using EPiServer.Shell.Security;
using Microsoft.AspNetCore.Identity;

namespace FLS.CoffeeDesk.Infrastructure
{
    public class GrantDefaultRoles
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UIRoleProvider _roleProvider;

        public GrantDefaultRoles(
            RoleManager<IdentityRole> roleManager,
            UIRoleProvider roleProvider)
        {
            _roleManager = roleManager;
            _roleProvider = roleProvider;
        }

        public async Task GrantTo(string userName)
        {
            const string commerceadmins = "CommerceAdmins";
            await _roleManager.CreateAsync(new IdentityRole(Roles.Administrators));
            await _roleManager.CreateAsync(new IdentityRole(commerceadmins));
            await _roleProvider.AddUserToRolesAsync(userName, new[] { Roles.Administrators, commerceadmins });
        }
    }
}