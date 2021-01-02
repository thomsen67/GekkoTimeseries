/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2016, Thomas Thomsen, T-T Analyse.

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
using System.Threading;


namespace Gekko
{
	/// <summary>
	/// Class emulates long process which runs in worker thread
	/// and makes synchronous user UI operations.
	/// </summary>
	public class LongProcess
	{
		#region Members

		// Main thread sets this event to stop worker thread:
		ManualResetEvent eventStop2;

		// Worker thread sets this event when it is stopped:
		ManualResetEvent eventStopped2;

		// Reference to main form used to make syncronous user interface calls:
		public Gui gekkoGui;

		#endregion

		#region Functions

		public LongProcess(ManualResetEvent eventStop,
			               ManualResetEvent eventStopped,
			               Gui form)
		{
			eventStop2 = eventStop;
			eventStopped2 = eventStopped;
			gekkoGui = form;
            Globals.workerThread = this;
		}

		// Function runs in worker thread and emulates long process.
        public void Run(P p)
        {           
            Program.RunCommandCalledFromGUI(gekkoGui.threadInput, p);

            // check if thread is cancelled (do this inside loops in e.g. "sim" etc.)
            if (eventStop2.WaitOne(0, true))
            {
                // clean-up operations may be placed here
                // ...
                // inform main thread that this thread stopped
                eventStopped2.Set();
                return;
            }

            // Make asynchronous call to main form
            // to inform it that thread finished
            //if (!Globals.applicationIsInProcessOfAborting)
            {
                gekkoGui.Invoke(gekkoGui.threadDelegateThreadFinished, null);
            }
        }

		#endregion
	}
}
