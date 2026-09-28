using Produits.MVC.Models;

namespace Produits.MVC.Interfaces
{
    /// <summary>
    /// Les regles du catalogue : un seul produit vedette a la fois, et le
    /// produit vedette ne se modifie ni ne se supprime. Le controleur ne
    /// connait que cette interface.
    /// </summary>
    public interface IProduitService
    {
        /// <summary>Le produit vedette et les autres, filtres par nom si un nom est donne.</summary>
        Task<AccueilViewModel> ObtenirAccueilAsync(string? filtre = null);

        Task<Produit?> ObtenirParIdAsync(int id);

        Task<ResultatOperation> AjouterAsync(Produit produit);

        Task<ResultatOperation> ModifierAsync(Produit produit);

        Task<ResultatOperation> SupprimerAsync(int id);
    }
}
