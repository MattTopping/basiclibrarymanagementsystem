namespace CSNet_LibraryManagement_DotNetCore;

internal class CmdPrompts
{
    //User loggedInUser; // Not yet implemented
    LibraryViewer librarian;

    public CmdPrompts(){
        librarian = new LibraryViewer();
    }

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
        List<string> bookList = librarian.fetchAllBooks();
        bookListFormatter(bookList);
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
                if (inputTitle != null)
                {
                    List<string> bookList = librarian.fetchBooksByTitle(inputTitle);
                    bookListFormatter(bookList);
                }
                else 
                {
                    Console.WriteLine("No value entered.");
                }
                break;
            case "2":
                Console.WriteLine("Please enter an author:");
                string? inputAuthor = Console.ReadLine();
                if (inputAuthor != null)
                {
                    List<string> bookList = librarian.fetchBooksByAuthor(inputAuthor);
                    bookListFormatter(bookList);
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
                    try
                    {
                        inputYearAsInt = int.Parse(inputYear);
                        List<string> bookList = librarian.fetchBooksByPublicationYear(inputYearAsInt);
                        bookListFormatter(bookList);
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

    private void promptBookBorrow()
    {
        Console.WriteLine("Enter the GUID of the book you would like to borrow:");
        string? inputGuid = Console.ReadLine();
        if (inputGuid != null)
        {
            Guid inputGuidAsGuid;
            try
            {
                //TODO: Single use (move to another class?)
                inputGuidAsGuid = Guid.Parse(inputGuid);
                Book book = new Book(inputGuidAsGuid);
                if (book.Borrowed == false)
                {
                    book.Borrowed = true;
                    book.push(); //TODO: Try/Catch? 
                    Console.WriteLine("Book borrowed!");
                }
                else 
                {
                    Console.WriteLine("Book is already borrowed. It is not available to borrow at this time.");
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

    private void promptBookReturn()
    {
        Console.WriteLine("Enter the GUID of the book you would like to return:");
        string? inputGuid = Console.ReadLine();
        if (inputGuid != null)
        {
            Guid inputGuidAsGuid;
            try
            {
                inputGuidAsGuid = Guid.Parse(inputGuid);

                //TODO: Single use (move to another class?)
                Book book = new Book(inputGuidAsGuid);
                if (book.Borrowed == true)
                {
                    book.Borrowed = false;
                    book.push(); //TODO: Try/Catch? 
                    Console.WriteLine("Book returned!");
                }
                else
                {
                    Console.WriteLine("Book is already returned. It is not available to return at this time.");
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

        Console.WriteLine("You have selected to add a book to our library. Please provide the following.\n" +
            "Title:");
        string? inputtedTitle = Console.ReadLine();
        if(inputtedTitle != null){ //TODO: investigate let statment?
            title = inputtedTitle;
        }
        else{
            Console.WriteLine("No value entered. Ending operation...");
            promptLibraryTasks();
            return;
        }

        Console.WriteLine("Author: ");
        string? inputtedAuthor = Console.ReadLine();
        if(inputtedAuthor != null){ //TODO: investigate let statment?
            author = inputtedAuthor;
        }
        else{
            Console.WriteLine("No value entered. Ending operation...");
            promptLibraryTasks();
            return;
        }

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

        Book newBook = new Book(title, author, publicationYear);
        newBook.push();
        promptLibraryTasks();
    }

    private void bookListFormatter(List<string> bookList)
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

    /// <summary>
    ///     Function is used to initial the user action to either login or create or use during the session.
    ///     User logic is currently not implemented.
    /// </summary>
    public void setUser()
    {
        throw new NotImplementedException();
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