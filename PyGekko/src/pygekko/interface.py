# Interface to Gekko (C#.NET)

import time 
import clr  # Python.NET (pythonnet)
clr.AddReference("c:\\Thomas\\Gekko\\GekkoCS\\Gekko\\bin\\x64\\Release\\Gekko.exe")
from Gekko import Python
import threading
from System import Object 
from System.Threading import Thread, ThreadStart, ApartmentState, Monitor 
from System.Windows import Application, Window
_last_thread = None
RUN_LOCK = Object()
python = Python()

def prun(s: str):
    python.Run(s)

def run(s: str):
    """
    Call a Gekko command (or several Gekko commands delimited by semicolon) as a string.
    """
    global _last_thread
    if _last_thread is not None:
        _last_thread.Join()
    thread = Thread(ThreadStart(lambda: prun(s)))
    thread.SetApartmentState(ApartmentState.STA)
    thread.Start()
    _last_thread = thread
    return thread
