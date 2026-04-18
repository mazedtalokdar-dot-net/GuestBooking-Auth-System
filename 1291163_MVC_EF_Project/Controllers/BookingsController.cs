using _1291163_MVC_EF_Project.Models;
using _1291163_MVC_EF_Project.Models.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _1291163_MVC_EF_Project.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bookings
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var data = await db.Bookings
                .Include(b => b.Guest)
                .OrderByDescending(b => b.BookingId)
                .ToListAsync();

            return View(data);
        }

        // GET: Bookings/Create
        public async Task<ActionResult> Create()
        {
            var model = new BookingInputModel
            {
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1)
            };

            model.Rooms.Add(new BookedRoomRow());

            ViewBag.Guests = new SelectList(await db.Guests.ToListAsync(), "GuestId", "FullName");
            ViewBag.Rooms = new SelectList(await db.Rooms.Where(r => r.IsActive).ToListAsync(), "RoomId", "RoomNo");

            return View(model);
        }

        // POST: Bookings/Create  (MASTER + DETAILS + TRANSACTION + SP)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookingInputModel model, string act = "")
        {
            // 1) Add row (no save)
            if (act == "add")
            {
                model.Rooms.Add(new BookedRoomRow());
                ModelState.Clear();

                ViewBag.Guests = new SelectList(await db.Guests.ToListAsync(), "GuestId", "FullName", model.GuestId);
                ViewBag.Rooms = new SelectList(await db.Rooms.Where(r => r.IsActive).ToListAsync(), "RoomId", "RoomNo");
                return View(model);
            }

            // 2) Remove row (no save)
            if (act.StartsWith("remove_"))
            {
                int index = int.Parse(act.Split('_')[1]);
                if (index >= 0 && index < model.Rooms.Count)
                    model.Rooms.RemoveAt(index);

                ModelState.Clear();

                ViewBag.Guests = new SelectList(await db.Guests.ToListAsync(), "GuestId", "FullName", model.GuestId);
                ViewBag.Rooms = new SelectList(await db.Rooms.Where(r => r.IsActive).ToListAsync(), "RoomId", "RoomNo");
                return View(model);
            }

            // 3) Save booking (MASTER + DETAILS + TRANSACTION + SP)
            if (act == "insert")
            {
                // If user wants to create new guest from this page
                if (model.CreateNewGuest)
                {
                    if (string.IsNullOrWhiteSpace(model.NewGuestFullName))
                    {
                        ModelState.AddModelError("NewGuestFullName", "Guest name is required.");
                    }
                    else
                    {
                        string photoPath = null;

                        if (model.NewGuestPhoto != null && model.NewGuestPhoto.ContentLength > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(model.NewGuestPhoto.FileName);
                            var savePath = Server.MapPath("~/Uploads/Guests/");
                            System.IO.Directory.CreateDirectory(savePath);
                            var fullPath = System.IO.Path.Combine(savePath, fileName);

                            model.NewGuestPhoto.SaveAs(fullPath);
                            photoPath = "/Uploads/Guests/" + fileName;
                        }

                        var newGuest = new Guest
                        {
                            FullName = model.NewGuestFullName,
                            Phone = model.NewGuestPhone,
                            Email = model.NewGuestEmail,
                            Address = model.NewGuestAddress,
                            PhotoPath = photoPath
                        };

                        db.Guests.Add(newGuest);
                        await db.SaveChangesAsync();

                        model.GuestId = newGuest.GuestId; // use guest for booking
                    }
                }

                // Guest validation (only if NOT creating new guest)
                if (!model.CreateNewGuest && (!model.GuestId.HasValue || model.GuestId.Value <= 0))
                    ModelState.AddModelError("GuestId", "Please select a guest or register a new guest.");

                // Date validations
                if (model.CheckInDate.Date < DateTime.Today)
                    ModelState.AddModelError("", "Check-in date cannot be in the past.");

                if (model.CheckOutDate <= model.CheckInDate)
                    ModelState.AddModelError("", "Check-out date must be greater than Check-in date.");

                // Room validation
                if (model.Rooms == null || model.Rooms.Count == 0 || model.Rooms.All(x => x.RoomId <= 0))
                    ModelState.AddModelError("", "Please select at least one room.");

                if (ModelState.IsValid)
                {
                    using (var tx = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var booking = new Booking
                            {
                                GuestId = model.GuestId.Value,
                                CheckInDate = model.CheckInDate,
                                CheckOutDate = model.CheckOutDate,
                                Notes = model.Notes,
                                Status = BookingStatus.Pending
                            };

                            db.Bookings.Add(booking);
                            await db.SaveChangesAsync();

                            foreach (var r in model.Rooms)
                            {
                                if (r.RoomId <= 0) continue;
                                if (r.Nights <= 0) r.Nights = 1;

                                await db.Database.ExecuteSqlCommandAsync(
                                    "EXEC spBookedRoom_Insert @p0,@p1,@p2,@p3",
                                    booking.BookingId,
                                    r.RoomId,
                                    r.PricePerNight,
                                    r.Nights
                                );
                            }

                            tx.Commit();
                            return RedirectToAction("Index");
                        }
                        catch
                        {
                            tx.Rollback();
                            ModelState.AddModelError("", "Booking failed. Transaction rolled back.");
                        }
                    }
                }
            }

            // Reload dropdowns
            ViewBag.Guests = new SelectList(await db.Guests.ToListAsync(), "GuestId", "FullName", model.GuestId);
            ViewBag.Rooms = new SelectList(await db.Rooms.Where(r => r.IsActive).ToListAsync(), "RoomId", "RoomNo");
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> RowDetails(int id)
        {
            var booking = await db.Bookings
                .Include(b => b.Guest)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return HttpNotFound();

            var details = await db.BookedRooms
                .Include(d => d.Room)
                .Where(d => d.BookingId == id)
                .ToListAsync();

            ViewBag.Details = details;
            return PartialView("_BookingRowDetails", booking);
        }
        // GET: Bookings/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var booking = await db.Bookings
                .Include(b => b.Guest)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return HttpNotFound();

            var details = await db.BookedRooms
                .Where(d => d.BookingId == id)
                .ToListAsync();

            var model = new BookingInputModel
            {
                BookingId = booking.BookingId,
                GuestId = booking.GuestId,

                // Guest fields (editable)
                GuestFullName = booking.Guest.FullName,
                GuestPhone = booking.Guest.Phone,
                GuestEmail = booking.Guest.Email,
                GuestAddress = booking.Guest.Address,
                GuestPhotoPath = booking.Guest.PhotoPath,

                // Booking fields
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                Notes = booking.Notes,
                Status = (int)booking.Status,

                Rooms = details.Select(d => new BookedRoomRow
                {
                    RoomId = d.RoomId,
                    Nights = d.Nights,
                    PricePerNight = d.PricePerNight
                }).ToList()
            };

            if (model.Rooms.Count == 0) model.Rooms.Add(new BookedRoomRow());

            ViewBag.Rooms = new SelectList(await db.Rooms.Where(r => r.IsActive).ToListAsync(), "RoomId", "RoomNo");
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(BookingStatus)).Cast<BookingStatus>()
                .Select(s => new { Id = (int)s, Name = s.ToString() }), "Id", "Name", model.Status);

            return View(model);
        }

        // POST: Bookings/Edit  (ALL-IN-ONE UPDATE: Guest + Booking + Details)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BookingInputModel model)
        {
            // 1) Basic validations
            if (!model.GuestId.HasValue || model.GuestId.Value <= 0)
                ModelState.AddModelError("", "Guest not found for this booking.");

            if (string.IsNullOrWhiteSpace(model.GuestFullName))
                ModelState.AddModelError("GuestFullName", "Guest name is required.");

            if (model.CheckOutDate <= model.CheckInDate)
                ModelState.AddModelError("", "Check-out must be greater than Check-in.");

            if (model.Rooms == null || model.Rooms.Count == 0 || model.Rooms.All(x => x.RoomId <= 0))
                ModelState.AddModelError("", "Please select at least one room.");

            if (!ModelState.IsValid)
            {
                // reload dropdowns and return view
                ViewBag.Rooms = new SelectList(await db.Rooms.Where(r => r.IsActive).ToListAsync(), "RoomId", "RoomNo");
                ViewBag.Status = new SelectList(
                    Enum.GetValues(typeof(BookingStatus)).Cast<BookingStatus>()
                        .Select(s => new { Id = (int)s, Name = s.ToString() }),
                    "Id", "Name", model.Status
                );
                return View(model);
            }

            using (var tx = db.Database.BeginTransaction())
            {
                try
                {
                    // 2) Save Guest Photo (optional) and compute final PhotoPath
                    string photoPath = model.GuestPhotoPath; // previous value (may be null)

                    if (model.GuestPhotoFile != null && model.GuestPhotoFile.ContentLength > 0)
                    {
                        var ext = System.IO.Path.GetExtension(model.GuestPhotoFile.FileName);
                        var fileName = Guid.NewGuid().ToString() + ext;

                        var folder = Server.MapPath("~/Uploads/Guests/");
                        System.IO.Directory.CreateDirectory(folder);

                        var fullPath = System.IO.Path.Combine(folder, fileName);
                        model.GuestPhotoFile.SaveAs(fullPath);

                        photoPath = "/Uploads/Guests/" + fileName;
                    }

                    // 3) Update Guest (SP)
                    await db.Database.ExecuteSqlCommandAsync(
                        "EXEC spGuest_Update @p0,@p1,@p2,@p3,@p4,@p5",
                        model.GuestId.Value,
                        model.GuestFullName,
                        model.GuestPhone,
                        model.GuestEmail,
                        model.GuestAddress,
                        photoPath
                    );

                    // 4) Update Booking (SP)
                    await db.Database.ExecuteSqlCommandAsync(
                        "EXEC spBooking_Update @p0,@p1,@p2,@p3,@p4,@p5",
                        model.BookingId,
                        model.GuestId.Value,
                        model.CheckInDate.Date,
                        model.CheckOutDate.Date,
                        model.Notes,
                        model.Status
                    );

                    // 5) Replace Details
                    await db.Database.ExecuteSqlCommandAsync(
                        "EXEC spBookedRoom_DeleteByBookingId @p0",
                        model.BookingId
                    );

                    foreach (var r in model.Rooms)
                    {
                        if (r.RoomId <= 0) continue;
                        if (r.Nights <= 0) r.Nights = 1;

                        await db.Database.ExecuteSqlCommandAsync(
                            "EXEC spBookedRoom_Insert @p0,@p1,@p2,@p3",
                            model.BookingId,
                            r.RoomId,
                            r.PricePerNight,
                            r.Nights
                        );
                    }

                    tx.Commit();
                    return RedirectToAction("Index");
                }
                catch
                {
                    tx.Rollback();
                    ModelState.AddModelError("", "Update failed. Transaction rolled back.");

                    // reload dropdowns
                    ViewBag.Rooms = new SelectList(await db.Rooms.Where(r => r.IsActive).ToListAsync(), "RoomId", "RoomNo");
                    ViewBag.Status = new SelectList(
                        Enum.GetValues(typeof(BookingStatus)).Cast<BookingStatus>()
                            .Select(s => new { Id = (int)s, Name = s.ToString() }),
                        "Id", "Name", model.Status
                    );

                    return View(model);
                }
            }
        }

        // GET: Bookings/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var booking = await db.Bookings
                .Include(b => b.Guest)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return HttpNotFound();

            var details = await db.BookedRooms
                .Include(d => d.Room)
                .Where(d => d.BookingId == id)
                .ToListAsync();

            ViewBag.Details = details;
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var tx = db.Database.BeginTransaction())
            {
                try
                {
                    // 1) Delete details first
                    await db.Database.ExecuteSqlCommandAsync(
                        "EXEC spBookedRoom_DeleteByBookingId @p0",
                        id
                    );

                    // 2) Delete master booking
                    await db.Database.ExecuteSqlCommandAsync(
                        "EXEC spBooking_Delete @p0",
                        id
                    );

                    tx.Commit();
                    return RedirectToAction("Index");
                }
                catch
                {
                    tx.Rollback();
                    TempData["Err"] = "Delete failed. Transaction rolled back.";
                    return RedirectToAction("Index");
                }
            }
        }

    }
}

