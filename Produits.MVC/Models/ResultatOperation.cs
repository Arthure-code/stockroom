namespace Produits.MVC.Models
{
    /// <summary>
    /// Ce qu'une operation du catalogue repond : elle a reussi, ou elle a
    /// echoue avec la raison a montrer. Le controleur n'a pas a deviner
    /// pourquoi une regle a refuse.
    /// </summary>
    public class ResultatOperation
    {
        private ResultatOperation(bool reussi, string? message)
        {
            Reussi = reussi;
            Message = message;
        }

        public bool Reussi { get; }

        /// <summary>La raison du refus, vide quand l'operation a reussi.</summary>
        public string? Message { get; }

        public static ResultatOperation Succes() => new(true, null);

        public static ResultatOperation Refus(string message) => new(false, message);

        /// <summary>Le produit demande n'existe pas.</summary>
        public static ResultatOperation Introuvable() => new(false, null);
    }
}
