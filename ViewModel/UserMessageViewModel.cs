using server.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace server.ViewModel
{
    public class UserMessageViewModel
    {
        [Required]
        public User UserOrigin { get; set; }
        public User UserTarget { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime Moment { get; set; } = DateTime.Now;
    }
}
