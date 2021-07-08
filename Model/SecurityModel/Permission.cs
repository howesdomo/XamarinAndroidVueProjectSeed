using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityModel
{
    [System.ComponentModel.Description("权限")]
    [Table("Permissions")]
    public partial class Permission
    {
        public Permission()
        {
            InverseParent = new HashSet<Permission>();
            RolePermissions = new HashSet<RolePermission>();
        }

        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("ParentId")]
        [System.ComponentModel.Description("父权限")]
        public Guid? ParentId { get; set; }
        
        #region ParentId

        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(Permission.InverseParent))]
        public virtual Permission Parent { get; set; }

        [InverseProperty(nameof(Permission.Parent))]
        public virtual ICollection<Permission> InverseParent { get; set; }

        #endregion

        [Column("PermissionName", TypeName = "nvarchar(200)")]
        [Required]
        [System.ComponentModel.Description("权限名称")]
        public string PermissionName { get; set; }

        [Column("SysCode", TypeName = "varchar(50)")]
        [Required]
        [System.ComponentModel.Description("代号")]
        // [Unique] 未找到唯一标签 // 将唯一性写在 Fluent API 中
        public string SysCode { get; set; }

        [Required]
        [System.ComponentModel.Description("排序")]
        public int Seq { get; set; }

        [Column("ClassName", TypeName = "varchar(500)")]
        [System.ComponentModel.Description("类名(Form类名 或 网页地址)")]
        public string ClassName { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
