using Produits.MVC.Interfaces;
using Produits.MVC.Models;
using System.Text;

namespace Produits.MVC.Repositories
{
    /// <summary>
    /// Le fichier CSV : une ligne par produit, sept champs separes par des
    /// points-virgules, sans entete. Les lectures et les ecritures sont
    /// asynchrones, avec StreamReader et StreamWriter.
    /// </summary>
    public class ProduitRepository : IProduitRepository
    {
        private readonly string chemin;

        // Le fichier est nomme dans la configuration et cherche a la racine
        // du site, plutot qu'ecrit en dur et dependant du dossier courant.
        public ProduitRepository(IWebHostEnvironment environnement, IConfiguration configuration)
        {
            string nomFichier = configuration["Catalogue:Fichier"] ?? "produits.csv";
            chemin = Path.Combine(environnement.ContentRootPath, nomFichier);
        }

        public async Task<List<Produit>> LireTousAsync()
        {
            List<Produit> produits = new List<Produit>();

            if (!File.Exists(chemin))
            {
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

        public async Task EcrireTousAsync(IEnumerable<Produit> produits)
        {
            using (StreamWriter redacteur = new StreamWriter(chemin, false, Encoding.UTF8))
            {
                foreach (var produit in produits)
                {
                    await redacteur.WriteLineAsync(
                        $"{produit.Id};{produit.Nom};{produit.Description};{produit.Prix};{produit.Quantite};{produit.Image};{produit.Vedette}");
                }
            }
        }
    }
}
