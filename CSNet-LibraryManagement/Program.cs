using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSNetLibraryManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CmdPrompts menu = new CmdPrompts();
            menu.promptLibraryTasks();
        }
    }
}
