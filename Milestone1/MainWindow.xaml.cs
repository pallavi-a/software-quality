using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using System.Data;
using System.Windows.Baml2006;
using System.ComponentModel;
using System.Windows.Automation.Peers;

namespace Milestone1
{
    ///Change
    /// <summary> 
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string UID;
        public class Business
        {
            public string bid { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string distance { get; set; }
            public string busRating { get; set; }
            public string numReviews { get; set; }
            public string avgRating { get; set; }
            public string numCheckins { get; set; }
        }

        UsersWindow userInfo;
        public MainWindow(string userID)
        {
            userInfo = new UsersWindow();
            UID = userID;
            InitializeComponent();
            addState();
            addBusColumnsToGrid();
        }
        private void addBusColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("name");
            col1.Header = "BusinessName";
            col1.Width = 100;
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("state");
            col2.Header = "State";
            col2.Width = 60;
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 60;
            businessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("address");
            col4.Header = "address";
            col4.Width = 150;
            businessGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("distance");
            col5.Header = "distance";
            col5.Width = 75;
            businessGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("busRating");
            col6.Header = "Business Rating";
            col6.Width = 110;
            businessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Binding = new Binding("numReviews");
            col7.Header = "# Reviews";
            col7.Width = 100;
            businessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Binding = new Binding("avgRating");
            col8.Header = "Avg Rating";
            col8.Width = 100;
            businessGrid.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Binding = new Binding("numCheckins");
            col9.Header = "# check ins";
            col9.Width = 100;
            businessGrid.Columns.Add(col9);

            DataGridTextColumn col10 = new DataGridTextColumn();
            col10.Binding = new Binding("bid");
            col10.Header = "bid";
            col10.Width = 0;
            businessGrid.Columns.Add(col10);

            // businessGrid.Items.Add(new Business() { name = "business1", state = "WA", city = "Pullman" });
            //businessGrid.Items.Add(new Business() { name = "business2", state = "ID", city = "Moscow" });
            //businessGrid.Items.Add(new Business() { name = "business3", state = "NV", city = "Las Vegas" });

        }
        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = yelpersDb; password=root";
        }
        private void addState()
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state";   //sql query
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            stateList.Items.Add(reader.GetString(0));
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


            //stateList.Items.Add("WA");
            //stateList.Items.Add("CA");
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
        private void addCity(NpgsqlDataReader R)
        {
            cityList.Items.Add(R.GetString(0));
        }
        private void stateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cityList.Items.Clear();
            if (stateList.SelectedIndex > -1)
            {

                string sqlstr = "SELECT distinct city FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' ORDER BY city";   //sql query
                executeQuery(sqlstr, addCity);

            }

        }

        private void addZipCode(NpgsqlDataReader R)
        {
            zipcodeList.Items.Add(R.GetString(0));
        }

        private void cityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zipcodeList.Items.Clear();
            if (cityList.SelectedIndex > -1)
            {
                string sqlstr = "SELECT distinct zipcode FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER BY zipcode"; //sql queruy
                executeQuery(sqlstr, addZipCode);


            }

        }

        private void addCategory(NpgsqlDataReader R)
        {
            categoryList.Items.Add(R.GetString(0));
        }

        private void zipcodeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoryList.Items.Clear();
            if (zipcodeList.SelectedIndex > -1)
            {
                string sqlstr = "SELECT distinct categoryname from business join category on business.businessid=category.businessid WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND zipcode = '" + zipcodeList.SelectedItem.ToString() + "'"; //sql queruy
                executeQuery(sqlstr, addCategory);


            }

        }
        //SELECT businessname from business join category on business.businessid=category.businessid WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND zipcode = '" + zipcodeList.SelectedItem.ToString() + "' and categoryname = '" + categoryList.SelectedItem.ToString() + "'"
        //select businessname from business join category on business.businessid = category.businessid where categoryname = 'Restaurants';
        private string executeDistance(string sqlstr)
        {
            string distance = "";
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
                        {
                            distance = reader.GetValue(2).ToString();
                        }
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
            return distance;
        }


        private void addBusiness(NpgsqlDataReader R)
        {

            Business b1 = new Business() { name = R.GetString(0), state = R.GetString(1), city = R.GetString(2), address = R.GetString(3), busRating = R.GetValue(4).ToString(), numReviews = R.GetValue(5).ToString(), avgRating = R.GetValue(6).ToString(), numCheckins = R.GetValue(7).ToString(), bid = R.GetString(8) };
            string query = "SELECT p.latitude,p.longitude,myDistance(p.latitude, p.longitude, b.latitude, b.longitude) FROM business b, person p where p.personid = '" + UID + "' and b.businessid = '" + b1.bid + "'";
            string distance = executeDistance(query);
            b1.distance = distance;
            businessGrid.Items.Add(b1);

        }


        //query to display businesses in a certain city/state combo -- relates to addGridRow
        //"SELECT businessname,state,city,businessid FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER BY businessname;"
        private void findBusButton_Click(object sender, RoutedEventArgs e)
        {
            if (zipcodeList.SelectedItem != null && stateList.SelectedItem != null && cityList != null)
            {
                businessGrid.Items.Clear();
                String zipcode = zipcodeList.SelectedItem.ToString();
                String state = stateList.SelectedItem.ToString();
                String city = cityList.SelectedItem.ToString();
                System.Collections.IList categories = categoryList.SelectedItems;
                if (categories.Count == 0)
                {
                    if (zipcodeList.SelectedIndex > -1)
                    {
                        string sqlstr = "SELECT businessname, state, city, address, businessrating, reviewcount, stars, numcheckins, businessid FROM business WHERE state = '" + state + "' AND city = '" + city + "' and zipcode = '" + zipcode + "' order by businessname"; //sql queruy
                        executeQuery(sqlstr, addBusiness);
                    }
                }
                else
                {
                    int index = 0;
                    string sqlstr = "SELECT businessname, state, city, address, businessrating, reviewcount, stars, numcheckins, b.businessid FROM business b ";
                    foreach (String category in categories)
                    {
                        sqlstr += " join category c" + index + " on b.businessid = c" + index + ".businessid and c" + index + ".categoryname = '" + category + "'";
                        index++;
                    }
                    sqlstr += " WHERE state = '" + state + "' AND city = '" + city + "' and zipcode = '" + zipcode + "' order by businessname";
                    executeQuery(sqlstr, addBusiness);
                }
            }
        }

        private void addReview(NpgsqlDataReader R)
        {
            // reviewList.Items.Add(R.GetString(0));
        }

        // string sqlstr = "SELECT text FROM business b join review r ON b.businessid = r.businessid where b.businessid = '" + busID + "'"; //sql query
        // executeQuery(sqlstr, addReview);
        private void addBusInfo(NpgsqlDataReader R)
        {
            busInfoList.Items.Add("Today's Hours: " + R.GetString(2) + " -- " + R.GetString(3));

        }
        private void showBusCategories(NpgsqlDataReader R)
        {
            busCategoryList.Items.Add(R.GetString(0));

        }
        private void businessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            busInfoList.Items.Clear();
            busCategoryList.Items.Clear();

            if (businessGrid.SelectedIndex > -1)
            {
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                String busID = B.bid;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    busInfoList.Items.Add("Business Name: " + B.name);
                    busInfoList.Items.Add("Business Adress: " + B.address);
                    string sqlstr = " select b.businessid, day, open, close from business b join hours h on b.businessid = h.businessid where day = to_char(now(),'FMDay') and b.businessid = '" + busID + "'";
                    executeQuery(sqlstr, addBusInfo);
                    string sqlstr2 = "select categoryname from category c join business b on b.businessid = c.businessid where b.businessid = '" + busID + "'";
                    executeQuery(sqlstr2, showBusCategories);
                }
            }
        }

        private void sortList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            if (zipcodeList.SelectedItem != null && stateList.SelectedItem != null && cityList.SelectedItem != null && sortList.SelectedItem != null)
            {
                String zipcode = zipcodeList.SelectedItem.ToString();
                String state = stateList.SelectedItem.ToString();
                String city = cityList.SelectedItem.ToString();
                String sortby = ((ComboBoxItem)sortList.SelectedItem).Content.ToString();
                System.Collections.IList categories = categoryList.SelectedItems;
                if (categories.Count == 0)
                {
                    if (zipcodeList.SelectedIndex > -1)
                    {
                        string sqlstr = "SELECT businessname, state, city, address, businessrating, reviewcount, stars, numcheckins, businessid FROM business WHERE state = '" + state + "' AND city = '" + city + "' and zipcode = '" + zipcode + "' order by " + sortby; //sql queruy
                        executeQuery(sqlstr, addBusiness);
                    }
                }
                else
                {
                    int index = 0;
                    string sqlstr = "SELECT businessname, state, city, address, businessrating, reviewcount, stars, numcheckins, b.businessid FROM business b ";
                    foreach (String category in categories)
                    {
                        sqlstr += " join category c" + index + " on b.businessid = c" + index + ".businessid and c" + index + ".categoryname = '" + category + "'";
                        index++;
                    }
                    sqlstr += " WHERE state = '" + state + "' AND city = '" + city + "' and zipcode = '" + zipcode + "' order by " + sortby;
                    executeQuery(sqlstr, addBusiness);
                }
                if (sortby == "Nearest")
                {
                    //businessGrid.Items.SortDescriptions.Add(new SortDescription("distance", ListSortDirection.Ascending));

                    // Refresh items to display sort
                    //businessGrid.Items.Refresh();
                    //DataGridColumnHeaderItemAutomationPeer peer = new DataGridColumnHeaderItemAutomationPeer(businessGrid.Columns."distance");
                    // IInvokeProvider invoker = (IInvokeProvider)peer;
                    //invoker.Invoke(); // Invoke a click programmatically
                }
            }
        }

        private void showReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedItem != null) {
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                String busID = B.bid;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    Reviews reviewWindow = new Reviews(busID, UID);
                    reviewWindow.Show();

                }
            }
        }

        private void addToFavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedItem != null) { 
            Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
            String busID = B.bid;
            if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
            {
                string querystr = "insert into favor  (personid, businessid) values ('" + UID + "', '" + busID + "')";
                executeQuery(querystr);
            }
        }
    }

        private void loadUserButton_Click(object sender, RoutedEventArgs e)
        {

            userInfo.Show();
            Close();
        }

        private void checkInButton_Click(object sender, RoutedEventArgs e)
        {
            DayOfWeek wk = DateTime.Today.DayOfWeek;
            string day1 = wk.ToString();
            var src = DateTime.Now;
            string day = src.DayOfWeek.ToString();
            string hour = src.Hour.ToString();
            hour += ":00";

            if (businessGrid.SelectedItem != null) { 
            Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
            String busID = B.bid;
            if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
            {
                string querystr = "update checkin set numpeople = numpeople +1 where day = '" + day + "' and hour = '" + hour + "' and businessid = '" + busID + "'";
                executeQuery(querystr);
            }
        }
    }
      
    }
}
