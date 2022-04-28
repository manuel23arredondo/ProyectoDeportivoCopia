namespace PrimerProyectoClubDeportivoPA2.Web.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AdditionalSkillViewModel : AdditionalSkill
    {
        [Display(Name = "Deporte")]
        public int SportId { get; set; }

        [Display(Name = "Nombre del coach")]
        public int CoachId { get; set; }

        public IEnumerable<SelectListItem> Sports { get; set; }
        public IEnumerable<SelectListItem> Coaches { get; set; }
    }
}
