namespace CSNet_LibraryManagement_DotNetCore;
using Microsoft.Data.SqlClient;
using System.Data;

public class Book
{
    private Utilities tools = new Utilities();

    private Guid bookId;
    private Guid borrwingUserId;
    private bool borrowed;

    private string? title;
    private string? author;
    private int? publicationYear;

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
                "SET Title=@title,Author=@author,[Publication Year]=@publicationyear,Borrowed=@borrowed " +
                "WHERE [Book ID]=CAST(@id as uniqueidentifier)";
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
                command.Parameters.Add("@borrowed", SqlDbType.Bit);

                command.Parameters["@title"].Value = this.title;
                command.Parameters["@author"].Value = this.author;
                command.Parameters["@publicationyear"].Value = this.publicationYear;
                command.Parameters["@id"].Value = this.bookId;
                command.Parameters["@borrowed"].Value = this.borrowed;

                sqlConnection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected <= 0)
                {
                    throw new Exception("Unable to create/update your book.");
                }
                sqlConnection.Close();
            };
        };
    }

    private void pull(Guid guid) 
    {
        string sqlQuery = "Select Title, Author, [Publication Year], Borrowed " +
            "FROM [dbo].[Books] " +
            "WHERE [Book ID]=CAST(@id as uniqueidentifier)";

        //Open SQL connection
        using (SqlConnection sqlConnection = new SqlConnection(tools.DbConectionString))
        {
            using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
            {
                //Mapping GUID into sqlQuery to avoid SQL injection
                command.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                command.Parameters["@id"].Value = guid;

                sqlConnection.Open();
                using (SqlDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        // check each field for null reference before mapping the value.
                        title = !sqlReader.IsDBNull(sqlReader.GetOrdinal("Title")) ? (string)sqlReader["Title"] : null;
                        author = !sqlReader.IsDBNull(sqlReader.GetOrdinal("Author")) ? (string)sqlReader["Author"] : null;
                        publicationYear = !sqlReader.IsDBNull(sqlReader.GetOrdinal("Publication Year")) ? (int)sqlReader["Publication Year"] : null;
                        borrowed = !sqlReader.IsDBNull(sqlReader.GetOrdinal("Borrowed")) ? (bool)sqlReader["Borrowed"] : false; //Db Default
                    }
                    sqlReader.Close();
                }
                sqlConnection.Close();
            }
        }
    }

    /// <summary>
    ///     Used to borrow a book. Validates if a book is eligible to borrow before executing.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void borrowBook()
    {
        if (borrowed == false)
        {
            borrowed = true;
            push(); 
        }
        else 
        {
            throw new Exception("This book is already borrowed. You cannot borrow this book.");
        }
    }

    /// <summary>
    ///     Used to return a book. Validates if a book is eligible for return before executing.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void returnBook()
    {
        if (borrowed == true)
        {
            borrowed = false;
            push(); 
        }
        else 
        {
            throw new Exception("This book is already available in the library. You cannot return this book.");
        }
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

    public bool Borrowed
    {
        get { return borrowed; }
        set { borrowed = value; }
    }

    public string? Title
    {
        get { return title; }
        set { title = value; }
    }

    public string? Author
    {
        get { return author; }
        set { author = value; }
    }

    public int? PublicationYear
    {
        get { return publicationYear; }
        set { publicationYear = value; }
    }
}