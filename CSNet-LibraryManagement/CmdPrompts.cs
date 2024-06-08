using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CSNetLibraryManagement
{
    internal class CmdPrompts
    {
        //User loggedInUser; // Not yet implemented
        Utilities tools = new Utilities();

        /// <summary>
        ///     Function used to allow users t select what application function they would like to use.
        /// </summary>
        public void promptLibraryTasks()
        {
            Console.WriteLine("Please select an option\n" +
                "[1]: List books\n" +
                "[ ]: Search for a book\n" +
                "[ ]: Borrow a book\n" +
                "[ ]: Return a book\n" +
                "[5]: Add a book");
            string userResponse = Console.ReadLine();
            switch (userResponse)
            {
                case "1":
                    promptBookList();
                    break;
                case "2":
                    //promptBookSearch();
                    Console.WriteLine("Not yet implemented.");
                    promptLibraryTasks();
                    break;
                case "3":
                    //promptBookBorrow
                    Console.WriteLine("Not yet implemented.");
                    promptLibraryTasks();
                    break;
                case "4":
                    //promptBookReturn();
                    Console.WriteLine("Not yet implemented.");
                    promptLibraryTasks();
                    break;
                case "5":
                    promptBookAddition();
                    break;
                default:
                    Console.WriteLine("Invalid option...");
                    promptLibraryTasks();
                    break;
            }
        }

        /// <summary>
        ///     Function used to fetch a list of all books from the external SQL database in the console application.
        /// </summary>
        private void promptBookList()
        {
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

                        if (!sqlReader.IsDBNull(sqlReader.GetOrdinal("Borrowing User"))) status = "Borrowed";
                        else status = "Available";

                        // List row in console
                        Console.WriteLine(String.Format("|{0}|{1}|{2}|{3}|{4}|", bookid, title, author, publicationYear, status));
                    }
                    sqlReader.Close();
                }
                sqlConnection.Close();
            }
            promptLibraryTasks();
        }

        private void promptBookSearch()
        {
            throw new NotImplementedException();
        }

        private void promptBookBorrow()
        {
            throw new NotImplementedException();
        }

        private void promptBookReturn()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Function used to add a row into the database.
        /// </summary>
        private void promptBookAddition()
        {
            Console.WriteLine("You have selected to add a book to our library. Please provide the following.\n" +
                "Title:");
            string title = Console.ReadLine();

            Console.WriteLine("Author: ");
            string author = Console.ReadLine();
            int publicationYear;

            Console.WriteLine("Publication Year: ");
            try
            {
                publicationYear = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("A valid number as not provided. Book addition failed.");
                promptLibraryTasks();
                return;
            }

            Book newBook = new Book(title, author, publicationYear);
            newBook.push();
            promptLibraryTasks();
        }


        /// <summary>
        ///     Function is used to initial the user action to either login or create or use during the session.
        ///     User logic is currently not implemented.
        /// </summary>
        public void setUser()
        {
            Console.WriteLine("Please select an option\n" +
                "[1]: Login\n" +
                "[2]: Create");
            string userResponse = Console.ReadLine();
            switch (userResponse) {
                case "1": 
                    promptUserLogin();
                    break;
                case "2":
                    promptUserCreate();
                    break;
                default:
                    Console.WriteLine("Invalid option...");
                    setUser();
                    break;
            }
        }

        /// <summary>
        ///     Function used to check if a user exists and logs in with the correct username and password.
        ///     If complete, the user context will be set for the session
        ///     Otherwise, the user will be prompted again.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void promptUserLogin()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Function used to createa new a user record for a new user of the application.
        ///     Once a user has entered their details, they are set as the active user for the session
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void promptUserCreate() 
        { 
            throw new NotImplementedException();
        }
    }
}
