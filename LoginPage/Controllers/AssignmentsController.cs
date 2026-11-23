using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.EntityFrameworkCore;  
using System.Security.Claims; 
using LoginPage.Data;
using LoginPage.Models;

namespace LoginPage.Controllers
{
    [Authorize] // KİLİT: Sadece giriş yapmış (Cookie'si olan) kullanıcılar bu controller'a erişebilir!
    public class AssignmentsController : Controller
    {
        private readonly AppDbContext _context;

        public AssignmentsController(AppDbContext context)
        {
            _context = context;
        }

        // Oturum açmış kullanıcının ID'sini Cookie'den okuyan yardımcı metod
        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userIdString!);
        }

        // 1. SADECE KENDİ ÖDEVLERİNİ LİSTELEME (Async + Filtreli)
        public async Task<IActionResult> Index()
        {
            int currentUserId = GetCurrentUserId();

            var assignments = await _context.Assignments
                .Where(a => a.UserId == currentUserId) // Sadece giriş yapan kullanıcının ödevleri!
                .ToListAsync();

            return View(assignments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 2. YENİ ÖDEV EKLEME (Dinamik UserId + Async)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Assignment assignment)
        {
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                // Artık sabit "1" değil, kim giriş yaptıysa onun ID'si atanıyor!
                assignment.UserId = GetCurrentUserId();

                _context.Assignments.Add(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assignment);
        }

        // 3. DURUM DEĞİŞTİRME (Artık GET değil, güvenli POST!)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            int currentUserId = GetCurrentUserId();

            // Hem ödev ID'si tutmalı hem de bu ödev gerçekten bu kullanıcıya ait olmalı!
            var assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == currentUserId);

            if (assignment != null)
            {
                assignment.Status = status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // 4. SİLME İŞLEMİ (Artık GET değil, güvenli POST!)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            int currentUserId = GetCurrentUserId();

            var assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == currentUserId);

            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            int currentUserId = GetCurrentUserId();

            var assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == currentUserId);

            if (assignment == null)
            {
                return NotFound(); // Başkasının ödevini düzenlemeye çalışırsa bulamadım de!
            }
            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Assignment assignment)
        {
            int currentUserId = GetCurrentUserId();
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                
                var existingAssignment = await _context.Assignments
                    .FirstOrDefaultAsync(a => a.Id == assignment.Id && a.UserId == currentUserId);

                if (existingAssignment == null)
                {
                    return NotFound();
                }

                existingAssignment.ClassName = assignment.ClassName;
                existingAssignment.Title = assignment.Title;
                existingAssignment.Description = assignment.Description;
                existingAssignment.DueDate = assignment.DueDate;
                existingAssignment.Status = assignment.Status;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assignment);
        }
    }
}