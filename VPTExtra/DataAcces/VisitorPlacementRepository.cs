using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Interfaces;
using Models;
using MySql.Data.MySqlClient;
using static Google.Protobuf.WellKnownTypes.Field.Types;

namespace DataAcces
{
    public class VisitorPlacementRepository : IVisitorPlacement
    {
        private readonly MySqlConnection db;
        public VisitorPlacementRepository(string connectionString)
        {
            db = new MySqlConnection(connectionString);
        }

        public void PlaceVisitor(int chairId)
        {
            db.Open();

            MySqlCommand cmd = new MySqlCommand($"UPDATE chair SET user_id = @VisitorId WHERE id = @ChairId;", db);
            cmd.Parameters.Add(new MySqlParameter("@ChairId", MySqlDbType.VarChar));
            cmd.Parameters.Add(new MySqlParameter("@VisitorId", MySqlDbType.VarChar));

            cmd.Parameters["@ChairId"].Value = chairId;
            cmd.Parameters["@VisitorId"].Value = 1;

            cmd.ExecuteNonQuery();

            db.Close();

        }
    }
}
