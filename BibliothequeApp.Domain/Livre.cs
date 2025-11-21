namespace BibliothequeApp.Domain
{
    public class Livre
    {
        public int IdLivre { get; set; }
        public string Titre { get; set; } = "";
        public string Auteur { get; set; } = "";
        public int? Annee { get; set; }
        public string? ISBN { get; set; }
        public string? Categorie { get; set; }
        public int Quantite { get; set; }

    }
}
