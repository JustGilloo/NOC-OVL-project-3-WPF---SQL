using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Groepsproject_Blokken
{
    internal static class DataManager
    {
        //TODO: Speler met de beste winrate en aantal games tonen(minstens 5 games)Je kan sorten op winrate en aantal games
        //TODO: Extra functie query toevoegen: zoeken op username en wachtwoord





        //JSON VERSIES - HIER NIET VOORBIJ GAAN MET COMMENTEN







        //Haalt de ingelogde gebruiker op
        public static Player GetLoggedInPlayer(string naam, string wachtwoord)
        {
            List<Player> list = GetAllPlayers();
            Player returnGebruiker = new Player();
            foreach (Player gevondenGebruiker in list)
            {
                if (gevondenGebruiker.Name == naam && gevondenGebruiker.Password == wachtwoord)
                {
                    returnGebruiker = gevondenGebruiker;
                }
            }
            return returnGebruiker;
        }
        public static Admin GetLoggedInAdmin(string naam, string wachtwoord)
        {


            List<Admin> list = GetAllAdmins();
            Admin returnGebruiker = new Admin();
            foreach (Admin gevondenGebruiker in list)
            {
                if (gevondenGebruiker.Name == naam && gevondenGebruiker.Password == wachtwoord)
                {
                    returnGebruiker = gevondenGebruiker;


                }
            }
            return returnGebruiker;

        }
        public static Manager GetLoggedInManager(string naam, string wachtwoord)
        {
            List<Manager> list = GetAllManagers();
            Manager returnGebruiker = new Manager();
            foreach (Manager gevondenGebruiker in list)
            {
                if (gevondenGebruiker.Name == naam && gevondenGebruiker.Password == wachtwoord)
                {
                    returnGebruiker = gevondenGebruiker;

                }
            }
            return returnGebruiker;
        }

        //1 record ophalen: Speler, Admin of Manager
        public static Player GetOnePlayer(int pkPlayer)
        {
            List<Player> list = GetAllPlayers();
            Player gevondenPlayer = new Player();
            foreach (Player player in list)
            {
                if (player.SpelerID == pkPlayer)
                {
                    gevondenPlayer = player;
                }
            }
            return gevondenPlayer;
        }
        public static Admin GetOneAdmin(int pkAdmin)
        {

            List<Admin> list = GetAllAdmins();
            Admin returnGebruiker = new Admin();
            foreach (Admin gevondenGebruiker in list)
            {
                if (gevondenGebruiker.AdminID == pkAdmin)
                {
                    //TODO: Niet blij mee, wou de returnwaarde niet op 52, maar mag niet alleen hier
                    returnGebruiker = gevondenGebruiker;
                }
            }
            return returnGebruiker;
        }
        public static Manager GetOneManager(int pkManager)
        {

            List<Manager> list = GetAllManagers();
            Manager returnGebruiker = new Manager();
            foreach (Manager gevondenGebruiker in list)
            {
                if (gevondenGebruiker.ManagerID == pkManager)
                {
                    returnGebruiker = gevondenGebruiker;
                }
            }
            return returnGebruiker;
        }
        //alle records ophalen: Gamelogs, Players, Admins, Managers
        public static List<GameLogVS> GetAllGameLogVS()
        {

            using (StreamReader r = new StreamReader("../../GameLogVS/GamelogsVS"))
            {
                List<GameLogVS> lijstVS = new List<GameLogVS>();
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.IncludeFields = false;
                string json = r.ReadToEnd(); // Tekst inlezen in een string
                lijstVS = JsonSerializer.Deserialize<List<GameLogVS>>(json);
                return lijstVS;
            }
        }
        public static List<GameLogSP> GetAllGameLogSP()
        {
            using (StreamReader r = new StreamReader("../../GameLogSP/GamelogsSP"))
            {
                List<GameLogSP> lijstSP = new List<GameLogSP>();
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.IncludeFields = false;
                string json = r.ReadToEnd();
                lijstSP = JsonSerializer.Deserialize<List<GameLogSP>>(json);
                return lijstSP;
            }
        }
        public static List<Player> GetAllPlayersSorted() //haalt spelers op en displayt voorlopig gewonnen games, later aanpassen naar winrate%? of verder filteren?
        {
            using (StreamReader r = new StreamReader("../../Players/CurrentPlayers"))
            {
                List<Player> lijstOrderPlayers = new List<Player>();
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.IncludeFields = false;
                string json = r.ReadToEnd();
                lijstOrderPlayers = JsonSerializer.Deserialize<List<Player>>(json);
                lijstOrderPlayers.OrderBy(x => x.VSGamesPlayed).ToList();
                return lijstOrderPlayers;
            }
        }
        public static List<GameLogSP> GetAllGameLogSPSorted()
        {
            using (StreamReader r = new StreamReader("../../GameLogSP/GamelogsSP"))
            {
                List<GameLogSP> lijstGameSP = new List<GameLogSP>();
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.IncludeFields = false;
                string json = r.ReadToEnd();
                lijstGameSP = JsonSerializer.Deserialize<List<GameLogSP>>(json);
                lijstGameSP.OrderBy(x => x.Score).ToList();
                return lijstGameSP;
            }

        }
        public static List<Player> GetAllPlayers()
        {
            using (StreamReader r = new StreamReader("../../Players/CurrentPlayers"))
            {
                List<Player> lijstPlayers = new List<Player>();
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.IncludeFields = false;
                string json = r.ReadToEnd();
                lijstPlayers = JsonSerializer.Deserialize<List<Player>>(json);
                return lijstPlayers;
            }

        }
        public static List<Admin> GetAllAdmins()
        {
            using (StreamReader r = new StreamReader("../../Admins/CurrentAdmins"))
            {
                List<Admin> lijstAdmins = new List<Admin>();
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.IncludeFields = false;
                string json = r.ReadToEnd();
                lijstAdmins = JsonSerializer.Deserialize<List<Admin>>(json);
                return lijstAdmins;
            }
        }
        public static List<Manager> GetAllManagers()
        {
            using (StreamReader r = new StreamReader("../../Managers/CurrentManagers"))
            {
                List<Manager> lijstManagers = new List<Manager>();
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.IncludeFields = false;
                string json = r.ReadToEnd();
                lijstManagers = JsonSerializer.Deserialize<List<Manager>>(json);
                return lijstManagers;
            }
        }
        //1 record toevoegen: Gamelogs, Players, Manager
        public static bool InsertGameLogVS(GameLogVS aGameLogVS)
        {
            List<GameLogVS> listGameLogVS = new List<GameLogVS>();
            bool insertSucceeded = false;

            {
                listGameLogVS = GetAllGameLogVS();
            }
            listGameLogVS.Add(aGameLogVS);
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IncludeFields = false;
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(listGameLogVS, options);
            File.WriteAllText("../../GameLogVS/GamelogsVS", json);
            insertSucceeded = true;
            return insertSucceeded;
        }
        public static bool InsertGameLogSP(GameLogSP aGameLogSP)
        {
            List<GameLogSP> listGameLogSP = new List<GameLogSP>();
            bool insertSucceeded = false;
            if (System.IO.File.Exists("../../GameLogSP/GamelogsSP"))
            {
                listGameLogSP = GetAllGameLogSP();
            }

            listGameLogSP.Add(aGameLogSP);
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IncludeFields = false;
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(listGameLogSP, options);
            File.WriteAllText("../../GameLogSP/GamelogsSP", json);
            insertSucceeded = true;
            return insertSucceeded;

        }
        public static bool InsertPlayer(Player aPlayer)
        {
            List<Player> players = new List<Player>();
            bool insertSucceeded = false;
            if (System.IO.File.Exists("../../Players/CurrentPlayers"))
            {
                players = GetAllPlayers();
            }
            players.Add(aPlayer);
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IncludeFields = false;
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(players, options);
            File.WriteAllText("../../Players/CurrentPlayers", json);
            insertSucceeded = true;
            return insertSucceeded;
        }
        public static bool InsertManager(Manager aManager)
        {
            List<Manager> managers = new List<Manager>();

            bool insertSucceeded = false;
            if (System.IO.File.Exists("../../Managers/CurrentManagers"))
            {
                managers = GetAllManagers();
            }

            managers.Add(aManager);
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IncludeFields = false;
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(managers, options);
            File.WriteAllText("../../Managers/CurrentManagers", json);
            insertSucceeded = true;
            return insertSucceeded;
        }
        //1 record wijzigen: Players, Managers
        public static bool UpdatePlayer(Player aPlayer) //TOdo: kweet ff niet hoe het te schrijven
        {

            bool updateSucceeded = false;
            List<Player> players = GetAllPlayers();
            foreach (Player player in players)
            {
                if (player.Name == aPlayer.Name)
                {
                    player.Password = aPlayer.Password;
                    player.ProfilePicture = aPlayer.ProfilePicture; // TODO: Uitdaging, pfp is een string
                    player.VSGamesPlayed = aPlayer.VSGamesPlayed;
                    player.VSGamesWon = aPlayer.VSGamesWon;
                    player.VSHighscore = aPlayer.VSHighscore;
                    player.SPGamesPlayed = aPlayer.SPGamesPlayed;
                    player.SPGamesWon = aPlayer.SPGamesWon;
                    player.SPHighscore = aPlayer.SPHighscore;
                    updateSucceeded = true;
                }
            }
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IncludeFields = false;
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(players, options);
            File.WriteAllText("../../Players/CurrentPlayers", json);
            return updateSucceeded;

        }
        public static bool UpdateManager(Manager aManager)
        {

            bool updateSucceeded = false;
            List<Manager> managers = GetAllManagers();
            foreach (Manager manager in managers)
            {
                if (manager.Name == aManager.Name)
                {
                    manager.Password = aManager.Password;
                    updateSucceeded = true;
                }
            }
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IncludeFields = false;
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(managers, options);
            File.WriteAllText("../../Managers/CurrentManagers", json);
            return updateSucceeded;


        }
        //1 record verwijderen: Gamelogs, Players, Manager
        public static bool DeleteGameLogVS(GameLogVS aGameLogVS)
        {
            bool deleteSucceeded = false;
            List<GameLogVS> listGameLogVS = GetAllGameLogVS();
            foreach (GameLogVS gameLogVS in listGameLogVS)
            {
                if (gameLogVS.GameID == aGameLogVS.GameID)
                {
                    listGameLogVS.Remove(gameLogVS);
                    deleteSucceeded = true;
                    break;
                }
            }
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IncludeFields = false;
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(listGameLogVS, options);
            File.WriteAllText("../../GameLogVS/GamesLogVS", json);
            return deleteSucceeded;
        }
        public static bool DeleteGameLogSP(GameLogSP aGameLogSP)
        {

            bool deleteSucceeded = false;
            List<GameLogSP> listGameLogSP = GetAllGameLogSP();
            foreach (GameLogSP gameLogSP in listGameLogSP)
            {
                if (gameLogSP.GameNumber == aGameLogSP.GameNumber)
                {
                    listGameLogSP.Remove(gameLogSP);
                    deleteSucceeded = true;
                    break;
                }
            }
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IncludeFields = false;
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(listGameLogSP, options);
            File.WriteAllText("../../GameLogSP/GamesLogSP", json);
            return deleteSucceeded;
        }
        public static bool DeletePlayer(Player aPlayer)
        {
            bool deleteSucceeded = false;
            List<Player> players = GetAllPlayers();
            foreach (Player player in players)
            {
                if (player.Name == aPlayer.Name)
                {
                    players.Remove(player);
                    deleteSucceeded = true;
                    break;

                }
            }
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IncludeFields = false;
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(players, options);
            File.WriteAllText("../../Players/CurrentPlayers", json);
            return deleteSucceeded;
        }
        public static bool DeleteManager(Manager aManager)
        {
            bool deleteSucceeded = false;
            List<Manager> managers = GetAllManagers();
            foreach (Manager manager in managers)
            {
                if (manager.Name == aManager.Name)
                {
                    managers.Remove(manager);
                    deleteSucceeded = true;
                    break;
                }
            }
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IncludeFields = false;
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(managers, options);
            File.WriteAllText("../../Managers/CurrentManagers", json);
            return deleteSucceeded;
        }











        //DATABASE VERSIES - HIER NIET VOORBIJ GAAN  MET COMMENTEN














        //Haalt de ingelogde gebruiker op
        //public static Player GetLoggedInPlayer(string naam, string wachtwoord)
        //{

        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        var query = from Player in BenKrabbeDBEntities.Players
        //                    where Player.Name == naam && Player.Password == wachtwoord
        //                    select Player;
        //        return query.FirstOrDefault();
        //    }

        //}
        //public static Admin GetLoggedInAdmin(string naam, string wachtwoord)
        //{
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        var query = from Admin in BenKrabbeDBEntities.Admins
        //                    where Admin.Name == naam && Admin.Password == wachtwoord
        //                    select Admin;
        //        return query.FirstOrDefault();
        //    }
        //}
        //public static Manager GetLoggedInManager(string naam, string wachtwoord)
        //{
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        var query = from Manager in BenKrabbeDBEntities.Managers
        //                    where Manager.Name == naam && Manager.Password == wachtwoord
        //                    select Manager;
        //        return query.FirstOrDefault();
        //    }
        //}

        ////1 record ophalen: Speler, Admin of Manager
        //public static Player GetOnePlayer(int pkPlayer)
        //{

        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        var query = from Player in BenKrabbeDBEntities.Players
        //                    where Player.SpelerID == pkPlayer
        //                    select Player;
        //        return query.FirstOrDefault();
        //    }

        //}
        //public static Admin GetOneAdmin(int pkAdmin)
        //{

        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        var query = from Admin in BenKrabbeDBEntities.Admins
        //                    where Admin.AdminID == pkAdmin
        //                    select Admin;
        //        return query.FirstOrDefault();
        //    }

        //}
        //public static Manager GetOneManager(int pkManager)
        //{
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        var query = from Manager in BenKrabbeDBEntities.Managers
        //                    where Manager.ManagerID == pkManager
        //                    select Manager;
        //        return query.FirstOrDefault();
        //    }
        //}
        ////alle records ophalen: Gamelogs, Players, Admins, Managers
        //public static List<GameLogVS> GetAllGameLogVS()
        //{

        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        return BenKrabbeDBEntities.GamesLogVS.ToList();
        //    }

        //}
        //public static List<GameLogSP> GetAllGameLogSP()
        //{
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        return BenKrabbeDBEntities.GamesLogSP.ToList();
        //    }

        //}
        //public static List<Player> GetAllPlayersSorted() //haalt spelers op en displayt voorlopig gewonnen games, later aanpassen naar winrate%? of verder filteren?
        //{

        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        return BenKrabbeDBEntities.Players.OrderBy(x => x.VSGamesWon).ToList();
        //    }


        //}
        //public static List<GameLogSP> GetAllGameLogSPSorted()
        //{

        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        return BenKrabbeDBEntities.GamesLogSP.OrderBy(x => x.Score).ToList();
        //    }


        //}
        //public static List<Player> GetAllPlayers()
        //{

        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        return BenKrabbeDBEntities.Players.ToList();
        //    }


        //}
        //public static List<Admin> GetAllAdmins()
        //{
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        return BenKrabbeDBEntities.Admins.ToList();
        //    }

        //}
        //public static List<Manager> GetAllManagers()
        //{
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        return BenKrabbeDBEntities.Managers.ToList();
        //    }
        //}
        ////1 record toevoegen: Gamelogs, Players, Manager
        //public static bool InsertGameLogVS(GameLogVS aGameLogVS)
        //{

        //    bool insertSucceeded = false;
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        BenKrabbeDBEntities.GamesLogVS.Add(aGameLogVS);
        //        if (0 < BenKrabbeDBEntities.SaveChanges())
        //        {
        //            insertSucceeded = true;
        //        }
        //    }
        //    return insertSucceeded;

        //}
        //public static bool InsertGameLogSP(GameLogSP aGameLogSP)
        //{

        //    bool insertSucceeded = false;
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        BenKrabbeDBEntities.GamesLogSP.Add(aGameLogSP);
        //        if (0 < BenKrabbeDBEntities.SaveChanges())
        //        {
        //            insertSucceeded = true;
        //        }
        //    }
        //    return insertSucceeded;

        //}
        //public static bool InsertPlayer(Player aPlayer)
        //{

        //    bool insertSucceeded = false;
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        BenKrabbeDBEntities.Players.Add(aPlayer);
        //        if (0 < BenKrabbeDBEntities.SaveChanges())
        //        {
        //            insertSucceeded = true;
        //        }
        //    }
        //    return insertSucceeded;


        //}
        //public static bool InsertManager(Manager aManager)
        //{
        //    bool insertSucceeded = false;
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        BenKrabbeDBEntities.Managers.Add(aManager);
        //        if (0 < BenKrabbeDBEntities.SaveChanges())
        //        {
        //            insertSucceeded = true;
        //        }
        //    }
        //    return insertSucceeded;
        //}
        ////1 record wijzigen: Players, Managers
        //public static bool UpdatePlayer(Player aPlayer) //TOdo: kweet ff niet hoe het te schrijven
        //{            //Database Versie

        //    bool updateSucceeded = false;
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        BenKrabbeDBEntities.Entry(aPlayer).State = System.Data.Entity.EntityState.Modified;
        //        if (0 < BenKrabbeDBEntities.SaveChanges())
        //        {
        //            updateSucceeded = true;
        //        }
        //    }
        //    return updateSucceeded;
        //}
        //public static bool UpdateManager(Manager aManager)
        //{

        //    bool updateSucceeded = false;
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        BenKrabbeDBEntities.Entry(aManager).State = System.Data.Entity.EntityState.Modified;
        //        if (0 < BenKrabbeDBEntities.SaveChanges())
        //        {
        //            updateSucceeded = true;
        //        }
        //    }
        //    return updateSucceeded;
        //}
        ////1 record verwijderen: Gamelogs, Players, Manager
        //public static bool DeleteGameLogVS(GameLogVS aGameLogVS)
        //{            //Database Versie

        //    bool deleteSucceeded = false;
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        BenKrabbeDBEntities.Entry(aGameLogVS).State = System.Data.Entity.EntityState.Deleted;
        //        if (0 < BenKrabbeDBEntities.SaveChanges())
        //        {
        //            deleteSucceeded = true;
        //        }
        //    }
        //    return deleteSucceeded;

        //}
        //public static bool DeleteGameLogSP(GameLogSP aGameLogSP)
        //{

        //    bool deleteSucceeded = false;
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        BenKrabbeDBEntities.Entry(aGameLogSP).State = System.Data.Entity.EntityState.Deleted;
        //        if (0 < BenKrabbeDBEntities.SaveChanges())
        //        {
        //            deleteSucceeded = true;
        //        }
        //    }
        //    return deleteSucceeded;
        //}
        //public static bool DeletePlayer(Player aPlayer)
        //{
        //    bool deleteSucceeded = false;
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        BenKrabbeDBEntities.Entry(aPlayer).State = System.Data.Entity.EntityState.Deleted;
        //        if (0 < BenKrabbeDBEntities.SaveChanges())
        //        {
        //            deleteSucceeded = true;
        //        }
        //    }
        //    return deleteSucceeded;


        //}
        //public static bool DeleteManager(Manager aManager)
        //{

        //    bool deleteSucceeded = false;
        //    using (var BenKrabbeDBEntities = new BenKrabbeDBEntities())
        //    {
        //        BenKrabbeDBEntities.Entry(aManager).State = System.Data.Entity.EntityState.Deleted;
        //        if (0 < BenKrabbeDBEntities.SaveChanges())
        //        {
        //            deleteSucceeded = true;
        //        }
        //    }
        //    return deleteSucceeded;


        //}


















        //HIER NIET VOORBIJ GAAN MET COMMENTEN :)





    }
}
