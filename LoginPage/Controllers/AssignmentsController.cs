using Microsoft.AspNetCore.Mvc;
using LoginPage.Data;
using LoginPage.Models;
using System.Linq;

namespace LoginPage.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly AppDbContext _context;

        public AssignmentsController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Bütün Ödevleri Listeleme
        public IActionResult Index()
        {
            var assignment = _context.Assignments.ToList();
            return View(assignment);
        }

        // 2. Yeni Ödev Ekleme Ekranı (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 2. Yeni Ödev Kaydetme (POST)
        [HttpPost]
        public IActionResult Create(Assignment assignment)
        {
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                // Test amaçlı şimdilik UserId = 1 atıyoruz
                assignment.UserId = 1;
                _context.Assignments.Add(assignment);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(assignment);
        }

        // 3. Ödev Durumunu Tek Tıkla Güncelleme
        public IActionResult ChangeStatus(int id, string status)
        {
            var assignment = _context.Assignments.Find(id);
            if (assignment != null)
            {
                assignment.Status = status;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // 4. Ödev Silme
        public IActionResult Delete(int id)
        {
            var assignment = _context.Assignments.Find(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var assignment = _context.Assignments.Find(id);
            if (assignment == null)
            {
                return NotFound();
            }
            return View(assignment);
        }

        // --- DÜZENLEMEYİ VERİTABANINA KAYDEDER (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Assignments.Update(assignment);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(assignment);
        }
    }
}
