using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SecurityModel
{
    [System.ComponentModel.Description("角色")]
    [Table("Roles")]
    public partial class Role
    {
        public Role()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("RoleName", TypeName = "nvarchar(50)", Order = 1)]
        [Required]
        [System.ComponentModel.Description("角色名称")]
        public string RoleName { get; set; }

        [Required]
        [System.ComponentModel.Description("权限管理员")]
        public Admin UpdateAdmin { get; set; }

        [Required]
        [System.ComponentModel.Description("更新时间")]
        public DateTime UpdateDatetime { get; set; }

        [Required]
        [System.ComponentModel.Description("启用")]
        public bool IsEnabled { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        [NotMapped]
        public string IsEnabledInfo
        {
            get
            {
                return this.IsEnabled ? "启用" : "停用";
            }
        }

        [NotMapped]
        public List<dynamic> Permission_TreeView_WebUI { get; set; }

        [NotMapped]
        public List<dynamic> Permission_TreeView_WebUI_Edit { get; set; }

        [NotMapped]
        public List<User> User_DataGrid_WebUI { get; set; }

        [NotMapped]
        public List<User> User_DataGrid_WebUI_Edit { get; set; }

        // For EasyUI
        [NotMapped]
        public bool? IsCheck { get; set; }

        [NotMapped]
        public bool IsSelect { get; set; }







        public static List<Role> CreateDataGridViewEdit(List<Role> selectedList, List<Role> allList)
        {
            List<Role> r = new List<Role>();
            foreach (Role item in allList)
            {
                Role toAdd = new Role()
                {
                    Id = item.Id,
                    RoleName = item.RoleName,
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

        //dynamic getUser2Dynamic()
        //{
        //    dynamic d = new System.Dynamic.ExpandoObject();

        //    d.id = this.Id;
        //    d.text = this.RoleName;
        //    d.iconCls = "icon-remove";
        //    d.parentid = null;
        //    d.children = null;
        //    d.level = 0;

        //    d.isLeaf = true;

        //    return d;
        //}
    }
}
