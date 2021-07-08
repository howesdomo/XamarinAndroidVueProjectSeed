using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SecurityModel
{
    [System.ComponentModel.Description("用户主数据")]
    [Table("Users")]
    public partial class User
    {
        public User()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", Order = 0)]
        public Guid Id { get; set; }

        [Column("LoginAccount", TypeName = "varchar(200)", Order = 1)]
        [Required]
        [System.ComponentModel.Description("用户登录账号")]
        public string LoginAccount { get; set; }

        [Column("Password", TypeName = "varchar(200)", Order = 2)]
        [Required]
        [System.ComponentModel.Description("密码")]
        public string Password { get; set; }

        [Column("UserName", TypeName = "nvarchar(200)", Order = 3)]
        [Required]
        [System.ComponentModel.Description("用户姓名")]
        public string UserName { get; set; }

        [Column("UpdateAdminId", Order = 4)]
        [Required]
        [System.ComponentModel.Description("权限管理员")]
        public Guid UpdateAdminId { get; set; }
        public virtual Admin UpdateAdmin { get; set; }

        [Required]
        [System.ComponentModel.Description("更新时间")]
        public DateTime UpdateDatetime { get; set; }

        [Required]
        [System.ComponentModel.Description("启用")]
        public bool IsEnabled { get; set; }

        [NotMapped]
        public string IsEnabledInfo
        {
            get
            {
                return this.IsEnabled ? "启用" : "停用";
            }
        }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        [NotMapped]
        public List<Role> RoleList { get; set; }

        /// <summary>
        /// Web UI 使用
        /// </summary>
        [NotMapped]
        public string RoleListInfo { get; set; }

        [NotMapped]
        public List<Role> Role_DataGrid_WebUI { get; set; }

        [NotMapped]
        public List<Role> Role_DataGrid_WebUI_Edit { get; set; }

        // For EasyUI
        [NotMapped]
        public bool? IsCheck { get; set; }

        [NotMapped]
        public bool IsSelect { get; set; }

        // For Login Get User Permission
        [NotMapped]
        public List<Permission> PermissionList { get; set; }




        public static List<User> Create_WebView_DataGrid_Edit(List<User> selectedList, List<User> allList)
        {
            List<User> r = new List<User>();
            foreach (User item in allList)
            {
                var toAdd = new User()
                {
                    Id = item.Id,
                    LoginAccount = item.LoginAccount,
                    UserName = item.UserName,
                    IsEnabled = item.IsEnabled
                };

                if (selectedList.Any(j => j.Id == item.Id))
                {
                    toAdd.IsCheck = true;
                }

                r.Add(toAdd);
            }
            return r;
        }

    }
}
