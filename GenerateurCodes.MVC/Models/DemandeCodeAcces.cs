using System.ComponentModel.DataAnnotations;

namespace GenerateurCodes.MVC.Models
{
    public class DemandeCodeAcces
    {
        public int Id { get; set; }

        [MinLength(2, ErrorMessage ="Le nom doit avoir minimun 2 caractères")]
        [Display(Name = "Nom du demandeur")]
        [Required(ErrorMessage = "Le nom du demandeur est obligatoire")]
        public string NomDemandeur { get; set; }

        [Display(Name = "Date de naissance du demandeur")]
        [Required(ErrorMessage = "La date de naissance du demandeur est obligatoire")]
        public DateTime DateNaissanceDemandeur { get; set; }

        [Required(ErrorMessage ="Le NAS est obligatoire")]
        [Range(100000000,999999999, ErrorMessage ="Le NAS doit contenir 9 chiffres")]
        [Display(Name = "Numéro d'assurance sociale")]
        public int NAS { get; set; }

		[Display(Name = "Raison de la demande")]
		public string RaisonDemande { get; set; }

    }
}
