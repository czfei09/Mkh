﻿using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Mod.Admin.Core.Domain.Role
{
    /// <summary>
    /// 角色
    /// </summary>
    [Table("Role")]
    public class RoleEntity : EntityBaseSoftDelete
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码(唯一)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Length(300)]
        public string Remarks { get; set; }

        /// <summary>
        /// 锁定的
        /// </summary>
        public bool Locked { get; set; }
    }
}