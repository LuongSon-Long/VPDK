using Microsoft.AspNetCore.Mvc;

namespace HeThongQuanLyVanPhong.Controllers.View
{
    public class DoDacController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // DoDacController.cs
        public IActionResult FormXuLy()
        {
            return PartialView("~/Views/DoDac/_PartialFormXuLy.cshtml");
        }
        public IActionResult FormTiepNhan()
        {
            return PartialView("~/Views/DoDac/_PartialTiepNhan.cshtml");
        }
        public IActionResult FormBaoCao()
        {
            return PartialView("~/Views/DoDac/_PartialBaoCao.cshtml");
        }
        public IActionResult FormBanVe()
        {
            return PartialView("~/Views/DoDac/_PartialBanVe.cshtml");
        }
    }

}