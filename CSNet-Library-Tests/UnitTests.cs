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

        [TestMethod]
        public void LibraryViewer_ListBooks()
        {
            LibraryViewer librarian = new LibraryViewer();
            List<string> books = librarian.fetchAllBooks();
            string expectedBookFormat = String.Format("{0} (O): {1} by {2} ({3})", createdBookId, testBookTitle, testBookAuthor, testBookPublicationYear);
            Assert.IsTrue(books.Contains(expectedBookFormat), "The book used in the test suite was not listed by the worker class");
        }

        [TestMethod]
        public void LibraryViewer_SearchBookByTitle()
        {
            LibraryViewer librarian = new LibraryViewer();
            List<string> books = librarian.fetchBooksByTitle(testBookTitle);
            string expectedBookFormat = String.Format("{0} (O): {1} by {2} ({3})", createdBookId, testBookTitle, testBookAuthor, testBookPublicationYear);
            Assert.IsTrue(books.Contains(expectedBookFormat), "The book used in the test suite was not listed by the worker class");
        }

        [TestMethod]
        public void LibraryViewer_SearchBookByWrongTitle()
        {
            LibraryViewer librarian = new LibraryViewer();
            List<string> books = librarian.fetchBooksByTitle("Random search key");
            string expectedBookFormat = String.Format("{0} (O): {1} by {2} ({3})", createdBookId, testBookTitle, testBookAuthor, testBookPublicationYear);
            Assert.IsFalse(books.Contains(expectedBookFormat), "The book used in the test suite was listed by the worker class");
        }


        [TestMethod]
        public void Book_PullInfoFromDb()
        {
            Book book = new Book(createdBookId);     

            Assert.IsTrue(testBookTitle.Equals(book.Title), "Book object does not have a matching title");
            Assert.IsTrue(testBookAuthor.Equals(book.Author), "Book object does not have a matching author");
            Assert.IsTrue(testBookPublicationYear.Equals(book.PublicationYear), "Book object does not have a matching publication year");
            Assert.IsTrue(book.Borrowed.Equals(false), "Book object does not have borrowed bool as default false value");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Book_BorrowBorrowedBook()
        {
            Book book = new Book(createdBookId);
            book.borrowBook();
            book.borrowBook();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Book_ReturnReturnedBook()
        {
            Book book = new Book(createdBookId);
            book.returnBook();
        }
    }
}