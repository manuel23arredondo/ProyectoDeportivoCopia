namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Member : IEntity
    {
        [Display(Name = "Clave")]
        public int Id { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        public User User { get; set; }
        public MembershipType MembershipType { get; set; }
        public ICollection<Agenda> Agendas { get; set; }
    }
}
