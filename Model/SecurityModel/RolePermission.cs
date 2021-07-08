using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SecurityModel
{
    [System.ComponentModel.Description("角色权限关系表")]
    [Table("RolePermissions")]
    public partial class RolePermission
    {

        [Key] // 采用 Fluent api 设置符合主键
        [Column(name: "RoleId", Order = 0)]
        [ForeignKey("Role")]
        [System.ComponentModel.Description("角色")]
        public Guid RoleId { get; set; }        
        public virtual Role Role { get; set; }


        [Key] // 采用 Fluent api 设置符合主键
        [Column(name: "PermissionId", Order = 1)]
        [ForeignKey("Permission")]
        [System.ComponentModel.Description("权限")]
        public Guid PermissionId { get; set; }
        public virtual Permission Permission { get; set; }


        [Column(name: "UpdateAdminId", Order = 2)]
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
