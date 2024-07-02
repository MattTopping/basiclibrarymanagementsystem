namespace CSNet_LibraryManagement_DotNetCore;
using Microsoft.Data.SqlClient;
using System.Data;

public class LibraryViewer
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

            //Check SQL values is not null
            bookid = !sqlReader.IsDBNull(sqlReader.GetOrdinal("Book ID")) ? (Guid)sqlReader["Book ID"] : Guid.Empty;
            title = !sqlReader.IsDBNull(sqlReader.GetOrdinal("Title")) ? (string)sqlReader["Title"] : string.Empty;
            author = !sqlReader.IsDBNull(sqlReader.GetOrdinal("Author")) ? (string)sqlReader["Author"] : string.Empty;
            publicationYear = !sqlReader.IsDBNull(sqlReader.GetOrdinal("Publication Year")) ? (int)sqlReader["Publication Year"] : 0; //needs refactoring. 0 is not an accurate representation of null.

            //Check SQL value is not null and set to related character for console (X = Borrowed, 0 = Available)
            status = !sqlReader.IsDBNull(sqlReader.GetOrdinal("Borrowed")) && (bool)sqlReader["Borrowed"] == true ? "X" : "O";

            // List row in console
            books.Add(String.Format("{0} ({4}): {1} by {2} ({3})", bookid, title, author, publicationYear, status));
        }
        return books;
    }
}