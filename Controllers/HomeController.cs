using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoreDbSpCallMVC.Models;
using CoreDbSpCallMVC.Models.DB;

namespace CoreDbSpCallMVC.Controllers
{
    public class HomeController : Controller
    {
        #region Private Properties.  

        /// <summary>  
        /// Database Manager property.  
        /// </summary>  
        private readonly db_core_sp_callContext databaseManager;
        private readonly ILogger<HomeController> _logger;

        #endregion

        #region Public Properties  

        /// <summary>  
        /// Gets or sets department model property.  
        /// </summary>  
        [BindProperty]
        public ProductViewModel ProductVM { get; set; }

        #endregion

        public HomeController(ILogger<HomeController> logger, db_core_sp_callContext databaseManagerContext)
        {
            _logger = logger;
            this.databaseManager = databaseManagerContext;
        }

        #region Display Products and search for products by ID
        [HttpGet]
        [Obsolete]
        public async Task<IActionResult> Index(int productId = 0)
        {
            // Initialization.  
            this.ProductVM = new ProductViewModel();
            this.ProductVM.ProductID = productId;
            this.ProductVM.ProductDetail = new SpGetProductByID();
            this.ProductVM.ProductsGreaterThan1000 = new List<SpGetProductByPriceGreaterThan1000>();

            try
            {
                // Verification.  
                if (this.ProductVM.ProductID > 0)
                {
                    // Settings.  
                    var details = await this.databaseManager.GetProductByIDAsync(this.ProductVM.ProductID);
                    this.ProductVM.ProductDetail = details.First();
                }

                // Settings.  
                this.ProductVM.ProductsGreaterThan1000 = await this.databaseManager.GetProductByPriceGreaterThan1000Async();
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
            return View(this.ProductVM);
        }


        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Verification.  
                if (ModelState.IsValid)
                {
                    // Settings.  
                    string path = "/Index";

                    // Info.  
                    return this.RedirectToPage(path, new { productId = this.ProductVM.ProductID });
                }
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info. 
            return View(this.ProductVM);
        }

        #endregion

        #region Error method

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

    }
}
