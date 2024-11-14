namespace WebApp.Models // Remplacez "WebApp.Models" par le namespace approprié de votre projet
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } // La liste des éléments paginés
        public int TotalItems { get; set; } // Nombre total d'éléments
        public int PageNumber { get; set; } // Numéro de la page actuelle
        public int PageSize { get; set; } // Nombre d'éléments par page

        // Calcul automatique du nombre total de pages
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
