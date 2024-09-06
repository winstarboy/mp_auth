using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace MealPlanner
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\v11.0;AttachDbFilename=|DataDirectory|\\MealPlanner.mdf;Integrated Security=True");
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void log_button_Click(object sender, EventArgs e)
        {
            string email = log_email.Text.Trim();
            string password = log_password.Text.Trim();

            // Log input values for debugging
            System.Diagnostics.Debug.WriteLine("Email: " + email);
            System.Diagnostics.Debug.WriteLine("Password: " + password);

            string check = "SELECT COUNT(*), isAdmin FROM [Table] WHERE Email = @Email AND Password = @Password GROUP BY isAdmin";
            using (SqlCommand com = new SqlCommand(check, con))
            {
                com.Parameters.AddWithValue("@Email", email);
                com.Parameters.AddWithValue("@Password", password);

                try
                {
                    con.Open();
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int temp = Convert.ToInt32(reader[0]);
                            int isAdmin = Convert.ToInt32(reader[1]);

                            if (temp == 1)
                            {
                                if (isAdmin == 1)
                                {
                                    Response.Write("<script>alert('Admin Login successful!'); window.location.href = 'AdminDashboard.aspx';</script>");
                                }
                                else
                                {
                                    Response.Write("<script>alert('Login successful!'); window.location.href = 'Homepage.aspx';</script>");
                                }
                            }
                            else
                            {
                                Response.Write("<script>alert('Login unsuccessful. Please check your email and password.');</script>");
                            }
                        }
                        else
                        {
                            Response.Write("<script>alert('Login unsuccessful. Please check your email and password.');</script>");
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    // Exceptional error: Unexpected error during database operation
                    string errorMessage = ex.Message.Replace("'", "\\'");
                    Response.Write("<script>alert('An error occurred: " + errorMessage + "');</script>");
                }
            }
        }



    }
}