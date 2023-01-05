// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using D_real_social_app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using D_real_social_app.Data;
using D_real_social_app.Models;
using Microsoft.EntityFrameworkCore;

namespace D_real_social_app.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly SocialAppContext _context;
        public string UPhoto { get; set; }


        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            SocialAppContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [DataType(DataType.Upload)]
            [Display(Name = "Photo")]
            public IFormFile Photo { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);

            var firstName = "";
            var lastName = "";

            try
            {
                var sqlRes = await _context.User.FromSqlRaw("SELECT * FROM [User] WHERE Id = '" + userId + "'").ToListAsync();
                firstName = sqlRes[0].FirstName;
                lastName = sqlRes[0].LastName;
                UPhoto = sqlRes[0].Photo;
            }
            catch (Exception e)
            { }

            Username = userName;

            Input = new InputModel
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var firstName = "";
            var lastName = "";

            try
            {
                var sqlRes = await _context.User.FromSqlRaw("SELECT * FROM [User] WHERE Id = '" + userId + "'").ToListAsync();
                firstName = sqlRes[0].FirstName;
                lastName = sqlRes[0].LastName;
            }
            catch (Exception e)
            {
                StatusMessage = "Unexpected error when trying to save your data.";
                return RedirectToPage();
            }

            var fileName = "";
            if (Input.Photo != null)
            {
                var end = Input.Photo.FileName.Split(".")[Input.Photo.FileName.Split(".").Length - 1];

                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[32];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);

                fileName = "/img/uploads/" + finalString + "." + end;
                using (Stream fileStream = new FileStream("/app/wwwroot" + fileName, FileMode.Create))
                {
                    await Input.Photo.CopyToAsync(fileStream);
                }
            }

            if (Input.FirstName != firstName)
            {
                var sql = "UPDATE [User] SET FirstName = '" + Input.FirstName + "' WHERE Id = '" + userId + "'";
                _context.Database.ExecuteSqlRaw(sql);
            }

            if (Input.LastName != lastName)
            {
                var sql = "UPDATE [User] SET LastName = '" + Input.LastName + "' WHERE Id = '" + userId + "'";
                _context.Database.ExecuteSqlRaw(sql);
            }

            if (fileName != "")
            {
                var sql = "UPDATE [User] SET Photo = '" + fileName + "' WHERE Id = '" + userId + "'";
                _context.Database.ExecuteSqlRaw(sql);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
