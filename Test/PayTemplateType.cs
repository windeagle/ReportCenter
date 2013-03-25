using System.ComponentModel;

namespace DianPing.BA.ReportCenter.Domain.Test
{
    /// <summary>
    ///   支付模板类型
    /// </summary>
    public enum PayTemplateType
    {
        /// <summary>
        ///   第六套模板V6
        /// </summary>
        [Description("按月多次支付")] TemplateV6 = 6,

        /// <summary>
        ///   保底支付（保金额）
        /// </summary>
        [Description("保底支付(保金额)")] TemplateV7 = 8,

        /// <summary>
        ///   新电商模板
        /// </summary>
        [Description("实物配送支付（二）")] TemplateV8 = 9,

        /// <summary>
        ///   其他(文字模版)
        /// </summary>
        [Description("其他(文字模版)")] Other = 7,

        /// <summary>
        ///   标准月结
        /// </summary>
        [Description("按月支付")] StandardMonth = 1,

        /// <summary>
        ///   保底支付(保券数)
        /// </summary>
        [Description("保底支付(保券数)")] Gurantee = 2,

        /// <summary>
        ///   固定比例支付
        /// </summary>
        [Description("固定比例支付")] FixedPercent = 3,

        /// <summary>
        ///   电商及实物配送支付
        /// </summary>
        [Description("实物配送支付（一）")] EBusiness = 4,

        ///// <summary>
        ////  第五套模板V5
        ///// </summary>
        //[Description("模板V5")]
        //TemplateV5 = 5,

        /// <summary>
        ///   非模板
        /// </summary>
        [Description("非模板支付")] NonTemplate = 0
    }
}