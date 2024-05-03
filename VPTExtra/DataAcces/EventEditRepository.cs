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

            MySqlCommand getMaxPartNameCmd = new MySqlCommand("SELECT MAX(name) AS maxPartName FROM part WHERE event_id = @eventId;", db);
            getMaxPartNameCmd.Parameters.AddWithValue("@eventId", eventId);

            string maxPartName = getMaxPartNameCmd.ExecuteScalar() as string;
            char nextPartLetter = 'A';

            if (!string.IsNullOrEmpty(maxPartName))
            {
                char lastLetter = maxPartName[maxPartName.Length - 1];
                nextPartLetter = (char)(lastLetter + 1);
            }

            string nextPartName = nextPartLetter.ToString();

            MySqlCommand insertPartCmd = new MySqlCommand("INSERT INTO part (name, event_id) VALUES (@partName, @eventId);", db);
            insertPartCmd.Parameters.AddWithValue("@partName", nextPartName);
            insertPartCmd.Parameters.AddWithValue("@eventId", eventId);

            insertPartCmd.ExecuteNonQuery();

            db.Close();

            Part newPart = new Part
            {
                Name = nextPartName
            };

            return newPart;
        }

        public Row AddRow(int partId)
        {
            db.Open();

            MySqlCommand getMaxRowNumberCmd = new MySqlCommand("SELECT MAX(name) AS maxRowNumber FROM row WHERE part_id = @partId;", db);
            getMaxRowNumberCmd.Parameters.AddWithValue("@partId", partId);

            object maxRowNumberObj = getMaxRowNumberCmd.ExecuteScalar();

            string maxRowString = maxRowNumberObj.ToString();

            int nextRowNumber;
            if (maxRowString.Length >= 2)
            {
                string numberString = maxRowString.Substring(1);
                nextRowNumber = int.Parse(numberString) + 1;
            }
            else
            {
                nextRowNumber = 1;
            }

            MySqlCommand getPartLetterCmd = new MySqlCommand("SELECT name FROM part WHERE id = @partId;", db);
            getPartLetterCmd.Parameters.AddWithValue("@partId", partId);

            string partLetter = getPartLetterCmd.ExecuteScalar() as string;

            string nextRowName = $"{partLetter}{nextRowNumber}";

            MySqlCommand insertRowCmd = new MySqlCommand("INSERT INTO row (name, part_id) VALUES (@rowName, @partId);", db);
            insertRowCmd.Parameters.AddWithValue("@rowName", nextRowName);
            insertRowCmd.Parameters.AddWithValue("@partId", partId);

            insertRowCmd.ExecuteNonQuery();

            db.Close();

            Row newRow = new Row
            {
                Name = nextRowName,
            };

            return newRow;
        }

        public Chair AddChair(int rowId, int eventId)
        {
            db.Open();

            MySqlCommand getMaxChairNumberCmd = new MySqlCommand("SELECT MAX(name) AS maxChairName FROM chair WHERE row_id = @rowId;", db);
            getMaxChairNumberCmd.Parameters.AddWithValue("@rowId", rowId);

            object maxChairNumberObj = getMaxChairNumberCmd.ExecuteScalar();
            int nextChairNumber = 1;

            if (maxChairNumberObj != DBNull.Value)
            {
                string maxChairName = maxChairNumberObj.ToString();
                string[] chairNameSplit = maxChairName.Split('-');
                nextChairNumber = int.Parse(chairNameSplit[1]) + 1;
            }

            MySqlCommand getRowNameCmd = new MySqlCommand("SELECT name FROM row WHERE id = @rowId;", db);
            getRowNameCmd.Parameters.AddWithValue("@rowId", rowId);

            string rowName = getRowNameCmd.ExecuteScalar().ToString();

            string nextChairName = $"{rowName}-{nextChairNumber}";

            MySqlCommand insertChairCmd = new MySqlCommand("INSERT INTO chair (name, row_id) VALUES (@name, @rowId);", db);
            insertChairCmd.Parameters.AddWithValue("@name", nextChairName);
            insertChairCmd.Parameters.AddWithValue("@rowId", rowId);

            insertChairCmd.ExecuteNonQuery();

            MySqlCommand updateVisitorLimitCmd = new MySqlCommand("UPDATE event SET visitorLimit = visitorLimit + 1 WHERE id = @eventId;", db);
            updateVisitorLimitCmd.Parameters.AddWithValue("@rowId", rowId);
            updateVisitorLimitCmd.Parameters.AddWithValue("@eventId", eventId);
            updateVisitorLimitCmd.ExecuteNonQuery();

            db.Close();

            Chair newChair = new Chair
            {
                Name = nextChairName,
            };

            return newChair;
        }
    }
}
