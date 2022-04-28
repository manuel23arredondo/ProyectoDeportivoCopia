namespace PrimerProyectoClubDeportivoPA2.Web.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Agenda : IEntity
    {
        [Display(Name = "Clave")]
        public int Id { get; set; }

        public TrainingSession TrainingSession { get; set; }
        public Member Member { get; set; }
    }
}
