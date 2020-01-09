using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ACS_Form.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]

    public class IndexModel : PageModel
    {
        public bool RefreshSuccess { get; set; } = false;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            bool success = (new Database()).RefreshData();
            return new JsonResult(success);
        }
    }
}
