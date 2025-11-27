using System.ComponentModel.DataAnnotations;

namespace Eventa.DataAccess.Entities
{
    public class Organizer
    {
        [Key] // EF Core هيعرف إن ده المفتاح الأساسي
        public int OrganizerID { get; set; }

        // لا حاجة لإضافة أي خصائص أو علاقات أخرى إذا لم تكوني مسؤولة عن الـ implementation بتاعها
    }
}