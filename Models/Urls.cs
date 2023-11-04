using TestTask.Models;

namespace TestTask.Models
{
    public class Urls
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        internal static bool Any(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
