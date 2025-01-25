using Redbox.HAL.Component.Model;
using Redbox.HAL.Component.Model.Extensions;
using System;
using System.Collections.Generic;


namespace Redbox.HAL.Client.Executors
{
    internal sealed class HWCorrectionStat : IHardwareCorrectionStatistic
    {
        public HardwareCorrectionStatistic Statistic { get; internal set; }

        public string ProgramName { get; internal set; }

        public bool CorrectionOk { get; internal set; }

        public DateTime CorrectionTime { get; internal set; }

        internal static void From(HardwareJob job, List<IHardwareCorrectionStatistic> stats)
        {
            Stack<string> stack;
            if (!job.GetStack(out stack).Success)
                return;
            int num = int.Parse(stack.Pop());
            for (int index = 0; index < num; ++index)
            {
                HWCorrectionStat hwCorrectionStat = new HWCorrectionStat()
                {
                    Statistic = Enum<HardwareCorrectionStatistic>.ParseIgnoringCase(stack.Pop(), HardwareCorrectionStatistic.None),
                    CorrectionOk = stack.Pop().Equals("SUCCESS", StringComparison.CurrentCultureIgnoreCase),
                    ProgramName = stack.Pop(),
                    CorrectionTime = DateTime.Parse(stack.Pop())
                };
                stats.Add((IHardwareCorrectionStatistic)hwCorrectionStat);
            }
        }

        private HWCorrectionStat()
        {
        }
    }
}
