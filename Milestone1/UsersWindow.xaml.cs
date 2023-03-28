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
using System.Data;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for BusinessDetails.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        string UID;
        public UsersWindow()
        {
            InitializeComponent();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = yelpersDb; password=root";
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

        string selectedUser = "";
        private void loadID(NpgsqlDataReader R)
        {

            idList.Items.Add(R.GetString(0));
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            string sqlstr = "SELECT personid FROM person WHERE name = '" + uname.Text + "';";
            idList.Items.Clear();
            name.Document.Blocks.Clear();
            stars.Document.Blocks.Clear();
            fans.Document.Blocks.Clear();
            yelpingSince.Document.Blocks.Clear();
            voteCount.Document.Blocks.Clear();
            executeQuery(sqlstr, loadID);
        }

        private void populateLocation(NpgsqlDataReader R)
        {
            latitudeTextBox.Text = R.GetValue(0).ToString();
            longitudeTextBox.Text = R.GetValue(1).ToString(); 
        }
        private void idList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedUser = idList.SelectedItem.ToString();
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine(ex.Message.ToString());
                selectedUser = "";
            }

            UID = selectedUser;

            string sqlstr1 = "SELECT name,staravg,numfans,joindate,numvotes FROM person WHERE personid = '" + idList.SelectedItem + "';";
            executeQuery(sqlstr1, loadUserInfo);

            string sqlstr2 = "SELECT name,staravg,joindate FROM person,befriend WHERE requestFrom = personid AND requestto = '" + idList.SelectedItem + "' ORDER BY name;";
            NpgsqlFillDataGrid(sqlstr2, friends, "Friends");

            string sqlstr3 = "SELECT businessname,stars,city,zipcode,address FROM business,favor WHERE favor.businessid = business.businessid AND personid = '" + idList.SelectedItem + "' ORDER BY businessname;";
            NpgsqlFillDataGrid(sqlstr3, favoriteBusinesses, "Favorite Businesses");

            string locationQuery = "select latitude, longitude from person where personid = '" + idList.SelectedItem + "'";
            executeQuery(locationQuery, populateLocation);

            string subquery =
                "SELECT name, businessname, city, text, createdate " +
                "FROM person, befriend, review, business " +
                "WHERE requestFrom = person.personid " +
                "AND person.personid = review.personid " +
                "AND review.businessid = business.businessid " +
                "AND requestto = '" + idList.SelectedItem + "' " +
                "ORDER BY name";
            string sqlstr4 =
                "SELECT DISTINCT ON (name) f.name, f.businessname, f.city, f.text " +
                "FROM ( " +
                "    SELECT  name, MAX(createdate) as maxcd " +
                "    FROM ( " + subquery + ") as g " +
                "    GROUP BY name " +
                ") as x INNER JOIN (" + subquery + ") as f " +
                "    on f.name = x.name AND f.createdate = x.maxcd;";  //make id's equal
            NpgsqlFillDataGrid(sqlstr4, friendReviews, "Friend Reviews");
        }

        private void loadUserInfo(NpgsqlDataReader R)
        {

            String oldname = R.GetString(0);
            name.Document.Blocks.Clear();
            String thisname = R.GetString(0);
            name.AppendText(thisname);
   
            stars.Document.Blocks.Clear();
            stars.AppendText(R.GetValue(1).ToString());

            fans.Document.Blocks.Clear();
            fans.AppendText(R.GetValue(2).ToString());

            yelpingSince.Document.Blocks.Clear();
            yelpingSince.AppendText(R.GetString(3));

            voteCount.Document.Blocks.Clear();
            voteCount.AppendText(R.GetValue(4).ToString());

            //lat
            //long
        }

        private void NpgsqlFillDataGrid(String sqlstr, DataGrid dg, String tableName)
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {

                    command.Connection = connection;
                    command.CommandText = sqlstr;
                    try
                    {
                        NpgsqlDataAdapter pgda = new NpgsqlDataAdapter(command);
                        DataTable dt = new DataTable(tableName);
                        pgda.Fill(dt);
                        dg.ItemsSource = dt.DefaultView;
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
            NpgsqlCommand cmd = new NpgsqlCommand();
        }

        private void removeFromFavorites(NpgsqlDataReader R)
        {
            return;
        }
        private void Remove_from_Favorites_Click(object sender, RoutedEventArgs e)
        {
            DataGrid gd = favoriteBusinesses;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string bname = row_selected["businessname"].ToString();
                string stars = row_selected["stars"].ToString();
                string city = row_selected["city"].ToString();
                string zc = row_selected["zipcode"].ToString();
                string addr = row_selected["address"].ToString();
                string pid = selectedUser;
                //Console.WriteLine(bname + " " + stars + " " + city + " " + zc + " " + addr + " " + pid);

                string sqlstr = "DELETE FROM favor " +
                    "USING business " +
                    "WHERE businessname = '" + bname + "' " +
                    "AND city = '" + city + "' " +
                    "AND zipcode = '" + zc + "' " +
                    "AND address = '" + addr + "' " +
                    "AND business.businessid = favor.businessid " +
                    "AND personid = '" + pid + "';";

                executeQuery(sqlstr, removeFromFavorites);

                string sqlstr2 = "SELECT businessname,stars,city,zipcode,address FROM business,favor WHERE favor.businessid = business.businessid AND personid = '" + idList.SelectedItem + "' ORDER BY businessname;";
                NpgsqlFillDataGrid(sqlstr2, favoriteBusinesses, "Favorite Businesses");
            }
            else
                Console.WriteLine("Row selection is null.");
        }


        private void loadBusButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow businesses = new MainWindow(UID);
            businesses.Show();
            Close();

        }

        private void updateLocationButton_Click(object sender, RoutedEventArgs e)
        {
            float newLatitude = float.Parse(latitudeTextBox.Text);
            float newLongitude = float.Parse(longitudeTextBox.Text);
            string query = "update person set latitude=" + newLatitude + ", longitude=" + newLongitude + " where personid = '" + idList.SelectedItem + "'";
            executeQuery(query);
        }
    }
}
