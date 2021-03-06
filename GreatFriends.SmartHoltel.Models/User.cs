using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GreatFriends.SmartHoltel.Models
{
  public class User
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required]
    [StringLength(256)]
    public string UserName { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Note { get; set; }
  }
}
