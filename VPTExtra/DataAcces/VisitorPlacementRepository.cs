using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Interfaces.Repositories;
using Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg;
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

        public void PlaceVisitor(int chairId, int visitorId, int eventId)
        {
            try
            {
                db.Open();

                MySqlCommand cmd = new MySqlCommand($"UPDATE chair SET user_id = @VisitorId WHERE id = @ChairId;", db);
                cmd.Parameters.Add(new MySqlParameter("@ChairId", MySqlDbType.VarChar));
                cmd.Parameters.Add(new MySqlParameter("@VisitorId", MySqlDbType.VarChar));

                cmd.Parameters["@ChairId"].Value = chairId;
                cmd.Parameters["@VisitorId"].Value = visitorId;

                cmd.ExecuteNonQuery();

                PlaceVisitorInEvent(eventId, visitorId, chairId);
            }
            catch (Exception)
            {

            }
            finally
            {
                db.Close();
            }
        }
        private void PlaceVisitorInEvent(int eventId, int visitorId, int chairId)
        {
            try
            {
                db.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO user_event (event_id, user_id, chair_id) VALUES (@EventId, @UserId, @ChairId)", db);

                cmd.Parameters.AddWithValue("@EventId", eventId);
                cmd.Parameters.AddWithValue("@UserId", visitorId);
                cmd.Parameters.AddWithValue("@ChairId", chairId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }
            finally
            {
                db.Close();
            }
        }

        public void RevertVisitorPlacement(int chairId, int visitorId, int eventId)
        {
            try
            {
                db.Open();

                MySqlCommand cmd = new MySqlCommand($"UPDATE chair SET user_id = NULL WHERE id = @ChairId;", db);
                cmd.Parameters.Add(new MySqlParameter("@ChairId", MySqlDbType.VarChar));

                cmd.Parameters["@ChairId"].Value = chairId;

                cmd.ExecuteNonQuery();

                RevertVisitorPlacementEvent(chairId, visitorId, eventId);
            }
            catch (Exception)
            {
            }
            finally
            {
                db.Close();
            }
        }
        private void RevertVisitorPlacementEvent(int chairId, int visitorId, int eventId)
        {
            try
            {
                db.Open();

                MySqlCommand cmd = new MySqlCommand($"DELETE from user_event where user_id = @UserId and event_id = @EventId and chair_id = @ChairId", db);

                cmd.Parameters.AddWithValue("@UserId", visitorId);
                cmd.Parameters.AddWithValue("@EventId", eventId);
                cmd.Parameters.AddWithValue("@ChairId", chairId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                db.Close();
            }
        }        
    }
}
