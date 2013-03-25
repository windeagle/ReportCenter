using System.Collections.Generic;
using System.Linq;

namespace DianPing.BA.ReportCenter.Domain.DomainFacade
{
    //��ά�����ݱ�ɶ�άXY���������һά����NAME
    //��ά���ϵ����ݱ�ɶ�άXY���������ά��ֵ����NAME���飬��ʾʱ��/�ָ�, �硰V6ģ��/�Ϻ������������30�콻�׶��
    /// <summary>
    ///   ������
    /// </summary>
    public abstract class SingleReport
    {
        public string Name { set; get; }
        public IEnumerable<object> ObjectData { get; set; }
    }

    //��ά�����ݱ�ɶ�άXY���������һά����NAME
    //��ά���ϵ����ݱ�ɶ�άXY���������ά��ֵ����NAME���飬��ʾʱ��/�ָ�, �硰V6ģ��/�Ϻ������������30�콻�׶��
    /// <summary>
    ///   ������
    /// </summary>
    /// <typeparam name="TY"> </typeparam>
    public class SingleReport<TY> : SingleReport
    {
        public List<TY> Data
        {
            get { return ObjectData.Cast<TY>().ToList(); }
            set { ObjectData = value.Cast<object>(); }
        }
    }
}