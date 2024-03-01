using GenerateurCodes.MVC.Models;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Reflection.Metadata;

namespace GenerateurCodes.MVC.Data
{
    public class DataContext : IDisposable
    {
        private readonly SQLiteConnection _connection;

        public DataContext(string connectionstring)
        {
            _connection = new SQLiteConnection(connectionstring);
            _connection.Open();
        }

        public Task InsererDemande(DemandeCodeAcces demandeCodeAcces)
        {
            var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "INSERT INTO DemandeCodeAcces(NomDemandeur, DateNaissanceDemandeur," +
                "NAS, RaisonDemande) VALUES('"+demandeCodeAcces.NomDemandeur+"','"+
                demandeCodeAcces.DateNaissanceDemandeur + "','" +
                demandeCodeAcces.NAS + "','"+demandeCodeAcces.RaisonDemande+"')";

            return cmd.ExecuteNonQueryAsync();
        }

        public Task InsererCodeAcces(CodeAcces codeAcces)
        {
            var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "INSERT INTO CodeAcces(NomDemandeur, DateExpiration," +
                "Code) VALUES('" + codeAcces.NomDemandeur + "','" +
                codeAcces.DateExpiration + "','" + codeAcces.Code + "')";

            return cmd.ExecuteNonQueryAsync();
        }

        public async Task<IList<DemandeCodeAcces>> ObtenirDemandeCodeAccess()
        {
            List<DemandeCodeAcces> demandeCodeAccess = new List<DemandeCodeAcces>();

            var cmd = new SQLiteCommand("SELECT * FROM DemandeCodeAcces", _connection);

            var rdr = await cmd.ExecuteReaderAsync();

            while (await rdr.ReadAsync())
            {
                var demandeCodeAcces = new DemandeCodeAcces()
                {
                    Id = Convert.ToInt32(rdr["Id"]),
                    NomDemandeur = rdr["NomDemandeur"].ToString(),
                    DateNaissanceDemandeur =  DateTime.Parse((string)rdr["DateNaissanceDemandeur"]),
                    NAS = Convert.ToInt32 (rdr["NAS"]),
                    RaisonDemande = rdr["RaisonDemande"].ToString(),
                        
                };
                demandeCodeAccess.Add(demandeCodeAcces);
              }

           return (demandeCodeAccess); 
        }


        public async Task<IList<CodeAcces>> ObtenirCodeAccess(string nomDemandeur)
        {
            List<CodeAcces> codeAccess = new List<CodeAcces>();

            var cmd = new SQLiteCommand(_connection);
            cmd.CommandText = "SELECT * FROM CodeAcces WHERE NomDemandeur='"+nomDemandeur+"'";

            var rdr = await cmd.ExecuteReaderAsync();

            while (await rdr.ReadAsync())
            {
                var codeAcces = new CodeAcces()
                {
                    Id = Convert.ToInt32(rdr["Id"]),
                    NomDemandeur = rdr["NomDemandeur"].ToString(),
                    DateExpiration = DateTime.Parse((string)rdr["DateExpiration"]),
                    Code = rdr["Code"].ToString(),

                };
                codeAccess.Add(codeAcces);
            }

            return (codeAccess);
        }

        public async Task ReInitialiserBaseDeDonnees()
        {
            var cmd = new SQLiteCommand(_connection);
            cmd.CommandText = "DELETE FROM DemandeCodeAcces";
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText = "DELETE FROM CodeAcces";
           await cmd.ExecuteNonQueryAsync();
        }


        public void Dispose() => _connection.Dispose();

    }
}
