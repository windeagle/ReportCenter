using System.ComponentModel;

namespace DianPing.BA.ReportCenter.Domain.Test
{
    /// <summary>
    ///   ֧��ģ������
    /// </summary>
    public enum PayTemplateType
    {
        /// <summary>
        ///   ������ģ��V6
        /// </summary>
        [Description("���¶��֧��")] TemplateV6 = 6,

        /// <summary>
        ///   ����֧��������
        /// </summary>
        [Description("����֧��(�����)")] TemplateV7 = 8,

        /// <summary>
        ///   �µ���ģ��
        /// </summary>
        [Description("ʵ������֧��������")] TemplateV8 = 9,

        /// <summary>
        ///   ����(����ģ��)
        /// </summary>
        [Description("����(����ģ��)")] Other = 7,

        /// <summary>
        ///   ��׼�½�
        /// </summary>
        [Description("����֧��")] StandardMonth = 1,

        /// <summary>
        ///   ����֧��(��ȯ��)
        /// </summary>
        [Description("����֧��(��ȯ��)")] Gurantee = 2,

        /// <summary>
        ///   �̶�����֧��
        /// </summary>
        [Description("�̶�����֧��")] FixedPercent = 3,

        /// <summary>
        ///   ���̼�ʵ������֧��
        /// </summary>
        [Description("ʵ������֧����һ��")] EBusiness = 4,

        ///// <summary>
        ////  ������ģ��V5
        ///// </summary>
        //[Description("ģ��V5")]
        //TemplateV5 = 5,

        /// <summary>
        ///   ��ģ��
        /// </summary>
        [Description("��ģ��֧��")] NonTemplate = 0
    }
}