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
            "[ ]: Search for a book\n" +
            "[ ]: Borrow a book\n" +
            "[ ]: Return a book\n" +
            "[5]: Add a book\n" + 
            "[E]: Exit");
        string? userResponse = Console.ReadLine();
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
        if (bookList.Count > 0){
            foreach (string book in bookList){
                Console.WriteLine(book);
            }
        }
        else{
            Console.WriteLine("There are currently no books in the library.");
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
                publicationYear = Int32.Parse(inputtedPublicationYear);
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


    /// <summary>
    ///     Function is used to initial the user action to either login or create or use during the session.
    ///     User logic is currently not implemented.
    /// </summary>
    public void setUser()
    {
        Console.WriteLine("Please select an option\n" +
            "[1]: Login\n" +
            "[2]: Create");
        string? userResponse = Console.ReadLine();
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