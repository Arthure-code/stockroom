using Microsoft.AspNetCore.Mvc;
using Produits.MVC.Interfaces;
using Produits.MVC.Models;

namespace Produits.MVC.Controllers
{
    /// <summary>
    /// Recoit les demandes, appelle le service et choisit la vue. Aucune
    /// regle du catalogue ne vit ici.
    /// </summary>
    public class ProduitsController : Controller
    {
        private readonly IProduitService _service;

        public ProduitsController(IProduitService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Accueil()
        {
            return View(await _service.ObtenirAccueilAsync());
        }

        public async Task<IActionResult> Filtre(string libelle)
        {
            return View(nameof(Accueil), await _service.ObtenirAccueilAsync(libelle));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produit produit)
        {
            if (!ModelState.IsValid)
            {
                return View(produit);
            }

            await _service.AjouterAsync(produit);
            return RedirectToAction(nameof(Accueil));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var produit = await _service.ObtenirParIdAsync(id);
            return produit == null ? NotFound() : View(produit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produit produit)
        {
            if (!ModelState.IsValid)
            {
                return View(produit);
            }

            var resultat = await _service.ModifierAsync(produit);
            if (resultat.Reussi)
            {
                return RedirectToAction(nameof(Accueil));
            }

            if (resultat.Message == null)
            {
                return NotFound();
            }

            ModelState.AddModelError(string.Empty, resultat.Message);
            return View(produit);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var produit = await _service.ObtenirParIdAsync(id);
            return produit == null ? NotFound() : View(produit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Produit produit)
        {
            var resultat = await _service.SupprimerAsync(id);
            if (resultat.Reussi)
            {
                return RedirectToAction(nameof(Accueil));
            }

            if (resultat.Message == null)
            {
                return NotFound();
            }

            var existant = await _service.ObtenirParIdAsync(id);
            ModelState.AddModelError(string.Empty, resultat.Message);
            return View(existant);
        }
    }
}
