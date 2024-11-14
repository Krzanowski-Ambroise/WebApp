// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Areas.Identity.Data;
using System.Net.Http;
using Newtonsoft.Json.Linq;


namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private async Task<(double? Latitude, double? Longitude)> GetCoordinatesAsync(string address)
        {
            // Remplacez par votre clé API Geoapify
            string apiKey = _configuration["Geoapify:ApiKey"];
            string url = $"https://api.geoapify.com/v1/geocode/search?text={Uri.EscapeDataString(address)}&apiKey={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                    if (jsonResponse.features.Count > 0)
                    {
                        double latitude = jsonResponse.features[0].geometry.coordinates[1];
                        double longitude = jsonResponse.features[0].geometry.coordinates[0];
                        return (latitude, longitude);
                    }
                }
            }

            // Retourne null si les coordonnées ne peuvent pas être récupérées
            return (null, null);
        }


        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
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

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Street")]
            public string Street { get; set; }

            [Display(Name = "City")]
            public string City { get; set; }

            [Display(Name = "Postcode")]
            public int? Postcode { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Date of Birth")]
            public DateTime? DateOfBirth { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Street = user.Street,
                City = user.City,
                Postcode = user.Postcode,
                DateOfBirth = user.DateOfBirth
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

            // Mise à jour du numéro de téléphone
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

            // Mise à jour des autres champs
            bool addressChanged = Input.Street != user.Street || Input.City != user.City || Input.Postcode != user.Postcode;

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.Street = Input.Street;
            user.City = Input.City;
            user.Postcode = Input.Postcode;
            user.DateOfBirth = Input.DateOfBirth;

            // Si un champ d'adresse est supprimé, mettre Latitude et Longitude à null
            if (addressChanged && (string.IsNullOrEmpty(user.Street) || string.IsNullOrEmpty(user.City) || !user.Postcode.HasValue))
            {
                user.Latitude = null;
                user.Longitude = null;
            }
            else if (addressChanged)
            {
                // Si l'adresse a changé et est complète, récupérez la nouvelle latitude et longitude
                var address = $"{user.Street}, {user.City} {user.Postcode}";
                var (latitude, longitude) = await GetCoordinatesAsync(address);
                if (latitude.HasValue && longitude.HasValue)
                {
                    user.Latitude = latitude.Value;
                    user.Longitude = longitude.Value;
                }
                else
                {
                    // Ajoutez un message d'erreur si la récupération échoue
                    ModelState.AddModelError(string.Empty, "Unable to retrieve latitude and longitude. Please verify your address.");
                    await LoadAsync(user);
                    return Page();
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update profile.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }




    }
}
