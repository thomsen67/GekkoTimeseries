/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2021, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.        
*/

using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Gekko
{

    /// <summary>
    ///     This class is a workaround for a known VS2005 SplitContainer bug.
    ///     The designer will set Panel2MinSize before setting the control Size, 
    ///     and this causes a exception to be thrown.  
    ///     The solution used here is to store the Panel2MinSize and set it in after the size is set.
    ///     Use this code at your own risk, as I make no warrenty of any kind. 
    ///     You are free to use this code however you like.
    /// </summary>
    public class SplitContainerFix : SplitContainer
    {
        public SplitContainerFix()
        {
            panel2MinSize = base.Panel2MinSize;
        }

        public new int Panel2MinSize
        {
            get
            {
                if (isSized)
                    return base.Panel2MinSize;
                return panel2MinSize;
            }
            set
            {
                panel2MinSize = value;
                if (isSized)
                    base.Panel2MinSize = panel2MinSize;
            }
        }

        public new Size Size
        {
            get { return base.Size; }
            set
            {
                base.Size = value;
                if (!isSized)
                {
                    isSized = true;
                    if (panel2MinSize != 0)
                        Panel2MinSize = panel2MinSize;
                }
            }
        }

        private int panel2MinSize;
        private bool isSized = false;
    }


}
