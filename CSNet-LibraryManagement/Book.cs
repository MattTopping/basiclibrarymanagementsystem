using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSNetLibraryManagement
{
    internal class Book
    {
        private Utilities tools = new Utilities();

        private Guid bookId;
        private Guid borrwingUserId;

        private string title;
        private string author;
        private int publicationYear;

        /// <summary>
        ///     Constructer used to pull an existing record from the database
        /// </summary>
        /// <param name="requestBookGuid"></param>
        public Book(Guid requestBookGuid) 
        { 
            this.bookId = requestBookGuid;
            pull(this.bookId);
        }

        /// <summary>
        ///     Constructor used to create a new book to be added to the database
        /// </summary>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="publicationYear"></param>
        public Book(string title, string author, int publicationYear) 
        {
            this.title = title;
            this.author = author;
            this.publicationYear = publicationYear;
        }

        /// <summary>
        ///     Function used to upsert a book into the database
        /// </summary>
        public void push() 
        {
            string sqlStatement;

            // If record has a GUID, use for update, otherwise, create a new record.
            if (this.bookId == Guid.Empty)
            {
                sqlStatement = "INSERT INTO dbo.Books (Title,Author,[Publication Year]) " +
                    "VALUES (@title,@author,@publicationyear)";
            }
            else 
            {
                sqlStatement = "UPDATE dbo.Books " +
                    "SET Title=@title,Author=@author,[Publication Year]=@publicationyear" +
                    "WHERE [Book ID]=@id";
            }

            using (SqlConnection sqlConnection = new SqlConnection(tools.DbConectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlStatement, sqlConnection))
                {
                    // Insert values with SQL parameters to avoid SQL injection
                    command.Parameters.Add("@title", SqlDbType.NVarChar);
                    command.Parameters.Add("@author", SqlDbType.NVarChar);
                    command.Parameters.Add("@publicationyear", SqlDbType.Int);
                    command.Parameters.Add("@id", SqlDbType.UniqueIdentifier);

                    command.Parameters["@title"].Value = this.title;
                    command.Parameters["@author"].Value = this.author;
                    command.Parameters["@publicationyear"].Value = this.publicationYear;
                    command.Parameters["@id"].Value = this.bookId;

                    // Execute SQL command
                    try
                    {
                        sqlConnection.Open();
                        Int32 rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine(title + " was created!");
                        }
                        else
                        {
                            Console.WriteLine("Creation failed, please try again");
                        }
                        sqlConnection.Close();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                };
            };
        }

        private void pull(Guid guid) 
        {
            throw new NotImplementedException();
        }

        public Guid BookID
        {
            get { return bookId; }
            set { bookId = value; }
        }
        public Guid BorrwingUserId
        {
            get { return borrwingUserId; }
            set { borrwingUserId = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        public int PublicationYear
        {
            get { return publicationYear; }
            set { publicationYear = value; }
        }
    }
}
