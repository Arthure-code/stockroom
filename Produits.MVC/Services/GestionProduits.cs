using Produits.MVC.Interfaces;
using Produits.MVC.Models;
using System.Text;

namespace Produits.MVC.Services
{
    public class GestionProduits : IProduit
    {
        private readonly string chemin;

        // Le fichier est nomme dans la configuration et cherche a la racine
        // du site, plutot qu'ecrit en dur et dependant du dossier courant.
        public GestionProduits(IWebHostEnvironment environnement, IConfiguration configuration)
        {
            string nomFichier = configuration["Catalogue:Fichier"] ?? "produits.csv";
            chemin = Path.Combine(environnement.ContentRootPath, nomFichier);
        }
        public async Task<List<Produit>> InitialisationAsync()
        {
            List<Produit> produits = new List<Produit>();

            if (!File.Exists(chemin))
            {
                File.Create(chemin).Close();
                return produits;
            }

            using (StreamReader lecteur = new StreamReader(chemin, Encoding.UTF8))
            {
                string? ligne;
                while ((ligne = await lecteur.ReadLineAsync()) != null)
                {
                    string[] valeurs = ligne.Split(';');

                    Produit produit = new Produit
                    {
                        Id = int.Parse(valeurs[0]),
                        Nom = valeurs[1],
                        Description = valeurs[2],
                        Prix = decimal.Parse(valeurs[3]),
                        Quantite = int.Parse(valeurs[4]),
                        Image = valeurs[5],
                        Vedette = bool.Parse(valeurs[6])
                    };

                    produits.Add(produit);
                }
            }
                return produits;
            }
        public async Task<List<Produit>> GetAllProduitsAsync()
        {
            return await InitialisationAsync();
        }


        // Écrase le fichier existant et enregistre une nouvelle liste de produits
        public async Task EnregistreToutAsync(List<Produit> produits)
        {
            using (StreamWriter item = new StreamWriter(chemin, false, Encoding.UTF8))
            {
                foreach (var produit in produits)
                {
                    await item.WriteLineAsync($"{produit.Id};{produit.Nom};{produit.Description};{produit.Prix};{produit.Quantite};{produit.Image};{produit.Vedette}");
                }
            }
        }

        public async Task<Produit?> GetProduiByIdAsync(int id)
        {
            var produits = await GetAllProduitsAsync();
            return produits.SingleOrDefault(x => x.Id == id);
        }

        public async Task EnregistreProduitAsync(Produit produit)
        {
            using (StreamWriter item = new StreamWriter(chemin, true, Encoding.UTF8))
            {
                await item.WriteLineAsync($"{produit.Id};{produit.Nom};{produit.Description};{produit.Prix};{produit.Quantite};{produit.Image};{produit.Vedette}");
            }
        }

    }
}
