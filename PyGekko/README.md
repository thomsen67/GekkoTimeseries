# ![Gekko logo](Gekko.png) Gekko Timeseries And Modeling Software
PyGekko is a Python package for Gekko. Gekko is an open-source time-series oriented software package for handling and analyzing timeseries data, and for solving and analyzing large-scale economic models. You may install the PyGekko package with (requires Python 3.11-3.13 and Windows):

    pip install pygekko

Following this, you may for instance use PyGekko like this:

    import pygekko as pg
    t1 = 2021
    t2 = 2023
    pg.run("reset;")
    pg.run(f"time {t1} {t2};")
    pg.run("x1 = 2, 3, 4;")
    pg.run("x2 = 7, 5, 6;")
    pg.run("y = x1 + x2;")
    pg.run("prt x1, x2, x1/y;")

In general, at the moment, you call Gekko through text strings, with the run() function. Output will be shown in the Python terminal, in this case:

                     x1         %             x2         %           x1/y         % 
    2021         2.0000         M         7.0000         M         0.2222         M
    2022         3.0000     50.00         5.0000    -28.57         0.3750     68.75
    2023         4.0000     33.33         6.0000     20.00         0.4000      6.67

## Disclaimers
The PyGekko 0.0.x versions are purely experimental, and use an experimental Gekko 3.3.x version internally. Regarding more professional use, please wait until an official PyGekko 3.3.x version is released, which will indicate that the Gekko interface is considered stable/mature (in the longer run, PyGekko version numbers will follow Gekko version numbers). Such a PyGekko 3.3.x release is expected before the end of 2025.

## Requirements
The Gekko package only works for Python versions 3.11-3.13 under Windows 64-bit (Python 3.11 was released in October 2022). PyGekko uses Python.NET under the hood, which only works for Windows and does not yet support Python 3.14+ (released in October 2025). PyGekko support for Python < 3.11 could be provided if there is sufficient interest. Windows 32-bit is no longer supported
for newer Gekko versions 3.3.x, and hence is not supported for PyGekko either. PyGekko is set up
to use the .NET Framework (not .NET Core) with version at least 4.6.1. If a normal
Gekko version can run on your Windows 64-bit pc, so should PyGekko (because it uses the same
.NET Framework).

## Gekko links
* [Gekko main homepage](http://www.t-t.dk/gekko)
* [GitHub repository](https://github.com/thomsen67/GekkoTimeseries)
* [Gekko help pages](http://t-t.dk/gekko/docs/user-manual/index.html)
* [New features (3.2)](http://t-t.dk/gekko/docs/user-manual/index.html?i_new_features.htm)

## Contributing + source code
See the GitHub repository.

## More info
Since 2009, Gekko is being used by Danish ministeries, banks, interest groups and universities, for the simulation of economic and energy-related models. It is also used to show and analyze GAMS models. The software runs under Windows (.NET), and is licenced under GNU GPL.

Some features:
* Timeseries-oriented software, with flexible databanks. Suitable data handling/wrangling and modeling, etc.
* Timeseries can be dimensional (so-called array-timeseries). Data-tracing remembers how timeseries are constructed (data lineage).
* Annual, quarters, months, weeks, days and undated frequencies supported. Conversions between these + seasonal correction.
* Other variable types like values, dates, strings, lists, maps and matrices, including many functions dealing with these.
* Dynamically loaded and compiled models, including failsafe mode.
* Gauss and Newton solvers, with ordering and feedback logic. Fair-Taylor or Newton-Fair-Taylor solver for forward-looking models. Any number of simultaneous goals/means possible.
* In-built equation browser, with integrated variable list.
* Decomposition of model equations (including GAMS models), showing contributions. Flowgraphs to show this info, too.
* User-defined functions and procedures, can be stored in libraries.
* Graphics by means of embedded gnuplot
* Interface to Python, R, GAMS and others. Reads/writes file formats like xls, xlsx, prn, csv, tsd, gdx, px, arrow-files. Integrated add-in for Excel.
* Tabelling and menu system, outputting in text, html or Excel
* Open databank format, using (Google) protocol buffers for storage.
* Strict and consistent language syntax (via in-built ANTLR parser), with loops, conditionals etc.
* Used by Statistics Denmark, Ministry of Finance, Ministry for Economic Affairs, Danish Economic Councils, the central bank of Denmark, institutions, universities and more.
* Easy installation by means of a all-inclusive Windows installer, or manually by means of a zip-file with gekko.exe etc.
* Open source. No licenses to compilers etc. -- everything used is C#/.NET (+ gnuplot and x12a). So everything is open-source, and therefore free of charge.
