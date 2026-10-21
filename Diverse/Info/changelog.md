[🏠 Home](../../README.md)

# Changelog

On this page you will find detailes regarding changes in different Gekko versions. On the download pages, you will find only brief descriptions of changes, whereas this page goes in more detail. Even more details can be seen in the commits at [GitHub](https://github.com/thomsen67/GekkoTimeseries/commits/master).


## Version 3.3.4 – 8/8 2026

- Compared to Gekko 3.3.3, Gekko 3.3.4 uses a new internal representation of equation and variable names, among other things used for Gekko models and GAMS scalar models. A lot of code regarding modeling, DECOMP etc. has been rewritten, so this represents "heavy surgery", and some glitches are to be expected. Such glitches will be fixed in Gekko 3.3.5. In Gekko 3.3.4, scalar models are loaded a little bit slower after the rewrite, but the rewrite will make it much easier to further develop Gekko regarding for instance DECOMP, flowgraphs, scalar model representations (for instance if time is not the last dimension in equations or variables), html browsers etc.

- Using a newer compiler to dynamically compile .gcm and model files (.NET Roslyn, which produces dll components only in RAM). The old compiler can be activated with "option system code compile ram = no".

- Scaling of PRT/PLOT/SHEET: for these statements, you can now state a separate scaling expression with `<scale=...>`. For the PLOT window, there is now a scaling box for interactive scaling (default value: 1).

- For DECOMP, there is now a real button [Infl.] for "Influences", which was previously a link.

- FINDMISSINGDATA: With new option `<ref>`, only missing values in the Work databank that are also missing values in the Ref databank (or where the series does not exist at all) are flagged/replaced. This can be used to find missing values that are "worse" relative to the Ref databank values.

- New `eval()` function, with string argument, returns a Gekko variable. Use like for instance: `x = 2; y = eval('x1/(x1+1)'); prt y;`. In a lot of cases, you may use `eval('...')` where math expression `...` is expected. There may be some edge cases where this does not work. The feature can be practical if you need to compose expressions as strings, or store expressions as strings (for instance in a list).

- New `time()` function variants, for instance `time(2010q1)`, so that it returns 0 for that particular period. You may also use `time('start')` or `time('end')` for sample endpoints. The function returns a timeseries with value 0 in the designated period.

- New `growth()` function, where the first argument is the growth rate, for instance `growth(0.02)`, which amounts to 2% year, also for higher frequencies (so not 2% per period for these). The function returns a timeseries with value 1 in the designated period. You may also use `growth(0.02, 2020)`, `growth(0.02, 'start')` or `growth(0.02, 'end')`.

- Datahash options for gbk files: `option databank file gbk datahash = yes; true;` `option databank file gbk datahash trace = yes;` `option databank file gbk datahash meta = no;` Datahashes will be more developed in a future Gekko version.

- Bugfix: When copy-pasting from the main upper window, the pasted string no longer gets `#action:...`, `#disp:...`, `#outputtab:...` inserted when there are links.

- Bugfix: Program dependency tracking: now shows a link to get the (potentially long) list of files printed.

- Bugfix: For library functions/procedure, wrong line text was shown for run-time errors. Now no text is shown, to not confuse.

- Bugfix: Gekko could hang/crash when writing a large databank with many traces combined with "option databank trace = no".

- Bugfix: `prt ... file=...` was not ported from Gekko 2.

<br>


## Version 3.3.3 – 1/6 2026

Older versions to come here, too...!
