using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace LibraCrypt
{
    [Serializable]
    public class Categorie
	{
		public string Nom {  get; set; }
		public List<Livre> LivreAssocies { get; set; }
		public Categorie(string nom)
		{
			Nom = nom;
			LivreAssocies=new List<Livre> ();
		}
		public Categorie() { }
		public void AjouterLivre(Livre livre)
		{
			LivreAssocies.Add(livre);
		}
		public void AfficherLivreDeLaCategorie()
		{
			Console.WriteLine($"Les livres Dans la catégorie '{Nom}':");
			foreach(var livre in LivreAssocies)
			{
				Console.WriteLine($"-{livre}");
			}
		}
	}
}
