using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Tracker
{
    class GridData
    {
        private List<GridRow> mList;

        public GridData()
        {
            mList = new List<GridRow>();
        }

        internal void Add(string[] data)
        {
            GridRow gridRow = new GridRow(data);
            mList.Add(gridRow);
        }

        internal GridRow Item(int index)
        {
            return mList[index];
        }

        public int Count
        {
            get
            {
                return mList.Count;
            }
        }

        internal GridRow Last()
        {
            return mList.Last();
        }

        internal int TimeElapsedAt(int i)
        {
            if (i < mList.Count - 1)
                return mList[i + 1].TimeInMinutes() - mList[i].TimeInMinutes();
            else
                return 0;
        }
    }
}
