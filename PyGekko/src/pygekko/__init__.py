__version__ = "0.0.2" # always increment with new upload to PyPI

from .interface import run, threads
from . import type_checks

# Regarding helper functions like decomp():
# In C#.NET (Python.cs) there is this method: 
# public static void Decomp(string name, string eq = null, int[] t = null, string op = null),
# so in principle C# could be called directly, bypassing parsing and compiling. In practice,
# Gekko statements often have context like default time period etc, or may expect a Gekko.Series
# as input type. So calling without making a string and calling interface.run() is a bit hard,
# but not impossible for future use.

def decomp(*args, **kwargs):      
    """
    Gekko DECOMP statement
    """  
    if False: 
        interface.python.Decomp(*args, **kwargs)
    else:        
        type_checks.length(args, 1); type_checks.is_string(args[0])
        t = op = from1 = endo = ""
        if 't' in kwargs: 
            tt = kwargs['t']
            type_checks.length(tt, 2)
            t = f"{tt[0]} {tt[1]}"
        if 'op' in kwargs: op = f"{kwargs['op']}"
        if 'from_' in kwargs: from1 = f"from {kwargs['from_']}" # 'from' is not allowed as kwarg
        if 'endo' in kwargs: endo = f"endo {kwargs['from']}"        
        s = f"decomp <{t} {op}> {args[0]} {from1} {endo};"
        interface.run(s)

def plot(*args, **kwargs):    
    """
    Gekko PLOT statement
    """      
    if False: 
        interface.python.Decomp(*args, **kwargs)
    else:        
        type_checks.positional_args(args, 1)        
        type_checks.is_string(args[0])
        op = kwargs['op']
        t = kwargs['t']        
        eq = kwargs['eq']
        s = f"decomp <{t[0]} {t[1]} {op}> {args[0]} from {eq};"
        print(s)
        interface.run(s)









