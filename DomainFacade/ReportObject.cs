using System.Collections.Generic;

namespace DianPing.BA.ReportCenter.Domain.DomainFacade
{
    /// <summary>
    ///   报表对象
    /// </summary>
    /// <typeparam name="TX"> </typeparam>
    public class ReportObject<TX>
    {
        public IEnumerable<TX> XAxis { get; set; }
        public List<SingleReport> YAxisL { get; set; }
        public List<SingleReport> YAxisR { get; set; }
    }
}