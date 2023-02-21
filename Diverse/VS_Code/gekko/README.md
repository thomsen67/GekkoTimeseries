# Gekko README

This is a [Visual Studio Code](https://code.visualstudio.com) extension for Gekko ([Gekko Timeseries and Modeling Software](https://www.t-t.dk/gekko)).

<table>
<tr>
<td><img src="https://www.t-t.dk/gekko/extensions/vscode/images/Gekko32x32.png" width = "32"></td>
<td><img src="https://www.t-t.dk/gekko/extensions/vscode/images/highlight.png" width = "350"></td>
</tr>
</table>

You may consider using the above 'Dark' color theme with blue keywords/symbols. Use Ctrl+K Ctrl+T or 'File' --> 'Preferences' --> 'Theme' --> 'Color Theme' to change color theme (default VS Code theme is 'Dark+').

## Features

Right now the Gekko extension provides syntax highlighting for Gekko .gcm command files and Gekko .frm model files, but more capabilities may evolve later on (like autocomplete or being able to run Gekko commands directly from VS Code). The extension is developed with Gekko 3.x in mind, but should work ok for Gekko 2.x, too.

## Known Issues

Keywords like PLOT etc. are marked, but not intelligently enough (should only be marked if they are the first token in a line, or if they follow a semicolon). For .frm files, the extension is not too happy about using '()' to designate comments (please use '//' instead), especially if these ()-comments contain single quotes...

## Restricted environment

If you need to use the extension on a computer that cannot or may not download extensions from the Internet, you may install the extension manually as a VSIX file. On another (connected) computer, go to [Gekko on Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=t-t-analyse.gekko), scroll down and click 'Download Extension'. Transfer this VSIX file (for instance ``t-t-analyse.gekko-1.1.2.vsix``) to the non-connected computer, and install the extension manually in VS Code under Extensions (Ctrl+Shift+X) --> click three dots --> choose 'Install from .VSIX.

## Other text editors

For Sublime Text, see [this](https://github.com/MartinBonde/gekko_sublime) extension for Gekko.

---

## Release Notes

There are the following releases of the Gekko extension:

### 1.1.2

Minor changes on README page.

### 1.1.1

Included the sigils '%' and '#' in the symbol coloring.

### 1.1.0

Some polishing, and allowing to run under older VS Code versions (from 2021 and on).

### 1.0.0

Initial release of the Gekko syntax highlighter.