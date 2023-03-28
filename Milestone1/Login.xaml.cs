using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Milestone1
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        Registration registration = new Registration();
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxUsername.Text.Length == 0 || textBoxUsername.Text.Length > 16)
            {
                errorbox.Text = "Enter a valid username.(1-16 characters in length)";
                textBoxUsername.Focus();
            }
            else if (passwordBox1.Password.Length == 0 || passwordBox1.Password.Length > 16)
            {
                errorbox.Text = "Please enter a valid password. (1-16 characters)";
                passwordBox1.Focus();
            }
            else
            {
                string username = System.Security.SecurityElement.Escape(textBoxUsername.Text);
                string password = System.Security.SecurityElement.Escape(passwordBox1.Password);
                using (var connection = new NpgsqlConnection("Host = localhost; Username = postgres; Database = yelpersDb; password=root"))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("select * from Use where username = @x and password = @y", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        password += username;
                        String hashedPass = getSaltedHash(password);
                        cmd.Parameters.AddWithValue("x", NpgsqlDbType.Varchar, username);
                        cmd.Parameters.AddWithValue("y", NpgsqlDbType.Varchar, hashedPass);
                        try
                        {
                            var reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                //App.Current.Shutdown();
                               
                                Console.Write(reader.Read());
                                MainWindow b1 = new MainWindow(username);
                                b1.Show();
                                Close();

                            }
                            else
                          {
                                errorbox.Text = "Sorry! Please enter existing username/password.";
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

                        //welcome.TextBlockName.Text = username;//Sending value from one form to another form.  
                        //welcome.Show();


                    }
                }
            }
        }
        private String getSaltedHash(String text)
        {
            String salted = "";

            foreach (char c in text)
            {
                salted.Append(Convert.ToChar(c + 5));

            }
               // text += "abc";
             byte[] data = System.Text.Encoding.ASCII.GetBytes(text);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            //String hash = System.Text.Encoding.ASCII.GetString(data);     
            var hashedPassword = Convert.ToBase64String(data);
            return hashedPassword;
        }
        // byte[] bytes = Encoding.Unicode.GetBytes(text);
        // byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
        // return Convert.ToBase64String(inArray);



        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            registration.Show();
            Close();
        }
        void PasswordChangedHandler(Object sender, RoutedEventArgs args)
        {
            errorbox.Text = "";
        }

        private void textBoxUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            errorbox.Text = "";
        }
    }
}