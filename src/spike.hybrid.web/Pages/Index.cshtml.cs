using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace spike.hybrid.web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }
        public void OnGet()
        {
            var conn =  Configuration.GetConnectionString("DefaultConnection");

            ViewData["Connection"] =conn?.Substring(0, conn.IndexOf("Password"));
        }
    }
}
