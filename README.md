# GekkoTimeseries
Gekko Timeseries Software: Timeseries handling, and solving of large-scale economic models.

Gekko is an open-source time-series oriented software package for handling and analyzing timeseries data, and for solving and analyzing large-scale economic models. Since 2009, Gekko is being used by Danish ministeries, banks, interest groups and universities, for the simulation of economic and energy-related models (more). The software runs under Windows (.NET), and is licenced under GNU GPL.

[![Main window](https://github.com/thomsen67/GekkoTimeseries/blob/master/Diverse/main1.png "Main window")](https://github.com/thomsen67/GekkoTimeseries/blob/master/Diverse/main2.png "Main window")

Some features:
* Timeseries-oriented software, with flexible databanks. Very suitable for modeling and data revision programs.
* Annual, quarters, months and undated frequencies supported, more to come. Conversions between these are in-built.
* Dynamically loaded and compiled models, including failsafe mode.
* Gauss and Newton solvers, with ordering and feedback logic. Fair-Taylor or Newton-Fair-Taylor solver for forward-looking models. Any number of simultaneous goals/means possible.
* In-built equation browser, with integrated variable list.
* Decomposition/tracking of changes in model equations.
* Lists, values, dates, strings, etc., including many functions dealing with these.
* Matrix calculations, including construction, addition, multiplication, inversion, etc. etc.
* User-defined functions.
* Strict language syntax (via in-built ANTLR parser), with loops, conditionals etc.
* Graphics by means of embedded gnuplot
* Interface to the free open-souce R package for econometrics, data mining etc.
* Seasonal correction (X12A)
* Tabelling and menu system, outputting in text, html or Excel
* Read/write from Excel or other spreadsheets. Formats like tsd, prn, csv supported.
* Used by Statistics Denmark, Ministry of Finance, Ministry for Economic Affairs, Danish Energy Agency, Technical University of Denmark, universities and more.
* Easy installation by means of a all-inclusive Windows installer, or manually by means of a zip-file with gekko.exe etc.
* Open source. No licenses to compilers etc. -- everything used is C#/.NET (+ gnuplot, 7zip and x12a). So everything is open-source, and therefore free of charge.

## More Gekko info:
* [Gekko main homepage](http://www.t-t.dk/gekko)
* [New features (2.0)](http://www.t-t.dk/gekko/doc/user-manual/index.html?i_new_features.htm)
* [Releases on GitHub](https://github.com/thomsen67/GekkoTimeseries/releases)

## Contributing + source code
* [Source code documentation](http://www.t-t.dk/gekko/source_code_documentation.html)
* You may fork and clone the source code from here, or simply download it as a zip file. You may consider working a stable [release](https://github.com/thomsen67/GekkoTimeseries/releases)
* Advice on running the code in Visual Studion 2010 (or later): open the solution file (GekkoCS.sln) with Visual Studio. Make sure that the solution configuration is set to "Debug" and the solution platform is set to "Any CPU". Also make sure that the Gekko project is chosen as "StartUp Project". Next, it should be possible to start the Gekko application under Visual Studio by simply pressing F5. If you change the ANTLR .g files, use the .bat files to transform to C# (this demands Java and ANTLR 3.1.3).
* Please note that the software is open source (GNU GPL). That is, it is free of charge, but the source code cannot be used in commercial applications. The source code can only be used and modified in other open source projects.

