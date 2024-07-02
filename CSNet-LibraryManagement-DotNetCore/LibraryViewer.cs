namespace CSNet_LibraryManagement_DotNetCore;
using Microsoft.Data.SqlClient;
using System.Data;

public class LibraryViewer
{
    Utilities tools;

    public LibraryViewer(){
        tools = new Utilities();
    }

    /// <summary>
    ///     Function to return all rows on the dbo.books table
    /// </summary>
    /// <returns>List of all found books</returns>
    public List<string> fetchAllBooks()
    {
        List<string> books = new List<string>();

        string sqlQuery = "Select [Book ID], Title, Author, [Publication Year], Borrowed " +
        "FROM [dbo].[Books] " +
        "ORDER BY Title";

        //Open SQL connection
        using (SqlConnection sqlConnection = new SqlConnection(tools.DbConectionString)) 
        {
            using(SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
            {
                sqlConnection.Open();
                using (SqlDataReader sqlReader = command.ExecuteReader())
                {
                    //Use helper function to format results
                    books = formatBooksAsString(sqlReader);
                    sqlReader.Close();
                }
                sqlConnection.Close();
            }
        }
        return books;
    }

    /// <summary>
    ///     Search function used to return all dbo.books record that match a provided Title.
    /// </summary>
    /// <param name="inputTitle">Title search key</param>
    /// <returns>List of all found books</returns>
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
                //Map title into sqlQuery to avoid SQL injection
                command.Parameters.Add("@title", SqlDbType.NVarChar);
                command.Parameters["@title"].Value = "%" + inputTitle + "%";

                sqlConnection.Open();
                using (SqlDataReader sqlReader = command.ExecuteReader())
                {
                    //Use helper function to format results
                    books = formatBooksAsString(sqlReader);
                    sqlReader.Close();
                }
                sqlConnection.Close();
            }
        }
        return books;
    }

    /// <summary>
    ///     Search function used to return all dbo.books record that match a provided author.
    /// </summary>
    /// <param name="inputAuthor">Author search key</param>
    /// <returns>List of all found books</returns>
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
                //Map author into sqlQuery to avoid SQL injection
                command.Parameters.Add("@author", SqlDbType.NVarChar);
                command.Parameters["@author"].Value = "%" + inputAuthor + "%";

                sqlConnection.Open();
                using (SqlDataReader sqlReader = command.ExecuteReader())
                {
                    //Use helper function to format results
                    books = formatBooksAsString(sqlReader);
                    sqlReader.Close();
                }
                sqlConnection.Close();
            }
        }
        return books;
    }

    /// <summary>
    ///     Search function used to return all dbo.books record that match a provided publication year.
    /// </summary>
    /// <param name="inputYear">Publication year search key</param>
    /// <returns>List of all found books</returns>
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
                //Map publication year into sqlQuery to avoid SQL injection
                command.Parameters.Add("@publicationyear", SqlDbType.Int);
                command.Parameters["@publicationYear"].Value = inputYear;

                sqlConnection.Open();
                using (SqlDataReader sqlReader = command.ExecuteReader())
                {
                    //Use helper function to format results
                    books = formatBooksAsString(sqlReader);
                    sqlReader.Close();
                }
                sqlConnection.Close();
            }
        }
        return books;
    }

    /// <summary>
    ///     Helper function takes an SQL reader and formats the values into a console friendly string
    /// </summary>
    /// <param name="sqlReader">SQL Reader that contains results for the dbo.Books table that need formatting for a console</param>
    /// <returns>List of all read rows in SQL reader as console friendly strings</returns>
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