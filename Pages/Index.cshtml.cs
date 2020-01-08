using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ACS_Form.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]

    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPost()
        {
            _ = (new Database()).RefreshData();
        }
    }
}
