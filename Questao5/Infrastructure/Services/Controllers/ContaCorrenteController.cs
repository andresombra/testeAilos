using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Questao5.Infrastructure.Services.Controllers
{
    public class ContaCorrenteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
