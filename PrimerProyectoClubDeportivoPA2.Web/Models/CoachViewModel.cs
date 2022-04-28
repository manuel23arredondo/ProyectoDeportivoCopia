namespace PrimerProyectoClubDeportivoPA2.Web.Models
{
    using Microsoft.AspNetCore.Http;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using System.ComponentModel.DataAnnotations;
    public class CoachViewModel : Coach
    {
        [Display(Name = "Foto")]
        public IFormFile ImageFile { get; set; }
    }
}
