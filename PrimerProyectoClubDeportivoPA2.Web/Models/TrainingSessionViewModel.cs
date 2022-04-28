namespace PrimerProyectoClubDeportivoPA2.Web.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class TrainingSessionViewModel : TrainingSession
    {
        [Display(Name = "Coaches")]
        public int CoachId { get; set; }

        [Display(Name = "Horario")]
        public int ScheduleId { get; set; }

        [Display(Name = "Deportes")]
        public int SportId { get; set; }

        public IEnumerable<SelectListItem> Coaches { get; set; }
        public IEnumerable<SelectListItem> Schedules { get; set; }
        public IEnumerable<SelectListItem> Sports { get; set; }
    }
}
