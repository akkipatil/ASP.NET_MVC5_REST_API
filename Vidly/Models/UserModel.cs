using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    //public class User
    //{
    //    public int Id { get; set; }
    //    [Required]
    //    [StringLength(25)]
    //    public string First_Name { get; set; }

    //    [Required]
    //    [StringLength(25)]
    //    public string Last_Name { get; set; }

    //    [Required]
    //    [StringLength(50)]
    //    public string Email_Address { get; set; }

    //    [Required]
    //    [StringLength(20)]
    //    public string Password { get; set; }
    //    public byte Status { get; set; } //Active /Inactive and Locked/Unlocked.
    //    public ICollection<User> ChildUsers { get; set; }
    //}

    [Table("AdminUsers")]
    public class AdminUser
    {
        [Key]
        public int AdminUserId { get; set; }
        [Display(Name = "Admin User Name")]
        public string AdminUserName { get; set; }

        [Display(Name = "Admin Password")]
        public string AdminPassword { get; set; }
    }

    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public AdminUser AdminUser { get; set; }

        [Display(Name = "Under Admin Name")]
        public int AdminUserId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "First Name")]
        public string First_Name { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Email Address")]
        public string Email_Address { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Status")]
        public byte StatusId { get; set; } //1 - Active   2 - Inactive    3 -Locked   4 -Unlocked.
    }

    public class UserStatus
    {
        [Key]
        public int AdminUserId { get; set; }
        [Display(Name = "Admin User Name")]
        public string AdminUserName { get; set; }

        [Display(Name = "Admin Password")]
        public string AdminPassword { get; set; }
    }

    [Table("OperationLog")]
    public class OperationLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime timestamp { get; set; }
        [Required]
        [StringLength(100)]
        public string level { get; set; }
        [Required]
        [StringLength(1000)]
        public string logger { get; set; }
        [Required]
        [StringLength(3600)]
        public string message { get; set; }
        [StringLength(3600)]
        public string exception { get; set; }
        [StringLength(3600)]
        public string stacktrace { get; set; }
    }
}