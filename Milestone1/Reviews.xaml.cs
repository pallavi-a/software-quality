using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;
using NpgsqlTypes;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for Reviews.xaml
    /// </summary>
    public partial class Reviews : Window
    {
        public class Review
        {
            public string username { get; set; }
            public string date { get; set; }       
            public string rating { get; set; }
            public string text { get; set; }
            
        }
        string BID;
        string UID;
        public Reviews(string businessid,string userID)
        {
            BID = businessid;
            UID = userID;
            InitializeComponent();
            addReviewColumnsToGrid();
            string sqlstr = "select createdate, name, rating, text from business b join review r on b.businessid = r.businessid join person p on r.personid = p.personid where b.businessid = '" + businessid + "' order by createdate";
            executeQuery(sqlstr, addReviewsToGrid);
        }

        private void addReviewColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("date");
            col1.Header = "Date";
            col1.Width = 50;
            reviewGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("username");
            col2.Header = "User Name";
            col2.Width = 60;
            reviewGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("rating");
            col3.Header = "Stars";
            col3.Width = 50;
            reviewGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("text");
            col4.Header = "Text";
            col4.Width = 600;
            reviewGrid.Columns.Add(col4);

            // businessGrid.Items.Add(new Business() { name = "business1", state = "WA", city = "Pullman" });
            //businessGrid.Items.Add(new Business() { name = "business2", state = "ID", city = "Moscow" });
            //businessGrid.Items.Add(new Business() { name = "business3", state = "NV", city = "Las Vegas" });

        }
        private void addReviewsToGrid(NpgsqlDataReader R)
        {
            
            Review r1 = new Review() { date = R.GetString(0), username = R.GetString(1), rating = R.GetValue(2).ToString(), text = R.GetString(3) };
            reviewGrid.Items.Add(r1);

        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = yelpersDb; password=root";
        }

        private void executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {

            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            myf(reader);
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());

                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }

        private void executeQuery(string sqlstr)
        {

            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());

                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }

        private void addReviewButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (ratingDropDown.SelectedItem != null && reviewTextBox.Text != "" && reviewidTextBox.Text != "")
            {
                reviewGrid.Items.Clear();
                string stars = ratingDropDown.Text;
                int starsint = Int32.Parse(stars);
                float starsfloat = (float)starsint;
                string text = reviewTextBox.Text.ToString();
                string reviewID = reviewidTextBox.Text;
                string date = DateTime.UtcNow.ToString("d");
                string querystr = "Insert into Review (reviewid, createdate, text, rating, personid, businessid) values (@reviewid, '" + date + "',@text, " + starsfloat + ", '" + UID + "', '" + BID + "')";
                using (var connection = new NpgsqlConnection(buildConnectionString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {

                        cmd.Connection = connection;
                        cmd.CommandText = querystr;
                        cmd.Parameters.AddWithValue("reviewid", NpgsqlDbType.Varchar, reviewID);
                        cmd.Parameters.AddWithValue("text", NpgsqlDbType.Varchar, text);
                        try
                        {
                            var reader = cmd.ExecuteNonQuery();
                        }
                        catch (NpgsqlException ex)
                        {
                            Console.WriteLine(ex.Message.ToString());

                        }
                        finally
                        {
                            connection.Close();
                        }
                    }

                }

                string sqlstr = "select createdate, name, rating, text from business b join review r on b.businessid = r.businessid join person p on r.personid = p.personid where b.businessid = '" + BID + "' order by createdate";
                executeQuery(sqlstr, addReviewsToGrid);
            }
        }

    }
    }

