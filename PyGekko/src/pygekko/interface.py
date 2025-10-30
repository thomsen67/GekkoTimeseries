# Interface to Gekko (C#.NET)

from . import settings, type_checks
import importlib.resources
import time 
import clr  # Python.NET (pythonnet)
with importlib.resources.path("pygekko.native.win-x64", "Gekko.exe") as dll_path: clr.AddReference(str(dll_path))

from Gekko import Python
from System.Threading import Thread, ThreadStart, ApartmentState

_last_thread = None
pygekko = Python()

def run(s: str):
    """
    A Gekko statement (or several Gekko statements delimited by semicolon) to be
    executed by Gekko. The statement(s) is provided as a string.
    """
    type_checks.is_string(s)
    output: string = None
    if settings.threads:
        # On some Python versions, it seems that the C#.NET windows only get smooth rendering when this is used.
        # FIX this so it can handle string output from Gekko like in the else: statement
        # Probably as a hack where the .Run() argument is a list with two string args... to get side effects, and then assign to output string
        global _last_thread
        if _last_thread is not None:
            _last_thread.Join()
        thread = Thread(ThreadStart(lambda: pygekko.Run(s)))
        thread.SetApartmentState(ApartmentState.STA)
        thread.Start()
        _last_thread = thread
    else:
        output = pygekko.Run(s)
    if (output is not None):
        print(output, end="")    

def runfile(s: str):
    """
    Run a Gekko file (typically with extension .gcm) containing Gekko statements.
    The file name is provided as a string.
    """
    type_checks.is_string(s)
    pygekko.RunFile(s)

def wait():    
    """
    Used at the end of a .py file to keep Gekko popup windows open
    """          
    pygekko.Wait()    

def stdout(b: bool):
    """
    Use stdout stream (default: False). This works for normal Python execution for instance in VS Code, and 
    also for Jupyter setups including VS Code interactive window (REPL coding).
    With False, while Python runs a run() function, PyGekko
    "records"/"remembers" Gekko-output, which is then printed by Python at the end when the run() function
    returns. Drawback: output for a run() command is only printed at the end of the command, but
    Gekko commands typically do not run for a long time individually. Use argument True to get
    continuous output, which will probably not work for Jupyter or VS Code interactive windows.
    """
    if(b):
        print("Python: stdout(True) --> stdout stream ('standard output') used for continuous Gekko output")
    else:
        print("Python: stdout(False) --> Gekko output is 'recorded' for each run() call and printed by Python")
    pygekko.Stdout(b)

def threads(b: bool):
    """
    Use threads (default: False) to call run() on a new thread. If Gekko popup windows like plot, decomp, etc. are
    laggy, you may try this option.
    """
    settings.threads = b
