using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Produits.MVC.Interfaces;
using Produits.MVC.Models;

namespace Produits.MVC.Controllers
{
    public class ProduitsController : Controller
    {
        private readonly IProduit _gestionProduits;

        public ProduitsController(IProduit gestionProduits)
        {
            _gestionProduits = gestionProduits;
        }

        public async Task<ActionResult> Accueil()
        {
            var liste = await _gestionProduits.GetAllProduitsAsync();

            var vm = new AccueilViewModel();
            vm.Vedettes = liste.Where(p => p.Vedette == true && p.Quantite != 0).ToList();

                vm.ProduitsFiltres = liste.FindAll(p => !p.Vedette && p.Quantite != 0).ToList();

            return View(vm);
        }

        // GET: ProduitsController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Produit produit)
        {
            if (ModelState.IsValid)
            {
                var liste = await _gestionProduits.GetAllProduitsAsync();

                if (produit.Vedette == true)
                {
                    foreach (var p in liste)
                    {
                        p.Vedette = false;
                    }
                }

                produit.Id = liste.Any() ? liste.Max(x => x.Id) + 1 : 1;
                liste.Add(produit);
                await _gestionProduits.EnregistreToutAsync(liste);

                return RedirectToAction(nameof(Accueil));
            }

            return View(produit);
        }

        // GET: ProduitsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var liste = await _gestionProduits.GetAllProduitsAsync();
            Produit? produitfiltre = liste.SingleOrDefault(p => p.Id == id);
            return View(produitfiltre);
        }

        // POST: ProduitsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Produit produit)
        {
            var liste = await _gestionProduits.GetAllProduitsAsync();
            Produit? produitfiltre = liste.SingleOrDefault(p => p.Id == produit.Id);

            if (produitfiltre != null && produitfiltre.Vedette == true)
            {
                ModelState.AddModelError(string.Empty, "Un produit vedette ne peut pas être modifier.");
                return View(produitfiltre);
            }

            if (ModelState.IsValid)
            {
                if (produit.Vedette == true)
                {
                    foreach (var p in liste)
                    {
                        p.Vedette = false;
                    }
                }

                produitfiltre.Nom = produit.Nom;
                produitfiltre.Description = produit.Description;
                produitfiltre.Quantite = produit.Quantite;
                produitfiltre.Vedette = produit.Vedette;
                produitfiltre.Prix = produit.Prix;
                produitfiltre.Image = produit.Image;
                await _gestionProduits.EnregistreToutAsync(liste);

                return RedirectToAction(nameof(Accueil));
            }

            return NotFound();
        }

        // GET: ProduitsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var produit = await _gestionProduits.GetProduiByIdAsync(id);

            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: ProduitsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Produit produit)
        {
            var liste = await _gestionProduits.GetAllProduitsAsync();
            var produitfiltr = liste.SingleOrDefault(p => p.Id == id);

            if (produitfiltr != null && produitfiltr.Vedette == true)
            {
                ModelState.AddModelError(string.Empty, "Un produit vedette ne peut pas être supprimé.");
                return View(produitfiltr);
            }
            else if (produitfiltr != null)
            {
                liste.Remove(produitfiltr);
                await _gestionProduits.EnregistreToutAsync(liste);
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Accueil));
        }

        public async Task<ActionResult> Filtre(string libelle)
        {
            var liste = await _gestionProduits.GetAllProduitsAsync();

            var vm = new AccueilViewModel();

            vm.Vedettes = liste
                .Where(p => p.Vedette)
                .ToList();

            vm.ProduitsFiltres = liste
                .Where(p => !p.Vedette && p.Quantite != 0)
                .Where(p => string.IsNullOrWhiteSpace(libelle) || p.Nom.Contains(libelle, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return View(nameof(Accueil), vm);
        }


    }
}
