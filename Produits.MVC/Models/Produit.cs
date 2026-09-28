using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Produits.MVC.Models
{
    public class Produit
    {
        [Display(Name = "Identifiant")]
        public int Id { get; set; }

        [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ ]+$", ErrorMessage = "Le nom doit contenir uniquement des lettres, sans chiffres ni caractères spéciaux.")]
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(100, ErrorMessage = "Le nom ne doit pas dépasser 100 caractères.")]
        public string? Nom { get; set; }

        [Required(ErrorMessage = "La description est obligatoire.")]
        [StringLength(250, ErrorMessage = "La description ne doit pas dépasser 250 caractères.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Le prix doit être un nombre positif.")]
        [RegularExpression(@"^\d+([,\.]\d{1,2})?$", ErrorMessage = "Le prix doit être un décimal, les valeurs négatives ne sont autorisées et autorisent deux décimales au plus")]
        public decimal Prix { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "La quantité doit être un nombre entier.")]
        [Required(ErrorMessage = "La quantité est obligatoire.")]
        [Range(0, 150, ErrorMessage = "La quantité doit être comprise entre 0 et 150.")]
        public int Quantite { get; set; }

        [Required(ErrorMessage = "L'image est obligatoire.")]
        [RegularExpression(@"^.+\.(png|jpg)$", ErrorMessage = "L'image doit avoir une extension .png ou .jpg.")]
        public string? Image { get; set; }

        public bool Vedette { get; set; } = false;

        public override string ToString()
        {
            return $"Produit(Id={Id}, Nom='{Nom}', Description='{Description}', Prix={Prix}, Quantite={Quantite}, Image='{Image}', Vedette={Vedette})";
        }


    }
}
