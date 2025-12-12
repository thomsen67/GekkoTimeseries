# Functions that are directly callable

__version__ = "3.3.2.post1" # always increment with new upload to PyPI

from . import type_checks
from .interface import run, threads, wait, stdout

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
        # A possible direct call...
        interface.pygekko.Decomp(*args, **kwargs)
    else:        
        # Using a string call
        type_checks.length(args, 1); type_checks.is_string(args[0])
        t = op = from1 = endo = ""
        for key, value in kwargs.items():        
            if key == 't':
                tt = kwargs['t']
                type_checks.length(tt, 2)
                t = f"{tt[0]} {tt[1]}"
            elif key == 'op': 
                op = f"{kwargs['op']}"
            elif key == 'from': 
                raise SyntaxError("Use 'from_=...' instead of 'from=...'")
            elif key == 'from_': 
                from1 = f"from {strings_to_string(kwargs['from_'])}" # 'from' is not allowed as kwarg
            elif key == 'endo': 
                endo = f"endo {strings_to_string(kwargs['endo'])}"
            else:
                raise SyntaxError(f"Keyword '{key}' not implemented")
        s = f"decomp <{t} {op}> {args[0]} {from1} {endo};"
        interface.run(s)

def plot(*args, **kwargs):    
    """
    Gekko PLOT statement
    """      
    #type_checks.length(args, 1); type_checks.is_list_of_strings(args[0])
    vars = strings_or_values_to_string(args[0])
    t = op = ""
    for key, value in kwargs.items():        
        if key == 't':
            tt = kwargs['t']
            type_checks.length(tt, 2)
            t = f"{tt[0]} {tt[1]}"
        elif key == 'op': 
            op = f"{kwargs['op']}"        
        else:
            raise SyntaxError(f"Keyword '{key}' not implemented")
        s = f"plot <{t} {op}> {vars};"
    interface.run(s)

def strings_or_values_to_string(x):
    """
    Transforms a single value (str, int, float) or a list of these values 
    into a comma-separated string. So "x" --> "x", "[2, "x", 3.5]" --> "2, x, 3.5".
    """    
    if isinstance(x, list):
        result = ""
        for s in x:     
            if not isinstance(s, (str, int, float)): 
                raise TypeError(f"Input must be a string, integer, float, or a list thereof")
        string_list = [str(item) for item in x]
        result = ", ".join(string_list)            
    elif isinstance(x, (str, int, float)):        
        result = str(x)            
    else:        
        raise TypeError(f"Input must be a string, integer, float, or a list thereof")
    return result


def strings_to_string(x):
    """
    Transforms a single string or a list of these
    into a comma-separated string. So "x" --> "x", "["x", "y"]" --> "x, y".
    """    
    if isinstance(x, list):
        result = ""
        for s in x:     
            if not isinstance(s, str): 
                raise TypeError(f"Input must be a string or a list thereof")
        string_list = [str(item) for item in x]
        result = ", ".join(string_list)            
    elif isinstance(x, str):        
        result = str(x)            
    else:        
        raise TypeError(f"Input must be a string or a list thereof")
    return result


    

