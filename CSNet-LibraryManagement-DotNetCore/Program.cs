namespace CSNet_LibraryManagement_DotNetCore;

class Program
{
    static void Main(string[] args)
    {
        CmdPrompts menu = new CmdPrompts();
        menu.promptLibraryTasks();
    }
}
