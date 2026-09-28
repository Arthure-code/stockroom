using Produits.MVC.Models;

namespace Produits.MVC.Interfaces
{
    public interface IProduit
    {
        Task<Produit?> GetProduiByIdAsync(int id);
        Task<List<Produit>> GetAllProduitsAsync();
        Task EnregistreToutAsync(List<Produit> produits);
        Task EnregistreProduitAsync(Produit produit);
        Task<List<Produit>> InitialisationAsync();
    }
}
