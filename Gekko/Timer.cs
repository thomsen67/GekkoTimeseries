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
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Gekko
{    
    public class Timer
    {
        public Hashtable data;
        public Timer() {
            data = new Hashtable();
        }
        public void Start(string name)
        {
            TimerHelper th = null;
            if (data.Contains(name))
            {
                th = (TimerHelper)data[name];
                if (th.running)
                {                    
                    return;
                }                
            }
            else
            {
                th = new TimerHelper();                
                th.millisUntiNow = TimeSpan.Zero;
                data.Add(name, th);
            }
            th.start = System.DateTime.Now;
            th.running = true;            
        }

        public void Stop(string name)
        {
            TimerHelper th = null;
            if (data.Contains(name))
            {
                th = (TimerHelper)data[name];
                if (th.running)
                {
                    TimeSpan more = System.DateTime.Now - th.start;
                    th.millisUntiNow += more;
                }
                else
                {                    
                    return;
                }
            }
            else
            {                
                return;
            }            
            th.running = false;
        }


    }
    public class TimerHelper
    {
        public DateTime start;
        public TimeSpan millisUntiNow;
        public bool running = false;
    }
}