using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ACS_Form.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                (new Database()).RefreshData();
            }).Start();
        }
    }
}
