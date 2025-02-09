using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace LibraCrypt
{
    [Serializable]
    public class Livre
	{
		public string Titre { get; set; }
		public string Auteur { get; set; }
		public DateTime DateDePublication { get; set; }
		public int ISBM { get; set; }
		public Categorie Categorie { get; set; }
		public DateTime DateAJout { get; set; }
		public Livre(string titre, string auteur, DateTime dateDePublication, int id, Categorie categorie)
		{
			Titre = titre;
			Auteur = auteur;
			ISBM = id;
			DateDePublication = dateDePublication;
			Categorie = categorie;
			DateAJout = DateTime.Now;

		}
		public Livre() { }
	}
}
