# Gekko README

This is a [Visual Studio Code](https://code.visualstudio.com) extension for Gekko ([Gekko Timeseries and Modeling Software](https://www.t-t.dk/gekko)).

<table>
<tr>
<td><img src="https://www.t-t.dk/gekko/extensions/vscode/images/Gekko32x32.png" width = "32"></td>
<td><img src="https://www.t-t.dk/gekko/extensions/vscode/images/highlight.png" width = "400"></td>
</tr>
</table>

You may consider using the 'Dark' instead of the default 'Dark+' color theme. These colors are shown above: use 'File' --> 'Preferences' --> 'Theme' --> 'Color Theme' to change colors (or Ctrl+K Ctrl+T on Windows).

## Features

Right now the Gekko extension provides syntax highlighting for Gekko .gcm command files and Gekko .frm model files, but more capabilities may evolve later on (like being able to issue Gekko commands directly from VS Code).

## Known Issues

Keywords like PLOT etc. are marked, but not intelligently enough (should only be marked if they are
the first token in a line, or if they follow a semicolon).

## Other text editors

For Sublime Text, see [this](https://github.com/MartinBonde/gekko_sublime) extension for Gekko.

## Release Notes

There are the following releases of the Gekko extension:

### 1.1.0

Some polishing, and allowing to run under older VS Code versions (from 2021 and on).

### 1.0.0

Initial release of the Gekko syntax highlighter.