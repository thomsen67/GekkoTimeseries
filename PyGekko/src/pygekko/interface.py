# Interface to Gekko (C#.NET)

from . import settings, type_checks
import importlib.resources
import time 
import clr  # Python.NET (pythonnet)
with importlib.resources.path("pygekko.native.win-x64", "Gekko.exe") as dll_path: clr.AddReference(str(dll_path))

from Gekko import Python
import threading
from System import Object 
from System.Threading import Thread, ThreadStart, ApartmentState, Monitor 
from System.Windows import Application, Window
_last_thread = None
RUN_LOCK = Object()
python = Python()

def threads(b: bool):
    settings.threads = b

def run(s: str):
    """
    A Gekko statement (or several Gekko statements delimited by semicolon) to be
    executed by Gekko. The statement(s) is provided as a string.
    """
    type_checks.is_string(s)
    if settings.threads:
        # On some Python versions, it seems that the C#.NET windows only get smooth rendering when this is used.
        global _last_thread
        if _last_thread is not None:
            _last_thread.Join()
        thread = Thread(ThreadStart(lambda: python.Run(s)))
        thread.SetApartmentState(ApartmentState.STA)
        thread.Start()
        _last_thread = thread
    else:
        python.Run(s)

def runfile(s: str):
    """
    Run a Gekko file (typically with extension .gcm) containing Gekko statements.
    The file name is provided as a string.
    """
    type_checks.is_string(s)
    python.RunFile(s)

def wait():    
    """
    Used at the end of a .py file to keep Gekko windows open
    """          
    python.Wait()    

def stdout(b: bool):
    """
    Use stdout stream. This works for normal Python execution (also in VS Code), but not
    for Jupyter setups including VS Code interactive window.
    Is False per default. With default value, while Python runs a run() function, PyGekko
    "records"/"remembers" Gekko-output, which is then printed by Python when the run() function
    returns. Drawback: output for a run() command is only printed at the end of the command, but
    Gekko command typically do not run for a long time individually.
    """
    if(b):
        print("Python: stdout(True) = stdout stream (Standard Output) used for continuous Gekko output")
    else:
        print("Python: stdout(False) = Gekko output is 'recorded' for each run() call and printed by Python")
    python.Stdout(b)
