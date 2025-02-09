using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace LibraCrypt
{
    [Serializable]
    public class BibliothequeData
    {
        public List<Livre> Livres { get; set; } = new List<Livre>();
        public List<Utilisateur> Utilisateurs { get; set; } = new List<Utilisateur>();
        public List<Categorie> Categories { get; set; } = new List<Categorie>();
    }
}
