﻿using Mkh.Utils.Annotations;

namespace Mkh.Mod.Admin.Core.Domain.Menu;

public partial class MenuEntity
{
    /// <summary>
    /// 类型名称
    /// </summary>
    [Ignore]
    public string TypeName => Type.ToDescription();

    /// <summary>
    /// 打开方式名称
    /// </summary>
    [Ignore]
    public string OpenTargetName => OpenTarget.ToDescription();
}