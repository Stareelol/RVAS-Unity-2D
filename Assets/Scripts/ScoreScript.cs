using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;
using System.Linq;

public class ScoreScript : MonoBehaviour
{

    public TextMeshProUGUI text;
    private string connectionString;


    void Start()
    {
        connectionString = "Server=localhost;Database=game_db;User ID=root;Pooling=false;";
        DataTable data = GetScores();
        IterateThroughDataTable(data);
    }

    public DataTable GetScores()
    {
        DataTable dataTable = new DataTable();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT username, wins, losses FROM users";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
            }
        }
        return dataTable;
    }

    public void IterateThroughDataTable(DataTable dataTable)
    {
        text.text = "Username\tWins\tLosses\n";
        foreach (DataRow row in dataTable.Rows)
        {
            text.text += string.Format("{0}\t\t{1}\t{2}\n", row["username"], row["wins"], row["losses"]);
        }
    }
}
