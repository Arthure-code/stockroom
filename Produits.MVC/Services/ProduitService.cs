using Produits.MVC.Interfaces;
using Produits.MVC.Models;

namespace Produits.MVC.Services
{
    /// <summary>
    /// Les regles du catalogue, au meme endroit : le numero du prochain
    /// produit, le produit vedette unique, et le refus de modifier ou de
    /// supprimer ce produit vedette.
    /// </summary>
    public class ProduitService : IProduitService
    {
        private const string RefusModification = "Un produit vedette ne peut pas être modifié.";
        private const string RefusSuppression = "Un produit vedette ne peut pas être supprimé.";

        private readonly IProduitRepository _depot;

        public ProduitService(IProduitRepository depot)
        {
            _depot = depot;
        }

        public async Task<AccueilViewModel> ObtenirAccueilAsync(string? filtre = null)
        {
            var produits = await _depot.LireTousAsync();

            return new AccueilViewModel
            {
                Vedettes = produits.Where(p => p.Vedette && p.Quantite != 0).ToList(),
                ProduitsFiltres = produits
                    .Where(p => !p.Vedette && p.Quantite != 0)
                    .Where(p => string.IsNullOrWhiteSpace(filtre)
                                || (p.Nom ?? string.Empty).Contains(filtre, StringComparison.OrdinalIgnoreCase))
                    .ToList()
            };
        }

        public async Task<Produit?> ObtenirParIdAsync(int id)
        {
            var produits = await _depot.LireTousAsync();
            return produits.SingleOrDefault(p => p.Id == id);
        }

        // Le numero suit le plus grand deja pris, et le nouveau vedette
        // retire la vedette aux autres.
        public async Task<ResultatOperation> AjouterAsync(Produit produit)
        {
            var produits = await _depot.LireTousAsync();

            if (produit.Vedette)
            {
                RetirerLaVedette(produits);
            }

            produit.Id = produits.Count == 0 ? 1 : produits.Max(p => p.Id) + 1;
            produits.Add(produit);
            await _depot.EcrireTousAsync(produits);

            return ResultatOperation.Succes();
        }

        public async Task<ResultatOperation> ModifierAsync(Produit produit)
        {
            var produits = await _depot.LireTousAsync();
            var existant = produits.SingleOrDefault(p => p.Id == produit.Id);

            if (existant == null)
            {
                return ResultatOperation.Introuvable();
            }

            if (existant.Vedette)
            {
                return ResultatOperation.Refus(RefusModification);
            }

            if (produit.Vedette)
            {
                RetirerLaVedette(produits);
            }

            existant.Nom = produit.Nom;
            existant.Description = produit.Description;
            existant.Prix = produit.Prix;
            existant.Quantite = produit.Quantite;
            existant.Image = produit.Image;
            existant.Vedette = produit.Vedette;

            await _depot.EcrireTousAsync(produits);
            return ResultatOperation.Succes();
        }

        public async Task<ResultatOperation> SupprimerAsync(int id)
        {
            var produits = await _depot.LireTousAsync();
            var existant = produits.SingleOrDefault(p => p.Id == id);

            if (existant == null)
            {
                return ResultatOperation.Introuvable();
            }

            if (existant.Vedette)
            {
                return ResultatOperation.Refus(RefusSuppression);
            }

            produits.Remove(existant);
            await _depot.EcrireTousAsync(produits);
            return ResultatOperation.Succes();
        }

        private static void RetirerLaVedette(IEnumerable<Produit> produits)
        {
            foreach (var produit in produits)
            {
                produit.Vedette = false;
            }
        }
    }
}
