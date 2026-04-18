using _1291163_MVC_EF_Project.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _1291163_MVC_EF_Project.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class GuestsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Guests
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var guests = await db.Database
                .SqlQuery<Guest>("EXEC spGuest_List")
                .ToListAsync();

            return View(guests);
        }

        // GET: Guests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Guests/Create
        [HttpPost]
        public async Task<ActionResult> Create(Guest guest)
        {
            string photoPath = null;

            if (guest.PhotoFile != null && guest.PhotoFile.ContentLength > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(guest.PhotoFile.FileName);
                var savePath = Server.MapPath("~/Uploads/Guests/");
                Directory.CreateDirectory(savePath);

                var fullPath = Path.Combine(savePath, fileName);
                guest.PhotoFile.SaveAs(fullPath);

                photoPath = "/Uploads/Guests/" + fileName;
            }

            if (ModelState.IsValid)
            {
                await db.Database.ExecuteSqlCommandAsync(
                    "EXEC spGuest_Insert @p0,@p1,@p2,@p3,@p4",
                    guest.FullName,
                    guest.Phone,
                    guest.Email,
                    guest.Address,
                    photoPath   
                );

                return RedirectToAction("Index");
            }

            return View(guest);
        }

        // GET: Guests/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var guest = await db.Guests.FindAsync(id);
            if (guest == null) return HttpNotFound();
            return View(guest);
        }

        // POST: Guests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guest guest)
        {
            if (ModelState.IsValid)
            {
                await db.Database.ExecuteSqlCommandAsync(
                    "EXEC spGuest_Update @p0,@p1,@p2,@p3,@p4,@p5",
                    guest.GuestId,
                    guest.FullName,
                    guest.Phone,
                    guest.Email,
                    guest.Address,
                    guest.PhotoPath
                );
                return RedirectToAction("Index");
            }
            return View(guest);
        }

        // GET: Guests/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var guest = await db.Guests.FindAsync(id);
            if (guest == null) return HttpNotFound();
            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await db.Database.ExecuteSqlCommandAsync(
                "EXEC spGuest_Delete @p0", id
            );
            return RedirectToAction("Index");
        }
    }
}
