using _1291163_MVC_EF_Project.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _1291163_MVC_EF_Project.Controllers
{
    
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rooms
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var rooms = await db.Database
                .SqlQuery<Room>("EXEC spRoom_List")
                .ToListAsync();

            return View(rooms);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.RoomTypes = new SelectList(db.RoomTypes, "RoomTypeId", "TypeName");
            ViewBag.RoomCategories = new SelectList(db.RoomCategories, "RoomCategoryId", "CategoryName");
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Room room)
        {
            if (ModelState.IsValid)
            {
                await db.Database.ExecuteSqlCommandAsync(
                    "EXEC spRoom_Insert @p0,@p1,@p2,@p3,@p4",
                    room.RoomNo,
                    room.Capacity,
                    room.PricePerNight,
                    room.RoomTypeId,
                    room.RoomCategoryId
                );

                return RedirectToAction("Index");
            }

            ViewBag.RoomTypes = new SelectList(db.RoomTypes, "RoomTypeId", "TypeName", room.RoomTypeId);
            ViewBag.RoomCategories = new SelectList(db.RoomCategories, "RoomCategoryId", "CategoryName", room.RoomCategoryId);
            return View(room);
        }
        [HttpGet]
        
        public async Task<JsonResult> GetRoomPrice(int id)
        {
            var room = await db.Rooms.FindAsync(id);
            if (room == null) return Json(new { ok = false }, JsonRequestBehavior.AllowGet);

            return Json(new { ok = true, price = room.PricePerNight }, JsonRequestBehavior.AllowGet);
        }

    }
}
