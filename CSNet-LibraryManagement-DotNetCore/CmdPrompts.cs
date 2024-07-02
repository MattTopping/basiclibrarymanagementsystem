namespace CSNet_LibraryManagement_DotNetCore;

public class CmdPrompts
{
    LibraryViewer librarian;

    public CmdPrompts(){
        librarian = new LibraryViewer();
    }

    /// <summary>
    ///     Function used to allow users to select what application function they would like to use.
    /// </summary>
    public void promptLibraryTasks()
    {
        Console.WriteLine("Please select an option\n" +
            "[1]: List books\n" +
            "[2]: Search for a book\n" +
            "[3]: Borrow a book\n" +
            "[4]: Return a book\n" +
            "[5]: Add a book\n" + 
            "[E]: Exit");
        string? userResponse = Console.ReadLine();
        switch (userResponse)
        {
            case "1":
                promptBookList();
                promptLibraryTasks();
                break;
            case "2":
                promptBookSearch();
                promptLibraryTasks();
                break;
            case "3":
                promptBookBorrow();
                promptLibraryTasks();
                break;
            case "4":
                promptBookReturn();
                promptLibraryTasks();
                break;
            case "5":
                promptBookAddition();
                promptLibraryTasks();
                break;
            case "E":
                Console.WriteLine("Thank you, see you again soon!");
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
        try{
            List<string> bookList = librarian.fetchAllBooks();
            printBookList(bookList);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void promptBookSearch()
    {
        Console.WriteLine("What would you like to search by:\n" +
            "[1]: By Title\n" +
            "[2]: By Author\n" +
            "[3]: By Publication Year");
        string? userResponse = Console.ReadLine();
        switch (userResponse)
        {
            case "1":
                Console.WriteLine("Please enter a title:");
                string? inputTitle = Console.ReadLine();
                if (inputTitle != null)  //If there is a valid input key, search and print results to screen
                {
                    try{
                        List<string> bookList = librarian.fetchBooksByTitle(inputTitle);
                        printBookList(bookList);
                    }
                    catch(Exception ex){
                        Console.WriteLine(ex.Message);
                    }
                }
                else 
                {
                    Console.WriteLine("No value entered.");
                }
                break;
            case "2":
                Console.WriteLine("Please enter an author:");
                string? inputAuthor = Console.ReadLine();
                if (inputAuthor != null) //If there is a valid input key, search and print results to screen
                {
                    try{
                        List<string> bookList = librarian.fetchBooksByAuthor(inputAuthor);
                        printBookList(bookList);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("No value entered.");
                }
                break;
            case "3":
                Console.WriteLine("Please enter a publication year:");
                string? inputYear = Console.ReadLine();
                if (inputYear != null) 
                {
                    int inputYearAsInt;
                    try // ensure input is a value int before searching against an int col in the DB
                    {
                        inputYearAsInt = int.Parse(inputYear);
                        try{
                            List<string> bookList = librarian.fetchBooksByPublicationYear(inputYearAsInt);
                            printBookList(bookList);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("A valid number as not provided. Unable to search.");
                    }

                }
                else
                {
                    Console.WriteLine("No value entered.");
                }
                break;
            default:
                Console.WriteLine("Invalid option...");
                break;
        }
    }

    /// <summary>
    ///     Function used to the prompt the user to borrow a book
    /// </summary>
    private void promptBookBorrow()
    {
        Console.WriteLine("Enter the GUID of the book you would like to borrow:");
        string? inputGuid = Console.ReadLine();
        if (inputGuid != null)
        {
            Guid inputGuidAsGuid;
            try // ensure provide input is a valid GUID
            {
                inputGuidAsGuid = Guid.Parse(inputGuid);
                Book book = new Book(inputGuidAsGuid);
                try // try borrow and catch validation exceptions
                {
                    book.borrowBook();
                    Console.WriteLine("Book borrowed!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch
            {
                Console.WriteLine("A valid guid as not provided. Unable to borrow book.");
            }
        }
        else
        {
            Console.WriteLine("No value entered.");
        }
    }

    /// <summary>
    ///     Funciton used to prompt a user through a book return
    /// </summary>
    private void promptBookReturn()
    {
        Console.WriteLine("Enter the GUID of the book you would like to return:");
        string? inputGuid = Console.ReadLine();
        if (inputGuid != null)
        {
            Guid inputGuidAsGuid;
            try // ensure provide input is a valid GUID
            {
                inputGuidAsGuid = Guid.Parse(inputGuid);

                //TODO: Single use (move to another class?)
                Book book = new Book(inputGuidAsGuid);
                try // try return and catch validation exceptions
                {
                    book.returnBook();
                    Console.WriteLine("Book returned!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch
            {
                Console.WriteLine("A valid guid as not provided. Unable to borrow book.");
            }
        }
        else
        {
            Console.WriteLine("No value entered.");
        }
    }

    /// <summary>
    ///     Function used to add a row into the database.
    /// </summary>
    private void promptBookAddition()
    {
        string title;
        string author;
        int publicationYear;

        //Valid title input by ensuring it is not null. Return to main menu if it is not valid.
        Console.WriteLine("You have selected to add a book to our library. Please provide the following.\n" +
            "Title:");
        string? inputtedTitle = Console.ReadLine();
        if(inputtedTitle != null){
            title = inputtedTitle;
        }
        else{
            Console.WriteLine("No value entered. Ending operation...");
            promptLibraryTasks();
            return;
        }

        //Valid author input by ensuring it is not null. Return to main menu if it is not valid.
        Console.WriteLine("Author: ");
        string? inputtedAuthor = Console.ReadLine();
        if(inputtedAuthor != null){ 
            author = inputtedAuthor;
        }
        else{
            Console.WriteLine("No value entered. Ending operation...");
            promptLibraryTasks();
            return;
        }

        //Valid title publication year by ensuring it is not null. Return to main menu if it is not valid.
        Console.WriteLine("Publication Year: ");
        string? inputtedPublicationYear = Console.ReadLine();
        if (inputtedPublicationYear != null){
            try
            {
                publicationYear = int.Parse(inputtedPublicationYear);
            }
            catch
            {
                Console.WriteLine("A valid number as not provided. Book addition failed.");
                promptLibraryTasks();
                return;
            }
        }
        else{
            Console.WriteLine("No value entered. Ending operation...");
            promptLibraryTasks();
            return;
        }

        //Push the inputted values into the database through the book object
        Book newBook = new Book(title, author, publicationYear);
        try
        {
            newBook.push();
            Console.WriteLine("Book created!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    ///     Small helper function that prints out each string item in a list
    /// </summary>
    /// <param name="bookList"></param>
    private void printBookList(List<string> bookList)
    {
        if (bookList.Count > 0)
        {
            foreach (string book in bookList)
            {
                Console.WriteLine(book);
            }
        }
        else
        {
            Console.WriteLine("There are no books for your query.");
        }
    }
}