using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayTable
{
    public partial class DisplayAuthorsTable : Form
    {
        public DisplayAuthorsTable()
        {
            InitializeComponent();
        }

        //Entity Framework DbContext
        private BooksExamples.BooksEntities dbcontext = new BooksExamples.BooksEntities();

        private DataTable dtAuthors = new DataTable();
        private bool notLoaded = true;

        //load data from database into DataGridView
        private void DisplayAuthorsTable_Load(object sender, EventArgs e)
        {
            outputTextBox.Hide();

            if (notLoaded)
            {
                authorDataGridView.DataSource = GetAuthorsList();
            }

            notLoaded = false;
        }

        /// <summary>
        /// Gets the table of authors.
        /// </summary>
        /// <returns></returns>
        private DataTable GetAuthorsList()
        {
            string connString = ConfigurationManager.ConnectionStrings["BooksEntities"].ConnectionString;

            if (connString.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(connString);
                connString = efBuilder.ProviderConnectionString;
            }

            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Authors", con))
                {
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    dtAuthors.Load(reader);
                }
            }

            return dtAuthors;
        }

        /// <summary>
        /// Refreshes the items in the table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void authorBindingNavigator_RefreshItems(object sender, EventArgs e)
        {
            outputTextBox.Hide();

            if (notLoaded)
            {
                authorDataGridView.DataSource = GetAuthorsList();
            }

            notLoaded = false;
        }


        /// <summary>
        /// Saves items in the table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void authorBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            authorBindingSource.EndEdit();
            try
            {
                dbcontext.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                MessageBox.Show("FirstName and LastName must contain values", "Entity Validation Exception");
            }
        }

        /// <summary>
        /// Event Handler for the LastName Search Box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBox_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Search by Last Name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            outputTextBox.Hide();

            DataView dvAuthors = dtAuthors.DefaultView;

            dvAuthors.RowFilter = "LastName LIKE '%" + searchBox.Text + "%'";
        }

        /// <summary>
        /// Resets View of the AuthorID.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            outputTextBox.Hide();

            searchBox.Clear();

            titleSearchBox.Clear();

            DataView dvAuthors = dtAuthors.DefaultView;

            dvAuthors.RowFilter = "LastName LIKE '%" + "" + "%'";
        }

        private void authorDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e){ }

        private void label1_Click(object sender, EventArgs e){ }

        /// <summary>
        /// Search by title.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Click(object sender, EventArgs e)
        {
            outputTextBox.Clear();

            outputTextBox.Show();

            // Entity Framework DbContext
            var dbc = new BooksExamples.BooksEntities();
            var authorsAndTitles =
                from book in dbcontext.Titles
                from author in book.Authors
                orderby book.Title1, author.LastName, author.FirstName
                select new { author.FirstName, author.LastName, book.Title1 };

            // get authors and titles of each book they co-authored
            outputTextBox.AppendText("\r\n\r\nAuthors and Titles:");

            // display authors and titles in tabular format
            foreach (var element in authorsAndTitles)
            {
                if(element.Title1.Contains(titleSearchBox.Text))
                {
                    outputTextBox.AppendText($"\r\n\t{element.FirstName,-10} " + $"{element.LastName,-10} {element.Title1}");
                }
            }
        }

        private void authorBindingSource_CurrentChanged(object sender, EventArgs e){ }

        /// <summary>
        /// Event Handler for the Title Search Box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void titleSearchBox_TextChanged(object sender, EventArgs e){ }

        private void DisplayAuthorsTable_Load_1(object sender, EventArgs e){ }

        private void button2_Click_1(object sender, EventArgs e)
        {
            outputTextBox.Clear();

            outputTextBox.Show();

            // Entity Framework DbContext
            var dbc = new BooksExamples.BooksEntities();
            var authorsAndTitles =
                from book in dbcontext.Titles
                from author in book.Authors
                orderby book.Title1, author.LastName, author.FirstName
                select new { author.FirstName, author.LastName, book.Title1 };

            // get authors and titles of each book they co-authored
            outputTextBox.AppendText("\r\n\r\nAuthors and Titles:");

            // display authors and titles in tabular format
            foreach (var element in authorsAndTitles)
            {
                outputTextBox.AppendText($"\r\n\t{element.FirstName,-10} " + $"{element.LastName,-10} {element.Title1}");
            }
        }
    }
}
