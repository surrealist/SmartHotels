using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreatFriends.SmartHoltel.Models
{
  public class RoomType
  {

    [Key]
    [Required]
    public string Code { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Range(0, 99_999)]
    //[Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }


    // Navigation Propertys
    public virtual ICollection<Room> Rooms { get; set; } = new HashSet<Room>();

  }
}