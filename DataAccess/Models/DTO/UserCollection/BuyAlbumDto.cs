using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.UserCollection
{
    public class BuyAlbumDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Album ID must be a positive integer.")]
        public int AlbumId { get; set; }
    }
}
