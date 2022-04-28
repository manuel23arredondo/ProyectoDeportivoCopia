namespace PrimerProyectoClubDeportivoPA2.Web.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AgendaViewModel:Agenda
    {
        [Display(Name = "Miembro")]
        public int MemberId { get; set; }

        [Display(Name = "Sesión de entrenamiento")]
        public int TrainingSessionId { get; set; }

        public IEnumerable<SelectListItem> Members { get; set; }
        public IEnumerable<SelectListItem> TrainingSessions { get; set; }
    }
}
