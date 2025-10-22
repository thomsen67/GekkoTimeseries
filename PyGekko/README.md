# ![](https://raw.githubusercontent.com/thomsen67/GekkoTimeseries/Gekko_3.3.x/PyGekko/Gekko.png) PyGekko: Gekko Timeseries And Modeling
PyGekko is a Python package for Gekko. Gekko is open-source timeseries-oriented software for handling and analyzing timeseries data, and for solving and analyzing large-scale economic models. You may install the PyGekko package with the following (Windows, Python 3.11-3.13):

    pip install pygekko

**BEWARE**: PyGekko is in its very early stages! 

You may for instance use PyGekko like this:

```python
import pygekko as pg
t1 = 2021
t2 = 2023
pg.run("reset;")
pg.run(f"time {t1} {t2};")
pg.run("x1 = 2, 3, 4;")
pg.run("x2 = 7, 5, 6;")
pg.run("y = x1 + x2;")
pg.run("prt x1, x2, x1/y;")
```

In general, at the moment, you call Gekko through text strings containing Gekko statements, with the `run()` function. Printed output will be shown in the Python terminal, in this case:

                     x1         %             x2         %           x1/y         % 
    2021         2.0000         M         7.0000         M         0.2222         M
    2022         3.0000     50.00         5.0000    -28.57         0.3750     68.75
    2023         4.0000     33.33         6.0000     20.00         0.4000      6.67

You may open the Gekko help system with `pg.run("help;")`.

## Looks
The following is how the Gekko stand-alone application looks. PyGekko does not show the main Gekko window, but can show other windows like plot, decomp, flowgraph, data-trace viewer, etc. As mentioned later on, you may have to end your Python program with a `pg.wait()` to make the Gekko windows stay open.

[![Main window](https://raw.githubusercontent.com/thomsen67/GekkoTimeseries/Gekko_3.3.x/Diverse/gekko_windows2.png "Main window")](https://raw.githubusercontent.com/thomsen67/GekkoTimeseries/Gekko_3.3.x/Diverse/gekko_windows1.png "Main window")

## Disclaimers
PyGekko is work in progress and quite immature as a package right now. Still, the string-based interface is expected to be stable and work as-is, also in the longer run.

The PyGekko 3.3.1 version corresponds to the official Gekko 3.3.1 version, and moving forwards, these version numbers will match. Note that 'beta' releases for PyGekko are expected, for instance PyGekko 3.3.2b1, signifying work in progress towards an official PyGekko 3.3.2 version (beta versions are not installed by `pip install` unless the user explicitly asks for it).

## Requirements
The PyGekko package only works for Python versions 3.11-3.13 under Windows 64-bit (Python 3.11 was released in October 2022). PyGekko uses the Python.NET package under the hood, which only works for Windows and does not yet support Python 3.14 (released in October 2025). PyGekko support for Python < 3.11 could be provided if there is sufficient interest. Windows 32-bit is no longer supported
for newer Gekko versions 3.3.x, and hence is not supported for PyGekko either. PyGekko uses the .NET Framework (not .NET Core) with version at least 4.6.1. If a normal
Gekko version can run on your Windows 64-bit pc, so should PyGekko (because it uses the same
.NET Framework). PyGekko is self-contained regarding Gekko: it works completely independently of any Gekko installation.

## Known issues
* It seems there can be some issues regarding scaling of the plot window. 
* Printed output does not seem to be shown in an interactive Jupyter terminal. 
* Loops and conditionals etc. (lines of Gekko code ending with an `end;`) cannot be directly transformed into corresponding lines of `pg.run()` calls. Using *one* glued-together string delimited with `;`'s will work though.
* Talking to dataframes (Pandas/Polars etc.) containing timeseries data can be done through csv files, cf. Gekko's `import` and `export`. Gekko can write Apache Arrow files (`export<arrow>`) for easy consumption by Pandas/Polars, but cannot read arrow files (yet). (Gekko's `python_run` amounts to calling Python from Gekko, and would be bizarre to use in PyGekko).
* When you open up Gekko windows like for instance the plot window (example: `pg.run("plot x1, x2, x1/y;"`), you may put a `pg.wait()` at the end of your Python program to make the window(s) stay open. Otherwise Python may take these windows down when it finishes its job.

## Gekko links
* [Gekko main homepage](http://www.t-t.dk/gekko)
* [GitHub repository](https://github.com/thomsen67/GekkoTimeseries)
* [Gekko help pages](http://t-t.dk/gekko/docs/user-manual/index.html)
* [New features (3.2)](http://t-t.dk/gekko/docs/user-manual/index.html?i_new_features.htm)

## Contributing + source code
See the GitHub repository.

## Roadmap
Among other things, it is expected that the string-based interface will gradually be supplemented with a more proper Python-function interface, for instance supplementing the string-based `pg.run(f"decomp <{t1} {t2} {op}> qBNP from E_qBNP;")` with an in-built `pg.decomp()` function providing a more Pythonic `pg.decomp("qBNP", t=[t1, t2], op=op, from_="E_qBNP")`.

Regarding Python and PyGekko, see much more details in [this roadmap](https://www.t-t.dk/gekko/docs/blueprints/Gekko_Roadmap_2025.pdf).

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