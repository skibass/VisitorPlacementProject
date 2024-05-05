using Interfaces.DataAcces.Repositories;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces
{
    public class EventEditRepository : IEventEditRepository
    {
        private readonly MySqlConnection db;

        public EventEditRepository(string connectionString)
        {
            db = new MySqlConnection(connectionString);
        }

        public Part AddPart(int eventId)
        {
            db.Open();

            string maxPartName = GetMaxPartName(eventId);
            char nextPartLetter = GetNextPartLetter(maxPartName);

            string nextPartName = nextPartLetter.ToString();
            InsertPart(eventId, nextPartName);

            db.Close();

            return new Part { Name = nextPartName };
        }

        public Row AddRow(int partId)
        {
            db.Open();

            string partLetter = GetPartLetter(partId);
            int nextRowNumber = GetNextRowNumber(partId);

            string nextRowName = $"{partLetter}{nextRowNumber}";
            InsertRow(partId, nextRowName);

            db.Close();

            return new Row { Name = nextRowName };
        }

        public Chair AddChair(int rowId, int eventId)
        {
            db.Open();

            int nextChairNumber = GetNextChairNumber(rowId);
            string rowName = GetRowName(rowId);

            string nextChairName = $"{rowName}-{nextChairNumber}";
            InsertChair(rowId, nextChairName, eventId);

            db.Close();

            return new Chair { Name = nextChairName };
        }

        private string GetMaxPartName(int eventId)
        {
            var cmd = new MySqlCommand("SELECT MAX(name) AS maxPartName FROM part WHERE event_id = @eventId;", db);
            cmd.Parameters.AddWithValue("@eventId", eventId);
            return cmd.ExecuteScalar() as string;
        }

        private char GetNextPartLetter(string maxPartName)
        {
            char nextPartLetter = 'A';
            if (!string.IsNullOrEmpty(maxPartName))
            {
                char lastLetter = maxPartName[maxPartName.Length - 1];
                nextPartLetter = (char)(lastLetter + 1);
            }
            return nextPartLetter;
        }

        private void InsertPart(int eventId, string partName)
        {
            var cmd = new MySqlCommand("INSERT INTO part (name, event_id) VALUES (@partName, @eventId);", db);
            cmd.Parameters.AddWithValue("@partName", partName);
            cmd.Parameters.AddWithValue("@eventId", eventId);
            cmd.ExecuteNonQuery();
        }

        private string GetPartLetter(int partId)
        {
            var cmd = new MySqlCommand("SELECT name FROM part WHERE id = @partId;", db);
            cmd.Parameters.AddWithValue("@partId", partId);
            return cmd.ExecuteScalar().ToString();
        }

        private int GetNextRowNumber(int partId)
        {
            var cmd = new MySqlCommand("SELECT MAX(name) AS maxRowNumber FROM row WHERE part_id = @partId;", db);
            cmd.Parameters.AddWithValue("@partId", partId);

            string maxRowString = cmd.ExecuteScalar().ToString();
            return maxRowString.Length >= 2 ? int.Parse(maxRowString.Substring(1)) + 1 : 1;
        }

        private void InsertRow(int partId, string rowName)
        {
            var cmd = new MySqlCommand("INSERT INTO row (name, part_id) VALUES (@rowName, @partId);", db);
            cmd.Parameters.AddWithValue("@rowName", rowName);
            cmd.Parameters.AddWithValue("@partId", partId);
            cmd.ExecuteNonQuery();
        }

        private int GetNextChairNumber(int rowId)
        {
            var cmd = new MySqlCommand("SELECT MAX(name) AS maxChairName FROM chair WHERE row_id = @rowId;", db);
            cmd.Parameters.AddWithValue("@rowId", rowId);

            object maxChairNumberObj = cmd.ExecuteScalar();
            return maxChairNumberObj != DBNull.Value ? int.Parse(maxChairNumberObj.ToString().Split('-')[1]) + 1 : 1;
        }

        private string GetRowName(int rowId)
        {
            var cmd = new MySqlCommand("SELECT name FROM row WHERE id = @rowId;", db);
            cmd.Parameters.AddWithValue("@rowId", rowId);
            return cmd.ExecuteScalar().ToString();
        }

        private void InsertChair(int rowId, string chairName, int eventId)
        {
            var cmd = new MySqlCommand("INSERT INTO chair (name, row_id) VALUES (@name, @rowId);", db);
            cmd.Parameters.AddWithValue("@name", chairName);
            cmd.Parameters.AddWithValue("@rowId", rowId);
            cmd.ExecuteNonQuery();

            var updateVisitorLimitCmd = new MySqlCommand("UPDATE event SET visitorLimit = visitorLimit + 1 WHERE id = @eventId;", db);
            updateVisitorLimitCmd.Parameters.AddWithValue("@eventId", eventId);
            updateVisitorLimitCmd.ExecuteNonQuery();
        }
    }
}
