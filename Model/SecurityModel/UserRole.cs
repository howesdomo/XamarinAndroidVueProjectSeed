using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityModel
{
    [System.ComponentModel.Description("用户角色关系表")]
    [Table("UserRoles")]
    public partial class UserRole
    {
        [Key] // 采用 Fluent api 设置符合主键
        [Column(name: "UserId", Order = 0)]
        [ForeignKey("User")]
        [System.ComponentModel.Description("用户")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }


        [Key] // 采用 Fluent api 设置符合主键
        [Column(name: "RoleId",Order = 1)]
        [ForeignKey("Role")]
        [System.ComponentModel.Description("角色")]
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }


        [Column(name: "UpdateAdminId", Order = 1)]
        [System.ComponentModel.Description("权限管理员")]
        [Required]
        [ForeignKey("Admin")]
        public Guid UpdateAdminId { get; set; }
        public virtual Admin UpdateAdmin { get; set; }


        [System.ComponentModel.Description("更新时间")]
        [Required]
        public DateTime UpdateDatetime { get; set; }

    }
}