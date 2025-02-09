using LibraCrypt;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LibraCrypt
{
    [Serializable]
    public class Utilisateur
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string email { get; set; }
        public DateTime DateInscription { get; set; }
        public List<Livre> LivreEmprintes { get; set; }
        public Utilisateur()
        {
            LivreEmprintes = new List<Livre>();
            DateInscription = DateTime.Now;
        }
        public Utilisateur(string nom, string prenom, string mail) : this()
        {
            Nom = nom;
            Prenom = prenom;
            email = mail;
        }
    }
}

