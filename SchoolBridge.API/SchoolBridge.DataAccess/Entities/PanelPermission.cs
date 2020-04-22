﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolBridge.DataAccess.Entities
{
    public class PanelPermission
    {      
        [ForeignKey("Panel")]
        public int PanelId { get; set; }
        public Panel Panel { get; set; }

        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

        public object Clone()
            => new PanelPermission
            {
                PanelId = PanelId,
                Panel = Panel,
                PermissionId = PermissionId,
                Permission = Permission
            };
    }
}