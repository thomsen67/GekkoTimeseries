[🏠 Home](../../README.md)
# Download development versions

On this page, you will find development versions, i.e. versions with an uneven number after the first period (e.g. 3.3.x).

The Gekko installers are very easy to use on Windows with these three steps: (1) close any open Gekko’s, (2) uninstall Gekko, and (3) download and install InstallerForGekko.msi. In step (3), you may have to skip a Windows warning. Regarding installation of the **Gekcel** add-in for Excel, read more [here](https://t-t.dk/gekko/docs/user-manual/index.html?gekcel_guided_tour.htm). Regarding installation of the **PyGekko** package for Python, read more [here](https://t-t.dk/gekko/docs/user-manual/index.html?pygekko_basics.htm). Regarding text editors, you may get syntax coloring in VS Code or Cursor: just search for “Gekko” under extensions.

When downloading files from the links below, please check the digital signatures afterwards.

---

**Version 3.3.4 — 8/8 2026**

- **NOTE**: This version uses a new internal representation of equation and variable names, affecting Gekko models, GAMS scalar models, DECOMP, etc. Also, a newer compiler for gcm and model files is used in this version. Any potential glitches regarding these improvements will be addressed in Gekko 3.3.5.
- **Gekko**: [installer](https://t-t.dk/gekko/downloads/3_3_4/64/InstallerForGekko.msi) (64-bit) [[signature](signatures.md)]. Please close Gekko before installation!
- **Gekko**: [setup](https://t-t.dk/gekko/downloads/3_3_4/64/Setup.exe) (64-bit) [[signature](signatures.md)]. Only if you have problems with the installer file.
- **Gekko**: [zip-file](https://t-t.dk/gekko/downloads/3_3_4/64/Gekko.zip) (64-bit) [[signature](signatures.md)]. For manual installation — experts only!
- **Gekcel**: [zip-file](https://t-t.dk/gekko/downloads/3_3_4/Gekcel/64/Gekcel.zip) (64-bit) [[signature](signatures.md)]. Excel add-in. Gekcel is a self-contained product bundle, containing all necessary Gekko files inside.
- **PyGekko**: see [PyPI](https://pypi.org/project/pygekko/), where the version numbers correspond. PyGekko is a self-contained Gekko package for Python, containing all necessary Gekko files inside.
- **System requirements**: Both Gekko, Gekcel and PyGekko require Microsoft .NET Framework 4.6.1 or later. Gekcel requires Microsoft Excel 2007 or later, too. Gekko 32-bit is not offered for Gekko 3.3.x.
- [Change-log](changelog.md) (detailed list of changes).
- **New features:** Scaling of PRT/PLOT/SHEET variables with &lt;scale=...&gt;, also interactively in the PLOT window. FINDMISSINGDATA with &lt;ref&gt; option. New eval() and growth() functions, and enhanced time() function. Hash code for the data part of .gbk files is in progress. Bugfixes.

<br>

---

**Version 3.3.3 — 1/6 2026**

Older versions to come here, too...!

---