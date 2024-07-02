using CSNet_LibraryManagement_DotNetCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CSNet_Library_Tests
{
    [TestClass]
    public class UnitTests
    {
        private Guid createdBookId;
        private const string testBookTitle = "#01: A book used for testing";
        private const string testBookAuthor = "MSTestUnitTestSuite";
        private const int testBookPublicationYear = 2050;

        /// <summary>
        ///     Test Initialiser that creates a unique record for use throughout each test. 
        ///     This ensures the test suite is interacting with a DB record it created.
        ///     
        ///     SQL query is done outside of written classes in the main program to ensure that 
        ///         there is no reliance on untested functions.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            Utilities tools = new Utilities();
            string sqlStatement = "INSERT INTO dbo.Books (Title,Author,[Publication Year]) " +
                "OUTPUT INSERTED.[Book ID] " +
                "VALUES (@title,@author,@publicationyear)";

            using (SqlConnection sqlConnection = new SqlConnection(tools.DbConectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlStatement, sqlConnection))
                {
                    command.Parameters.Add("@title", SqlDbType.NVarChar);
                    command.Parameters.Add("@author", SqlDbType.NVarChar);
                    command.Parameters.Add("@publicationyear", SqlDbType.Int);
                    
                    command.Parameters["@title"].Value = testBookTitle;
                    command.Parameters["@author"].Value = testBookAuthor;
                    command.Parameters["@publicationyear"].Value = testBookPublicationYear;

                    sqlConnection.Open();
                    createdBookId = (Guid) command.ExecuteScalar();
                    sqlConnection.Close();
                };
            };
        }

        /// <summary>
        ///     Test Cleanup deletes the DB record created to remove any test data created by the test suite. Delete call
        ///         is targetting the same record using the GUID returned from the insert in the initalise stage.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            Utilities tools = new Utilities();
            string sqlStatement = "DELETE FROM dbo.Books " +
                "WHERE [Book ID]=CAST(@id as uniqueidentifier)";

            using (SqlConnection sqlConnection = new SqlConnection(tools.DbConectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlStatement, sqlConnection))
                {
                    command.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                    command.Parameters["@id"].Value = createdBookId;

                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                    sqlConnection.Close();
                };
            };
        }

        /// <summary>
        ///     Test the list funcitonality of Library Viewer class.
        ///     Expected: Have the recored created in the initalise step appear in the Library Viewer list.
        /// </summary>
        [TestMethod]
        public void LibraryViewer_ListBooks()
        {
            LibraryViewer librarian = new LibraryViewer();
            List<string> books = librarian.fetchAllBooks();
            string expectedBookFormat = String.Format("{0} (O): {1} by {2} ({3})", createdBookId, testBookTitle, testBookAuthor, testBookPublicationYear);
            Assert.IsTrue(books.Contains(expectedBookFormat), "The book used in the test suite was not listed by the worker class");
        }

        /// <summary>
        ///     Test the search by title functionality of the Library Viewer class.
        ///     Expected: Have the recored created in the initalise step appear in the Library Viewer list.   
        /// </summary>
        [TestMethod]
        public void LibraryViewer_SearchBookByTitle()
        {
            LibraryViewer librarian = new LibraryViewer();
            List<string> books = librarian.fetchBooksByTitle(testBookTitle);
            string expectedBookFormat = String.Format("{0} (O): {1} by {2} ({3})", createdBookId, testBookTitle, testBookAuthor, testBookPublicationYear);
            Assert.IsTrue(books.Contains(expectedBookFormat), "The book used in the test suite was not listed by the worker class");
        }

        /// <summary>
        ///     Negative test the search by title functionality of the Library Viewer class.
        ///     Expected: Have the recored created in the initalise step absent from the Library Viewer list.   
        /// </summary>
        [TestMethod]
        public void LibraryViewer_SearchBookByWrongTitle()
        {
            LibraryViewer librarian = new LibraryViewer();
            List<string> books = librarian.fetchBooksByTitle("Random search key");
            string expectedBookFormat = String.Format("{0} (O): {1} by {2} ({3})", createdBookId, testBookTitle, testBookAuthor, testBookPublicationYear);
            Assert.IsFalse(books.Contains(expectedBookFormat), "The book used in the test suite was listed by the worker class");
        }

        /// <summary>
        ///     Test that the Book object correct pulls a representation of the row in the DB.
        ///     Expected: All book object properties match the data given to the initalisation function.
        /// </summary>
        [TestMethod]
        public void Book_PullInfoFromDb()
        {
            Book book = new Book(createdBookId);     

            Assert.IsTrue(testBookTitle.Equals(book.Title), "Book object does not have a matching title");
            Assert.IsTrue(testBookAuthor.Equals(book.Author), "Book object does not have a matching author");
            Assert.IsTrue(testBookPublicationYear.Equals(book.PublicationYear), "Book object does not have a matching publication year");
            Assert.IsTrue(book.Borrowed.Equals(false), "Book object does not have borrowed bool as default false value");
        }

        /// <summary>
        ///     Negative test that borrowing cannot happen on a borrowed book. 
        ///     Expected: Exception thrown when a borrowed book is request for borrowing.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Book_BorrowBorrowedBook()
        {
            Book book = new Book(createdBookId);
            book.borrowBook();
            book.borrowBook();
        }

        /// <summary>
        ///     Negative test for returning an available book.
        ///     Expected: Exception thrown when an available book is attmepted to be returned.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Book_ReturnReturnedBook()
        {
            Book book = new Book(createdBookId);
            book.returnBook();
        }
    }
}