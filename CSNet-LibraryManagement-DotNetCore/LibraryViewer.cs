namespace CSNet_LibraryManagement_DotNetCore;
using Microsoft.Data.SqlClient;

internal class LibraryViewer
{
    Utilities tools;

    public LibraryViewer(){
        tools = new Utilities();
    }

    public List<string> fetchAllBooks()
    {
        List<string> books = new List<string>();

        string sqlQuery = "Select [Book ID], Title, Author, [Publication Year], [Borrowing User] " +
        "FROM [dbo].[Books] " +
        "ORDER BY Title";

        //Open SQL connection
        using (SqlConnection sqlConnection = new SqlConnection(tools.DbConectionString)) 
        {
            sqlConnection.Open();
            SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);
            using (SqlDataReader sqlReader = command.ExecuteReader())
            {
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

                    if (!sqlReader.IsDBNull(sqlReader.GetOrdinal("Borrowing User"))) status = "X";
                    else status = "O";

                    // List row in console
                    books.Add(String.Format("{0} ({4}): {1} by {2} ({3})", bookid, title, author, publicationYear, status));
                }
                sqlReader.Close();
            }
            sqlConnection.Close();
        }
        return books;
    }
}
