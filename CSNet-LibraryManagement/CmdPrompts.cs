using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                "[2]: Search for a book\n" +
                "[3]: Borrow a book\n" +
                "[4]: Return a book\n" +
                "[5]: Add a book");
            string userResponse = Console.ReadLine();
            switch (userResponse)
            {
                case "1":
                    promptBookList();
                    break;
                case "2":
                    promptBookSearch();
                    break;
                case "3":
                    promptBookBorrow();
                    break;
                case "4":
                    promptBookReturn();
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
            string sqlQuery = "SELECT * FROM dbo.Books";
            using (SqlConnection sqlConnection = new SqlConnection(tools.DbConectionString)) 
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);
                using (SqlDataReader sqlReader = command.ExecuteReader())
                {
                    if (sqlReader.Read())
                    {
                        Console.WriteLine(String.Format("{0}", sqlReader["title"])); // To be expanded
                    }
                    else Console.WriteLine("No books found");
                }
                sqlConnection.Close();
            };
            promptLibraryTasks();
        }

        private void promptBookSearch()
        {
            throw new NotImplementedException();
            //conn.Open();

            //SqlCommand command = new SqlCommand("Select id from [table1] where name=@zip", conn);
            //command.Parameters.AddWithValue("@zip", "india");
            //// int result = command.ExecuteNonQuery();
            //using (SqlDataReader reader = command.ExecuteReader())
            //{
            //    if (reader.Read())
            //    {
            //        Console.WriteLine(String.Format("{0}", reader["id"]));
            //    }
            //}

            //conn.Close();
        }

        private void promptBookBorrow()
        {
            throw new NotImplementedException();
        }

        private void promptBookReturn()
        {
            throw new NotImplementedException();
        }

        private void promptBookAddition()
        {
            throw new NotImplementedException();
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
