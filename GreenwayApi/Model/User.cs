using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenwayApi.Model;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.Empty;

    [Required]
    [StringLength(100)]
    [Column("username")]
    public string Username { get; set; }

    [Required]
    [Column("password")]
    public string Password { get; set; }

    [Required]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [Column("role")]
    public UserRole Role { get; set; }

    public ICollection<Collect> Collects { get; set; } = [];
}