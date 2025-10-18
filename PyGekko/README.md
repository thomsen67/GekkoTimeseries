# Gekko Timeseries And Modeling Software
PyGekko is a Python package for Gekko. Gekko is for timeseries handling, and solving and analyzing large-scale economic models. The Gekko package only works for Python versions 3.11, 3.12 and 3.13 (because Python.NET only supports these versions).

Gekko is an open-source time-series oriented software package for handling and analyzing timeseries data, and for solving and analyzing large-scale economic models. Since 2009, Gekko is being used by Danish ministeries, banks, interest groups and universities, for the simulation of economic and energy-related models. It is also used to show and analyze GAMS models. The software runs under Windows (.NET), and is licenced under GNU GPL.

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

## More Gekko info:
* [Gekko main homepage](http://www.t-t.dk/gekko)
* [New features (3.2)](http://t-t.dk/gekko/docs/user-manual/index.html?i_new_features.htm)
* [Releases on GitHub](https://github.com/thomsen67/GekkoTimeseries/releases)

## Contributing + source code
* See the GitHub repository.