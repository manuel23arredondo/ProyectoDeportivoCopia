namespace PrimerProyectoClubDeportivoPA2.Web.Models
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class MemberViewModel : Member
    {
        [Display(Name = "Foto")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Tipo de membresía")]
        public int MembershipTypeId { get; set; }

        public IEnumerable<SelectListItem> MembershipTypes { get; set; }
    }
}
