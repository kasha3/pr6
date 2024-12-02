﻿using System;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace RegIN_Fadeev.Classes
{
    public class WorkingDB
    {
        readonly static string connection = "server=localhost;port=3307;database=regin;user=root;pwd=root;";
        public static MySqlConnection OpenConnection()
        {
            try
            {
                MySqlConnection mySqlConnection = new MySqlConnection(connection);
                mySqlConnection.Open();
                return mySqlConnection;
            } catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public static MySqlDataReader Query(string Sql, MySqlConnection mySqlConnection)
        {
            MySqlCommand mySqlCommand = new MySqlCommand(Sql, mySqlConnection);
            return mySqlCommand.ExecuteReader();
        }
        public static void CloseConnection(MySqlConnection mySqlConnection)
        {
            mySqlConnection?.Close();
            MySqlConnection.ClearPool(mySqlConnection);
        }
        public static bool OpenConnection(MySqlConnection mySqlConnection)
        {
            return mySqlConnection != null && mySqlConnection.State == System.Data.ConnectionState.Open;
        }
    }
}
