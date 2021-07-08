using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SecurityModel
{
    [System.ComponentModel.Description("权限管理员主数据")]
    [Table("Admins")]
    public partial class Admin
    {
        public Admin()
        {

        }

        [Key]
        [Column("Id", Order = 0)]
        public Guid Id { get; set; }

        [Column("IsSuperAdmin", Order = 1)]
        [Required]
        [System.ComponentModel.Description("是超级管理员")]
        public bool IsSuperAdmin { get; set; }

        [NotMapped]
        public string IsSuperAdminInfo
        {
            get
            {
                return this.IsSuperAdmin ? "是" : "否";
            }
        }

        [Column("LoginAccount", TypeName = "varchar(200)", Order = 2)]
        [Required]
        [System.ComponentModel.Description("管理员登录账号")]
        public string LoginAccount { get; set; }

        [Column("Password", TypeName = "varchar(200)", Order = 3)]
        [Required]
        [System.ComponentModel.Description("权限登录密码")]
        public string Password { get; set; }

        [Column("UserName", TypeName = "nvarchar(200)", Order = 4)]
        [Required]
        [System.ComponentModel.Description("管理员姓名")]
        public string UserName { get; set; }

        #region ParentId

        [Column("ParentId", Order = 5)]
        [ForeignKey("Admin")]
        [System.ComponentModel.Description("上级管理员")]
        public Guid? ParentId { get; set; }
        public virtual Admin Parent { get; set; }

        //[NotMapped]
        //[InverseProperty(nameof(Admin.InverseParent))]
        //public virtual Admin Parent { get; set; }

        //[NotMapped]
        //[InverseProperty(nameof(Admin.Parent))]
        //public virtual ICollection<Admin> InverseParent { get; set; }

        #endregion

        [Column("UpdateDatetime", Order = 6)]
        [Required]
        [System.ComponentModel.Description("更新时间")]
        public DateTime UpdateDatetime { get; set; }

        [Column("IsEnabled", Order = 7)]
        [Required]
        [System.ComponentModel.Description("是启用状态")]
        public bool IsEnabled { get; set; }

        [NotMapped]
        public string IsEnabledInfo
        {
            get
            {
                return this.IsEnabled ? "启用" : "停用";
            }
        }

        [NotMapped]
        public bool? IsCheck { get; set; }

        [NotMapped]
        public bool IsSelect { get; set; }
    }
}
