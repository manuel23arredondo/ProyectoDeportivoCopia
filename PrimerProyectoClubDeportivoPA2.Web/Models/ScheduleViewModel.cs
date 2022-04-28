namespace PrimerProyectoClubDeportivoPA2.Web.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class ScheduleViewModel : Schedule
    {       
        [Display(Name = "Instalación")]
        public int FacilityId { get; set; }

        [Display(Name = "Dia")]
        public int WeekDayId { get; set; }

        public IEnumerable<SelectListItem> Facilities { get; set; }
        public IEnumerable<SelectListItem> WeekDays { get; set; }
    }
}
