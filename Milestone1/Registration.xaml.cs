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
using System.Windows.Shapes;
using System.Data;
using System.Text.RegularExpressions;
using Npgsql;
using NpgsqlTypes;
using System.Security.Cryptography;

namespace Milestone1
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        public void Reset()
        {
            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxEmail.Text = "";
            textBlockUsername.Text = "";
            passwordBox1.Password = "";
            passwordBoxConfirm.Password = "";
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxUsername.Text.Length == 0 || textBoxUsername.Text.Length > 16)
            {
                errorbox.Text = "Enter a valid username.(1-16 characters in length)";
                textBoxUsername.Focus();
            }
            else if (textBoxEmail.Text.Length == 0)
            {
                errorbox.Text = "Please enter your email.";
                textBoxEmail.Focus();
            }
            
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errorbox.Text = "Please enter a valid email.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
                textBoxEmail.Focus();
            }
    
            
            else
            {
                //string firstname = textBoxFirstName.Text;
                //string lastname = textBoxLastName.Text;
                string email = textBoxEmail.Text;
                string password = passwordBox1.Password;
                string username = textBoxUsername.Text;
                if (passwordBox1.Password.Length == 0 || passwordBox1.Password.Length > 16)
                {
                    errorbox.Text = "Please enter a valid password. (1-16 characters)";
                    passwordBox1.Focus();
                }
                else if (passwordBoxConfirm.Password.Length == 0)
                {
                    errorbox.Text = "Please confirm password.";
                    passwordBoxConfirm.Focus();
                }
                else if (passwordBox1.Password != passwordBoxConfirm.Password)
                {
                    errorbox.Text = "Confirm password must be same as password.";
                    passwordBoxConfirm.Focus();
                }
                else
                {

                    errorbox.Text = "";
                    using (var connection = new NpgsqlConnection("Host = localhost; Username = postgres; Database = yelpersDb; password=root"))
                    {
                        connection.Open();
                        using (var cmd = new NpgsqlCommand("insert into Use (username, password, email) select @x, @y, @z where not exists(select 1 from Use where username = @x);", connection))
                        {
                            username = username.ToLower();
                            password += username;
                            String hashedPass = getSaltedHash(password);

                            cmd.Parameters.AddWithValue("x", NpgsqlDbType.Varchar, username);
                            cmd.Parameters.AddWithValue("y", NpgsqlDbType.Varchar, hashedPass);
                            cmd.Parameters.AddWithValue("z", NpgsqlDbType.Varchar, email);
                            var num = cmd.ExecuteNonQuery();
                            if(num > 0)
                            {
                                errorbox.Text = "You have registered successfully.";

                                
                                //App.Current.Shutdown();
                                Login b1 = new Login();
                                b1.Show();
                                Close();

                            }
                            else
                            {
                                errorbox.Text = "Your registration has failed. Please try a different username.";
                            }
                        }



                        connection.Close();
                   
                        //    Reset();
                    }
                }
            }
        }
        // byte[] bytes = Encoding.Unicode.GetBytes(text);
        // byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
        // return Convert.ToBase64String(inArray);

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


        private void textBoxUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            errorbox.Text = "";
        }

        private void textBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            errorbox.Text = "";
        }

        private void textBoxLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
       
    }
}