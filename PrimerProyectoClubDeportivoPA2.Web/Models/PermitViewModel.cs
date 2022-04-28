namespace PrimerProyectoClubDeportivoPA2.Web.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PermitViewModel:Permit
    {
        [Display(Name = "Tipo de membresía")]
        public int MembershipTypeId { get; set; }

        [Display(Name = "Deporte")]
        public int SportId { get; set; }

        public IEnumerable<SelectListItem> MembershipTypes { get; set; }
        public IEnumerable<SelectListItem> Sports { get; set; }
    }
}
