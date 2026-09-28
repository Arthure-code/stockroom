using Produits.MVC.Models;

namespace Produits.MVC.Interfaces
{
    /// <summary>
    /// Acces au fichier de produits. Seule cette couche lit et ecrit le CSV :
    /// rien au-dessus ne sait ou les produits sont ranges.
    /// </summary>
    public interface IProduitRepository
    {
        Task<List<Produit>> LireTousAsync();

        Task EcrireTousAsync(IEnumerable<Produit> produits);
    }
}
