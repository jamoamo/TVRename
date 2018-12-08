// 
// Main website for TVRename is http://tvrename.com
// 
// Source code available at https://github.com/TV-Rename/tvrename
// 
// This code is released under GPLv3 https://github.com/TV-Rename/tvrename/blob/master/LICENSE.md
// 

using System.Collections.Generic;
using NLog;

namespace TVRename
{
    public abstract class ScanActivity
    {
        protected static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();
        protected readonly TVDoc MDoc;
        private SetProgressDelegate progressDelegate;
        private int startPosition;
        private int endPosition;

        protected ScanActivity(TVDoc doc) => MDoc = doc;

        public void Check(SetProgressDelegate prog, int startpct, int totPct, ICollection<ShowItem> showList,
            TVDoc.ScanSettings settings)
        {
            startPosition = startpct;
            endPosition = totPct;
            progressDelegate = prog;
            Check(prog, showList,settings);
            progressDelegate.Invoke(endPosition , string.Empty);
        }

        protected abstract void Check(SetProgressDelegate prog, ICollection<ShowItem> showList,TVDoc.ScanSettings settings);

        protected void UpdateStatus(int recordNumber,int totalRecords, string message)
        {
            progressDelegate.Invoke(startPosition + ((endPosition - startPosition) * recordNumber / (totalRecords+1)), message);
        }

        public abstract bool Active();

        private void Check(SetProgressDelegate prog, List<ShowItem> showList, TVDoc.ScanSettings settings) =>
            Check(prog, 0, 100, showList, settings);

        public void CheckIfActive(SetProgressDelegate prog, List<ShowItem> showList,TVDoc.ScanSettings settings)
        {
            if (Active())
            {
                 Check(prog, showList, settings);
            }
        }

        internal void CheckIfActive(SetProgressDelegate prog, int startpct, int totPct, List<ShowItem> showList, TVDoc.ScanSettings settings)
        {
            if (Active())
            {
                Check(prog,startpct,totPct, showList, settings);
            }
        }
    }
}