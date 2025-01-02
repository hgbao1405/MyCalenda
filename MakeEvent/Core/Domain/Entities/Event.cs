using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeEvent.Core.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc")]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
        public string Color { get; set; } = "Red";
        public bool IsCompleted { get; set; } = false;
        public string? CheckDays { get; set; }
        public string LoopType { get; set; } = "None";
    }
}
