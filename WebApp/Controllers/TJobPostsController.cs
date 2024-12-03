﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Models;
using PagedList.Core;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;


namespace WebApp.Controllers
{
    public class TJobPostsController : Controller
    {
        private readonly WebAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public TJobPostsController(WebAppContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }

        // GET: TJobPosts
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var webAppContext = _context.TJobPosts.Include(t => t.Companie).Include(t => t.Poste).Include(t => t.Site).Include(t => t.TypeOfContract);
            return View(await webAppContext.ToListAsync());
        }

        // GET: TJobPosts/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tJobPost = await _context.TJobPosts
                .Include(t => t.Companie)
                .Include(t => t.Poste)
                .Include(t => t.Site)
                .Include(t => t.TypeOfContract)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tJobPost == null)
            {
                return NotFound();
            }

            return View(tJobPost);
        }

        // GET: TJobPosts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CompanieId"] = new SelectList(_context.TCompanies, "Id", "CompanieName");
            ViewData["PosteId"] = new SelectList(_context.TPostes, "Id", "Title");
            ViewData["SiteId"] = new SelectList(_context.TSites, "Id", "Title");
            ViewData["TypeOfContractId"] = new SelectList(_context.TTypeOfContracts, "Id", "ContractName");
            return View();
        }

        // POST: TJobPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,PosteId,Description,TypeOfContractId,SiteId,CompanieId")] TJobPost tJobPost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tJobPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanieId"] = new SelectList(_context.TCompanies, "Id", "CompanieName", tJobPost.CompanieId);
            ViewData["PosteId"] = new SelectList(_context.TPostes, "Id", "Title", tJobPost.PosteId);
            ViewData["SiteId"] = new SelectList(_context.TSites, "Id", "Title", tJobPost.SiteId);
            ViewData["TypeOfContractId"] = new SelectList(_context.TTypeOfContracts, "Id", "ContractName", tJobPost.TypeOfContractId);
            return View(tJobPost);
        }

        // GET: TJobPosts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tJobPost = await _context.TJobPosts.FindAsync(id);
            if (tJobPost == null)
            {
                return NotFound();
            }
            ViewData["CompanieId"] = new SelectList(_context.TCompanies, "Id", "CompanieName", tJobPost.CompanieId);
            ViewData["PosteId"] = new SelectList(_context.TPostes, "Id", "Title", tJobPost.PosteId);
            ViewData["SiteId"] = new SelectList(_context.TSites, "Id", "Title", tJobPost.SiteId);
            ViewData["TypeOfContractId"] = new SelectList(_context.TTypeOfContracts, "Id", "ContractName", tJobPost.TypeOfContractId);
            return View(tJobPost);
        }

        // POST: TJobPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,PosteId,Description,TypeOfContractId,SiteId,CompanieId")] TJobPost tJobPost)
        {
            if (id != tJobPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tJobPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TJobPostExists(tJobPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanieId"] = new SelectList(_context.TCompanies, "Id", "CompanieName", tJobPost.CompanieId);
            ViewData["PosteId"] = new SelectList(_context.TPostes, "Id", "Title", tJobPost.PosteId);
            ViewData["SiteId"] = new SelectList(_context.TSites, "Id", "Title", tJobPost.SiteId);
            ViewData["TypeOfContractId"] = new SelectList(_context.TTypeOfContracts, "Id", "ContractName", tJobPost.TypeOfContractId);
            return View(tJobPost);
        }

        // GET: TJobPosts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tJobPost = await _context.TJobPosts
                .Include(t => t.Companie)
                .Include(t => t.Poste)
                .Include(t => t.Site)
                .Include(t => t.TypeOfContract)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tJobPost == null)
            {
                return NotFound();
            }

            return View(tJobPost);
        }

        // POST: TJobPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tJobPost = await _context.TJobPosts.FindAsync(id);
            if (tJobPost != null)
            {
                _context.TJobPosts.Remove(tJobPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TJobPostExists(int id)
        {
            return _context.TJobPosts.Any(e => e.Id == id);
        }

        [Authorize]
        public async Task<IActionResult> List(
            string searchTitle,
            string searchSkill,
            string searchCompany,
            string searchCity,
            string searchPosition,
            int? minDistance,
            int? maxDistance,
            int? minCompatibility,
            int? maxCompatibility,
            string sortOrder,
            int page = 1,
            int pageSize = 8)
        {
            // Charger les données initiales des offres d'emploi
            var jobPostsQuery = _context.TJobPosts
                .Include(jp => jp.TypeOfContract)
                .Include(jp => jp.Poste)
                .Include(jp => jp.Site)
                .Include(jp => jp.TJobPostRequirements)
                .ThenInclude(r => r.RequirementType)
                .Include(jp => jp.Companie)
                .AsQueryable();

            // Récupérer l'utilisateur connecté
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.Latitude.HasValue || !user.Longitude.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Impossible de calculer la distance sans localisation utilisateur.");

                // Retourner un résultat paginé vide pour éviter les erreurs
                var emptyPagedResult = new PagedResult<TJobPost>
                {
                    Items = new List<TJobPost>(),
                    TotalItems = 0,
                    PageNumber = page,
                    PageSize = pageSize
                };

                return View(emptyPagedResult);
            }

            double userLatitude = user.Latitude.Value;
            double userLongitude = user.Longitude.Value;

            // Calculer l'expérience totale de l'utilisateur
            int userExperience = await CalculateUserExperience(user.Id);

            // Charger les offres d'emploi dans une liste
            var jobPosts = await jobPostsQuery.ToListAsync();

            // Filtrer les postes avec des coordonnées valides
            var jobLocations = jobPosts
                .Where(jp => jp.Site != null && jp.Site.Latitude.HasValue && jp.Site.Longitude.HasValue)
                .Select(jp => (jp.Site.Latitude.Value, jp.Site.Longitude.Value))
                .ToList();

            // Calculer les distances via l'API Geoapify
            var (distances, requestJson, responseJson) = await CalculateDistancesAsync(userLatitude, userLongitude, jobLocations);
            ViewData["RequestJson"] = requestJson;
            ViewData["ResponseJson"] = responseJson;

            int index = 0;
            foreach (var jobPost in jobPosts)
            {
                if (jobPost.Site?.Latitude.HasValue == true && jobPost.Site.Longitude.HasValue)
                {
                    var distanceInMeters = distances.ElementAtOrDefault(index);
                    jobPost.Site.DistanceFromUser = distanceInMeters / 1000.0; // Convertir en km
                    index++;
                }

                // Calculer le score de compatibilité
                jobPost.CompatibilityScore = CalculateCompatibilityScore(user, jobPost);
            }

            // Appliquer les filtres
            if (!string.IsNullOrEmpty(searchTitle))
            {
                searchTitle = searchTitle.ToLower();
                jobPosts = jobPosts.Where(jp =>
                    jp.Title.ToLower().Contains(searchTitle) ||
                    (jp.Companie != null && jp.Companie.CompanieName.ToLower().Contains(searchTitle)) ||
                    jp.TJobPostRequirements.Any(r => r.RequirementType.TypeName.ToLower().Contains(searchTitle)) ||
                    (jp.Site != null && jp.Site.City != null && jp.Site.City.ToLower().Contains(searchTitle)) || // Filtre par ville
                    (jp.Poste != null && jp.Poste.Title != null && jp.Poste.Title.ToLower().Contains(searchTitle)) // Filtre par poste
                ).ToList();
            }

            if (!string.IsNullOrEmpty(searchSkill))
            {
                jobPosts = jobPosts.Where(jp => jp.TJobPostRequirements.Any(r => r.RequirementType.TypeName.Contains(searchSkill, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            if (!string.IsNullOrEmpty(searchCompany))
            {
                jobPosts = jobPosts.Where(jp => jp.Companie != null && jp.Companie.CompanieName.ToLower().Contains(searchCompany.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(searchCity))
            {
                jobPosts = jobPosts.Where(jp => jp.Site != null && jp.Site.City != null && jp.Site.City.ToLower().Contains(searchCity.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(searchPosition))
            {
                jobPosts = jobPosts.Where(jp => jp.Poste != null && jp.Poste.Title != null && jp.Poste.Title.ToLower().Contains(searchPosition.ToLower())).ToList();
            }

            if (minDistance.HasValue)
            {
                jobPosts = jobPosts.Where(jp => jp.Site != null && jp.Site.DistanceFromUser >= minDistance).ToList();
            }

            if (maxDistance.HasValue)
            {
                jobPosts = jobPosts.Where(jp => jp.Site != null && jp.Site.DistanceFromUser <= maxDistance).ToList();
            }

            if (minCompatibility.HasValue)
            {
                jobPosts = jobPosts.Where(jp => jp.CompatibilityScore >= minCompatibility).ToList();
            }

            if (maxCompatibility.HasValue)
            {
                jobPosts = jobPosts.Where(jp => jp.CompatibilityScore <= maxCompatibility).ToList();
            }

            // Appliquer le tri en fonction de `sortOrder`
            jobPosts = sortOrder switch
            {
                "compatibilityAsc" => jobPosts.OrderBy(jp => jp.CompatibilityScore).ToList(),
                "compatibilityDesc" => jobPosts.OrderByDescending(jp => jp.CompatibilityScore).ToList(),
                "distanceAsc" => jobPosts.OrderBy(jp => jp.Site != null ? jp.Site.DistanceFromUser : double.MaxValue).ToList(),
                "distanceDesc" => jobPosts.OrderByDescending(jp => jp.Site != null ? jp.Site.DistanceFromUser : double.MinValue).ToList(),
                _ => jobPosts
            };

            /* Préparer les données pour l'API ChatGPT
            var jsonData = new
            {
                User = new
                {
                    Age = user.DateOfBirth.HasValue ? DateTime.Now.Year - user.DateOfBirth.Value.Year : 0,
                    Experience = userExperience,
                    CurrentPosition = ""
                },
                JobPosts = jobPosts.Select(jp => new
                {
                    jp.Id,
                    jp.Title,
                    jp.CompatibilityScore,
                    DistanceFromUser = jp.Site?.DistanceFromUser,
                    CompanyName = jp.Companie?.CompanieName,
                    City = jp.Site?.City,
                    PositionTitle = jp.Poste?.Title,
                    ContractType = jp.TypeOfContract?.ContractName
                }).ToList()
            };

            // Envoyer les données à ChatGPT
            var apiResponse = await SendToChatGPT(JsonConvert.SerializeObject(jsonData));

            // Traiter la réponse JSON de ChatGPT
            var bestJobPosts = JsonConvert.DeserializeObject<List<BestJobPost>>(apiResponse); 
            
            Je m'étais chauffer mais vas-y c'est payant chatgpt
            
            Cependant j'ai quand meme fait la methode pour l'appeler avec l'envoi du json
            
            */
            
            // Exécution de la pagination
            int totalItems = jobPosts.Count;
            var pagedItems = jobPosts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var pagedResult = new PagedResult<TJobPost>
            {
                Items = pagedItems,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize
            };

            // Ajouter les meilleurs postes pour affichage
            //ViewData["BestJobPosts"] = bestJobPosts;

            // Stocker les valeurs de filtre pour les conserver après soumission
            ViewData["searchTitle"] = searchTitle;
            ViewData["searchSkill"] = searchSkill;
            ViewData["searchCompany"] = searchCompany;
            ViewData["searchCity"] = searchCity;
            ViewData["searchPosition"] = searchPosition;
            ViewData["minDistance"] = minDistance;
            ViewData["maxDistance"] = maxDistance;
            ViewData["minCompatibility"] = minCompatibility;
            ViewData["maxCompatibility"] = maxCompatibility;
            ViewData["sortOrder"] = sortOrder;

            return View(pagedResult);
        }

        
        // Méthode pour envoyer les données à l'API ChatGPT
        private async Task<string> SendToChatGPT(string jsonRequest)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            var apiUrl = "https://api.openai.com/v1/chat/completions";

            var payload = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "system", content = "You are an AI assistant tasked with analyzing job posts." },
                    new { role = "user", content = $"Given this JSON data, return the top 5 job posts for the user with detailed reasoning: {jsonRequest}" }
                }
            };

            var requestBody = JsonConvert.SerializeObject(payload);

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var response = await httpClient.PostAsync(apiUrl, new StringContent(requestBody, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // Classe pour le résultat des meilleurs postes
        public class BestJobPost
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Reason { get; set; }
        }
        
        private async Task<int> CalculateUserExperience(string userId)
        {
            // Récupérer toutes les expériences de l'utilisateur
            var experiences = await _context.TExerces
                .Where(e => e.UserId == userId)
                .ToListAsync();

            int totalYears = 0;

            foreach (var experience in experiences)
            {
                // Si une date de fin n'est pas définie, utiliser la date actuelle
                var endDate = experience.EndDate ?? DateOnly.FromDateTime(DateTime.Now);

                // Calculer la différence en années
                int years = endDate.Year - experience.StartDate.Year;

                // Ajuster si l'année de début est plus tard dans l'année
                if (experience.StartDate > endDate.AddYears(-years))
                {
                    years--;
                }

                totalYears += years;
            }

            return totalYears;
        }




        // Fonction pour normaliser la chaîne de caractères en supprimant la ponctuation et en convertissant en minuscules
        private string NormalizeString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            // Convertir en minuscules
            input = input.ToLowerInvariant();

            // Supprimer la ponctuation
            input = Regex.Replace(input, @"[\p{P}-[.]]+", "");

            // Supprimer les espaces superflus
            input = Regex.Replace(input, @"\s+", " ").Trim();

            return input;
        }

        private double CalculateCompatibilityScore(ApplicationUser user, TJobPost jobPost)
        {
            var userRequirements = _context.TUserRequirements
                .Where(ur => ur.UserId == user.Id)
                .ToList();

            double totalScore = 0;
            int totalRequirements = jobPost.TJobPostRequirements.Count;

            foreach (var requirement in jobPost.TJobPostRequirements)
            {
                var userRequirement = userRequirements.FirstOrDefault(ur => ur.RequirementTypeId == requirement.RequirementTypeId);
                if (userRequirement == null)
                {
                    continue; // Sauter si l'utilisateur n'a pas cette compétence
                }

                double score = 0;

                // Évaluer la différence en évaluation personnelle
                double evaluationDifference = Math.Abs(userRequirement.PersonnalEvaluation - requirement.Evaluation);
                score += (5 - evaluationDifference); // score sur 5

                // Comparaison de l'expérience
                int userExperienceYears = CalculateYearsOfExperience(userRequirement.StartDate, userRequirement.EndDate);
                score += requirement.YearsOfExperience.HasValue && requirement.YearsOfExperience > 0
                    ? (userExperienceYears >= requirement.YearsOfExperience
                        ? 5
                        : (double)userExperienceYears / requirement.YearsOfExperience.Value * 5)
                    : 0;

                totalScore += score;
            }

            return totalRequirements > 0 ? (totalScore / (totalRequirements * 10)) * 100 : 0; // pourcentage
        }

        private int CalculateYearsOfExperience(DateTime startDate, DateTime? endDate)
        {
            var end = endDate ?? DateTime.Now;
            int years = end.Year - startDate.Year;
            if (startDate > end.AddYears(-years)) years--;
            return years;
        }



        private async Task<(List<double> distances, string requestJson, string responseJson)> CalculateDistancesAsync(double userLatitude, double userLongitude, List<(double latitude, double longitude)> jobLocations)
        {
            var apiKey = _configuration["Geoapify:ApiKey"];
            var url = $"https://api.geoapify.com/v1/routematrix?apiKey={apiKey}";

            var payload = new
            {
                mode = "drive",
                sources = new[] { new { location = new[] { userLongitude, userLatitude } } },
                targets = jobLocations.Select(location => new { location = new[] { location.longitude, location.latitude } }).ToArray()
            };

            var requestJson = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(url, new StringContent(requestJson, Encoding.UTF8, "application/json"));
                var responseBody = await response.Content.ReadAsStringAsync();

                var matrixResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RouteMatrixResponse>(responseBody);

                var distances = matrixResponse?.sources_to_targets?[0]?.Select(entry => entry.distance).ToList() ?? new List<double>();

                return (distances, requestJson, responseBody);
            }
        }

        public class RouteMatrixResponse
        {
            public List<List<DistanceEntry>> sources_to_targets { get; set; }
        }

        public class DistanceEntry
        {
            public double distance { get; set; }
            public int time { get; set; }
            public int source_index { get; set; }
            public int target_index { get; set; }
        }
    }
}
