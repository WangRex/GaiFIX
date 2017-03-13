/**
* 命名空间: Apps.Common
*
* 功 能： N/A
* 类 名： ReturnJson
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-3-11 22:36:21 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Common
{
    public class ReturnJson
    {
        public int code { get; set; }
        public string sessionId { get; set; }
        public string success { get; set; }
    }
}
