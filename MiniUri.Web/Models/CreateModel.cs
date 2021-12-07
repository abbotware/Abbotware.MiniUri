namespace MiniUri.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CreateModel
    {
        [Required]
        public string? Url { get; set; }

        public string? DesiredKey { get; set; }
    }
}
