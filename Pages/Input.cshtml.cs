using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ACS_Form.Pages
{
    public class InputModel : PageModel
    {
        Database database;
        public List<UserInput> userInput { get; set; }

        public void OnGet()
        {
            database = new Database();
            userInput = database.GetUserInput();
        }
    }
}
