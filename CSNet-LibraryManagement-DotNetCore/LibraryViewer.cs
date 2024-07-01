namespace CSNet_LibraryManagement_DotNetCore;
using Microsoft.Data.SqlClient;
using System.Data;
using static System.Reflection.Metadata.BlobBuilder;

internal class LibraryViewer
{
    Utilities tools;

    public LibraryViewer(){
        tools = new Utilities();
    }

    public List<string> fetchAllBooks()
    {
        List<string> books = new List<string>();

        string sqlQuery = "Select [Book ID], Title, Author, [Publication Year], Borrowed " +
        "FROM [dbo].[Books] " +
        "ORDER BY Title";

        //Open SQL connection
        using (SqlConnection sqlConnection = new SqlConnection(tools.DbConectionString)) 
        {
            sqlConnection.Open();
            SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);
            using (SqlDataReader sqlReader = command.ExecuteReader())
            {
                books = formatBooksAsString(sqlReader);
                sqlReader.Close();
            }
            sqlConnection.Close();
        }
        return books;
    }

    public List<string> fetchBooksByTitle(string inputTitle)
    {
        List<string> books = new List<string>();

        string sqlQuery = "Select [Book ID], Title, Author, [Publication Year], Borrowed " +
        "FROM [dbo].[Books] " +
        "WHERE Title LIKE @title " +
        "ORDER BY Title";

        //Open SQL connection
        using (SqlConnection sqlConnection = new SqlConnection(tools.DbConectionString))
        {
            using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection)) 
            {
                command.Parameters.Add("@title", SqlDbType.NVarChar);
                command.Parameters["@title"].Value = "%" + inputTitle + "%";

                sqlConnection.Open();
                using (SqlDataReader sqlReader = command.ExecuteReader())
                {
                    books = formatBooksAsString(sqlReader);
                    sqlReader.Close();
                }
                sqlConnection.Close();
            }
        }
        return books;
    }

    public List<string> fetchBooksByAuthor(string inputAuthor)
    {
        List<string> books = new List<string>();

        string sqlQuery = "Select [Book ID], Title, Author, [Publication Year], Borrowed " +
        "FROM [dbo].[Books] " +
        "WHERE Author LIKE @author " +
        "ORDER BY Title";

        //Open SQL connection
        using (SqlConnection sqlConnection = new SqlConnection(tools.DbConectionString))
        {
            using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
            {
                command.Parameters.Add("@author", SqlDbType.NVarChar);
                command.Parameters["@author"].Value = "%" + inputAuthor + "%";

                sqlConnection.Open();
                using (SqlDataReader sqlReader = command.ExecuteReader())
                {
                    books = formatBooksAsString(sqlReader);
                    sqlReader.Close();
                }
                sqlConnection.Close();
            }
        }
        return books;
    }

    public List<string> fetchBooksByPublicationYear(int inputYear)
    {
        List<string> books = new List<string>();

        string sqlQuery = "Select [Book ID], Title, Author, [Publication Year], Borrowed " +
        "FROM [dbo].[Books] " +
        "WHERE [Publication Year]=@publicationYear " +
        "ORDER BY Title";

        //Open SQL connection
        using (SqlConnection sqlConnection = new SqlConnection(tools.DbConectionString))
        {
            using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
            {
                command.Parameters.Add("@publicationyear", SqlDbType.Int);
                command.Parameters["@publicationYear"].Value = inputYear;

                sqlConnection.Open();
                using (SqlDataReader sqlReader = command.ExecuteReader())
                {
                    books = formatBooksAsString(sqlReader);
                    sqlReader.Close();
                }
                sqlConnection.Close();
            }
        }
        return books;
    }

    private List<string> formatBooksAsString(SqlDataReader sqlReader) 
    {
        List<string> books = new List<string>();

        while (sqlReader.Read())
        {
            Guid bookid;
            string title;
            string author;
            int publicationYear;
            string status;

            //Check SQL values for null
            if (!sqlReader.IsDBNull(sqlReader.GetOrdinal("Book ID"))) bookid = (Guid)sqlReader["Book ID"];
            else bookid = Guid.Empty;

            if (!sqlReader.IsDBNull(sqlReader.GetOrdinal("Title"))) title = (string)sqlReader["Title"];
            else title = string.Empty;

            if (!sqlReader.IsDBNull(sqlReader.GetOrdinal("Author"))) author = (string)sqlReader["Author"];
            else author = string.Empty;

            if (!sqlReader.IsDBNull(sqlReader.GetOrdinal("Publication Year"))) publicationYear = (int)sqlReader["Publication Year"];
            else publicationYear = 0;

            if (!sqlReader.IsDBNull(sqlReader.GetOrdinal("Borrowed")) && (bool)sqlReader["Borrowed"] == true) status = "X";
            else status = "O";

            // List row in console
            books.Add(String.Format("{0} ({4}): {1} by {2} ({3})", bookid, title, author, publicationYear, status));
        }
        return books;
    }
}
