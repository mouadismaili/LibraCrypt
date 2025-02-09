using System;
using System.IO;
using LibraCrypt;
using System.Security.Cryptography;

namespace LibraCrypt
{
    class Program
    {
        private static BibliothequeData data = new BibliothequeData();
        private static string basePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "Bibliotheque"
    );

        static void Main()
        {
            Directory.CreateDirectory(basePath);
            Console.WriteLine("Bonjour Cher Utilisateur dans votre Application LibraCrypt\n ---------------------------------------------------------------");

            while (true)
            {
                Console.WriteLine("\n1. Ajouter un livre\n2. Lister les livres\n3. Sauvegarder\n4. Charger\n5. Quitter");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AjouterLivre();
                        break;
                    case "2":
                        ListerLivres();
                        break;
                    case "3":
                        Sauvegarder();
                        break;
                    case "4":
                        Charger();
                        break;
                    case "5":
                        return;
                }
            }
        }

        static void AjouterLivre()
        {
            var livre = new Livre();
            Console.Write("Titre: ");
            livre.Titre = Console.ReadLine();
            data.Livres.Add(livre);
        }

        static void ListerLivres()
        {
            foreach (var livre in data.Livres)
            {
                Console.WriteLine($"- {livre.Titre}");
            }
        }

        static void Sauvegarder()
        {
            try
            {
                Console.Write("Format (xml/json): ");
                var ext = Console.ReadLine().ToLower() == "xml" ? ".xml" : ".json";
                var fileName = $"Bibliotheque_{Environment.UserName}{ext}";
                var fullPath = Path.Combine(basePath, fileName);

                Console.Write("Clé de cryptage (vide pour SID): ");
                var key = Console.ReadLine();

                var serializer = SerializerFactory.GetSerializer(ext);
                serializer.Serialize(data, fullPath, key);

                Console.WriteLine("Sauvegarde réussie !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur: {ex.Message}");
            }
        }

        static void Charger()
        {
            try
            {
                Console.Write("Format (xml/json): ");
                var ext = Console.ReadLine().ToLower() == "xml" ? ".xml" : ".json";
                var fileName = $"Bibliotheque_{Environment.UserName}{ext}";
                var fullPath = Path.Combine(basePath, fileName);

                if (!File.Exists(fullPath)) throw new FileNotFoundException("Fichier introuvable");

                int attempts = 0;
                while (attempts < 3)
                {
                    Console.Write("Clé de cryptage: ");
                    var key = Console.ReadLine();

                    try
                    {
                        var serializer = SerializerFactory.GetSerializer(ext);
                        data = serializer.Deserialize(fullPath, key);
                        Console.WriteLine("Chargement réussi !");
                        return;
                    }
                    catch (CryptographicException)
                    {
                        Console.WriteLine("Clé incorrecte !");
                        attempts++;
                    }
                }

                File.Delete(fullPath);
                Console.WriteLine("3 échecs - Fichier supprimé !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur: {ex.Message}");
            }
        }

    }

}

