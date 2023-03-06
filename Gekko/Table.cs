/*
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2021, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Gekko
{
    [Serializable]
    public class Table
    {
        enum ETablePrintType
        {
            Txt,
            Html
        }
        private Dictionary<Coord, Cell> _data = null;  //contains the data
        private Dictionary<int, RowInfo> _rows = null;  //info on the different rows (length etc.)
        private Dictionary<int, ColInfo> _cols = null; //info on the different rows (length etc.)
        public CurrentRow CurRow = null;
        private int _rowMaxNumber = 0;
        private int _colMaxNumber = 0;
        public string tabFileName = null;
        public bool writeOnce = false;  //if true, an exception will be raised if trying to overwrite a cell with a new value (borders not counting)
        public string type = "print";  //"print" or "table"

        public Dictionary<Coord, Cell> GetData()
        {
            return _data;
        }

        public int GetRowMaxNumber()
        {
            return _rowMaxNumber;
        }

        public int GetColMaxNumber()
        {
            return _colMaxNumber;
        }

        public int Count()
        {
            return _data.Count;
        }


        public Table()
        {
            _data = new Dictionary<Coord, Cell>();
            _rows = new Dictionary<int, RowInfo>();
            _cols = new Dictionary<int, ColInfo>();
            CurRow = new CurrentRow();
            CurRow._table = this;
        }

        public Table Transpose() {
            //creates a new transposed table. Not a complete clone, only indices are changed.
            //changing the keys directly as a kind of clone would be a little faster, but we
            //would like to keep the old table intact. Note that changing cells in the transposed table
            //will reflect into the old table and vice versa! (they point to same cell objects).
            Table ttable = new Table();
            ttable.type = this.type;  //cloning this
            foreach (KeyValuePair<Coord, Cell> kvp in this._data)
            {
                Coord c2 = new Coord(kvp.Key.Col, kvp.Key.Row);  //swap these
                ttable.Set(c2, kvp.Value);  //will also get max row/col numbers right
            }
            return ttable;
        }

        /// <summary>
        /// Returns null if not present
        /// </summary>
        public Cell Get(int x, int y)
        {
            Cell cell2 = null;
            Cell cell3 = null;
            if (_data.TryGetValue(new Coord(x, y), out cell2))
            {
                cell3 = cell2;
            }
            return cell3;
        }


        public void Set(int x, int y, string text)
        {
            Set(new Coord(x, y), text);
        }

        public enum TableBorderExtent
        {
            Outer,
            //Inner,
            All
        }

        public void SetBorder(int row1, int col1, int row2, int col2, BorderType type)
        {
            SetBorder(row1, col1, row2, col2, type, null, TableBorderExtent.Outer);
        }

        public void SetBorder(int row1, int col1, int row2, int col2, BorderType type, BorderStyle2 style)
        {
            SetBorder(row1, col1, row2, col2, type, style, TableBorderExtent.Outer);
        }

        public void SetBorder(int row1, int col1, int row2, int col2, BorderType type, BorderStyle2 style, TableBorderExtent extent)
        {
            SetBorderAbstract(row1, col1, row2, col2, type, style, extent, "");
        }

        public void HideBorder(int row1, int col1, int row2, int col2, BorderType type)
        {
            DisableBorder(row1, col1, row2, col2, type, TableBorderExtent.Outer);
        }

        public void DisableBorder(int row1, int col1, int row2, int col2, BorderType type, TableBorderExtent extent)
        {
            SetBorderAbstract(row1, col1, row2, col2, type, null, extent, "disable");
        }

        public void ShowBorder(int row1, int col1, int row2, int col2, BorderType type)
        {
            EnableBorder(row1, col1, row2, col2, type, TableBorderExtent.Outer);
        }

        public void EnableBorder(int row1, int col1, int row2, int col2, BorderType type, TableBorderExtent extent)
        {
            SetBorderAbstract(row1, col1, row2, col2, type, null, extent, "enable");
        }

        //can also disable
        public void SetBorderAbstract(int row1, int col1, int row2, int col2, BorderType type, BorderStyle2 style, TableBorderExtent extent, string enableType)
        {
            //only outer borders, not inner
            if (row1 > row2)
            {
                new Error("Table SetBorder: arg1 should be <= arg3");
            }
            if (col1 > col2)
            {
                new Error("Table SetBorder: arg2 should be <= arg4");
            }

            string color = null;
            if (style != null) color = style.color;

            for (int i = row1; i <= row2; i++)
            {
                if (extent == TableBorderExtent.Outer && type == BorderType.Top && i > row1) continue;
                if (extent == TableBorderExtent.Outer && type == BorderType.Bottom && i < row2) continue;
                for (int j = col1; j <= col2; j++)
                {
                    if (extent == TableBorderExtent.Outer && type == BorderType.Left && j > col1) continue;
                    if (extent == TableBorderExtent.Outer && type == BorderType.Right && j < col2) continue;
                    Cell cell = null;
                    Coord coord = new Coord(i, j);
                    if (!this._data.TryGetValue(new Coord(i, j), out cell))
                    {
                        Set(coord, "");
                        cell = this._data[coord];
                    }
                    switch (type)
                    {
                        case BorderType.Left:
                            {
                                if (enableType == "disable") cell.borderLeftDisable = true;
                                else if (enableType == "enable") cell.borderLeftDisable = false;
                                else
                                {
                                    cell.borderLeft = true;
                                    cell.borderLeftColor = color;
                                }
                            }
                            break;
                        case BorderType.Right:
                            {
                                if (enableType == "disable") cell.borderRightDisable = true;
                                else if (enableType == "enable") cell.borderRightDisable = false;
                                else
                                {
                                    cell.borderRight = true;
                                    cell.borderRightColor = color;
                                }
                            }
                            break;
                        case BorderType.Top:
                            {
                                if (enableType == "disable") cell.borderTopDisable = true;
                                else if (enableType == "enable") cell.borderTopDisable = false;
                                else
                                {
                                    cell.borderTop = true;
                                    cell.borderTopColor = color;
                                }
                            }
                            break;
                        case BorderType.Bottom:
                            {
                                if (enableType == "disble") cell.borderBottomDisable = true;
                                else if (enableType == "enable") cell.borderBottomDisable = false;
                                else
                                {
                                    cell.borderBottom = true;
                                    cell.borderBottomColor = color;
                                }
                            }
                            break;
                    }
                }
            }
        }

        public void SetAlign(int x, int y, Align type)
        {
            this.SetAlign(x, y, x, y, type);
        }

        public void SetAlign(int x1, int y1, int x2, int y2, Align type)
        {
            if (x1 > x2)
            {
                new Error("Table SetAlign: arg1 should be <= arg3");
            }
            if (y1 > y2)
            {
                new Error("Table SetAlign: arg2 should be <= arg4");
            }
            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    Cell cell = null;
                    Coord coord = new Coord(i, j);
                    if (!this._data.TryGetValue(new Coord(i, j), out cell))
                    {
                        Set(coord, "");
                        cell = this._data[coord];
                    }
                    cell.align = (int)type - 1;  //0, 1, 2 converted into -1, 0, 1
                }
            }
        }


        public void SetDates(int row, int col, GekkoTime tStart, GekkoTime tEnd)
        {
            int counter = 0;
            foreach (GekkoTime t in new GekkoTimeIterator( tStart, tEnd))
            {
                if (Program.ShouldFilterPeriod(t)) continue;
                this.SetDate(row, col + counter, t.ToString());
                counter++;
            }
        }

        public void SetDate(int x, int y, string text)
        {
            Set(new Coord(x, y), text, double.NaN, CellType.Date, null);  //TODO, FIX: not working on quarters.......
        }

        public void SetNumber(int x, int y, double number, string format)
        {
            Set(new Coord(x, y), null, number, CellType.Number, format);  //TODO, FIX: not working on quarters.......
        }

        public void Set(int x, int y, string text, CellType type)
        {
            Set(new Coord(x, y), text, double.NaN, type, null);  //TODO, FIX: not working on quarters.......
        }

        public void Set(Coord xy, string text)
        {
            Set(xy, text, double.NaN, CellType.Text, null);
        }

        public void Set(Coord xy, string text, double number, CellType type, string format)
        {
            //if (xy.Row == 2 && xy.Col == 2)
            //{

            //}

            if (writeOnce && _data.ContainsKey(xy))  //slows it down a tiny bit since Dictionary will be queried 2 times, but not much lost
            {
                Cell c = _data[xy];
                string s = "";

                new Error("Tried to override data in position (" + xy.Row + ", " + xy.Col + ") in table object.", false);

                string s2 = GetCellContent(c);
                G.Writeln("           " + s2);

                PrintCellsForDebug();

                throw new GekkoException();
            }
            Cell cell = new Cell();
            cell.cellType = type;
            if (type == CellType.Date)
            {
                cell.date = text;
            }
            else if (type == CellType.Text)
            {
                if (cell.CellText == null) cell.CellText = new Text();
                cell.CellText.TextData = new List<string>();
                cell.CellText.TextData.Add(text);
            }
            else if (type == CellType.Number)
            {
                cell.number = number;
                if (format != null && format != "") cell.numberFormat = format;
                if (G.IsMissingVariableArtificialNumber(cell.number))
                {
                    cell.number = double.NaN;
                    cell.numberShouldShowAsN = true;
                }
            }
            Set(xy, cell);
        }

        private static string GetCellContent(Cell c)
        {
            string s = null;
            if (c.cellType == CellType.Text) s = c.CellText.TextData[0];
            else if (c.cellType == CellType.Date) s = c.date;
            else if (c.cellType == CellType.Number) s = c.number.ToString();
            s = "Type: " + c.cellType.ToString() + " Content: " + "[" + s + "]";
            return s;
        }

        public void Merge(int x1, int y1, int x2, int y2)
        {
            if (x1 != x2)
            {
                new Error("Table merge: arg1 should be = arg3, merge does not work over rows");
            }
            if (y1 > y2)
            {
                //#837433329. maybe revive this error in fall 2013 when table generation has been redone
                //only reason is to allow timefilters that out-filter all periods.
                return;  //since row merge does not work this means that nothing needs to be done
            }

            if (y1 == y2)
            {
                return;  //since row merge does not work this means that nothing needs to be done
            }

            int anchorRow = x1;
            int anchorCol = y1;
            int shipRow = x2;
            int shipCol = y2;

            for (int i = x1; i <= x2; i++)  //will only be for 1 row here, i=x1=x2
            {
                for (int j = y1; j <= y2; j++)
                {
                    Cell cell = null;
                    Coord coord = new Coord(i, j);
                    if (!this._data.TryGetValue(new Coord(i, j), out cell))
                    {
                        Set(coord, "");
                        cell = this._data[coord];
                    }
                    cell.mergeCellAnchorRow = anchorRow;
                    cell.mergeCellAnchorCol = anchorCol;
                    cell.mergeCellShipRow = shipRow;
                    cell.mergeCellShipCol = shipCol;
                }
            }
        }

        public void Set(Coord xy, Cell newCell)
        {
            ColInfo colInfo = null;
            RowInfo rowInfo = null;
            Cell cell = null;
            if (_data.TryGetValue(xy, out cell))
            {
                cell.borderLeft = cell.borderLeft | newCell.borderLeft;
                cell.borderRight = cell.borderRight | newCell.borderRight;
                cell.borderTop = cell.borderTop | newCell.borderTop;
                cell.borderBottom = cell.borderBottom | newCell.borderBottom;
                if (newCell.CellText != null)
                {
                    cell.CellText = newCell.CellText;
                }
                if (newCell.date != null)
                {
                    cell.date = newCell.date;
                }
                cell.number = newCell.number;
                cell.numberFormat = newCell.numberFormat;
                cell.cellType = newCell.cellType;
                cell.align = newCell.align;
                cell.cellType = newCell.cellType;
                rowInfo = _rows[xy.Row];
                colInfo = _cols[xy.Col];
            }
            else
            {
                cell = newCell;
                _data.Add(xy, cell);

                if (_cols.TryGetValue(xy.Col, out colInfo))
                {
                }
                else
                {
                    colInfo = new ColInfo();
                    _cols.Add(xy.Col, colInfo);
                }

                if (_rows.TryGetValue(xy.Row, out rowInfo))
                {
                }
                else
                {
                    rowInfo = new RowInfo();
                    _rows.Add(xy.Row, rowInfo);
                }

                if (xy.Row > _rowMaxNumber) _rowMaxNumber = xy.Row;
                if (xy.Col > _colMaxNumber) _colMaxNumber = xy.Col;                

                if (xy.Row > colInfo.RowMaxNumber) colInfo.RowMaxNumber = xy.Row;
                if (xy.Col > rowInfo.ColMaxNumber) rowInfo.ColMaxNumber = xy.Col;

                //if (cell.borderTop) rowInfo.HasAnyLineTop = true;
                //if (cell.borderBottom) rowInfo.HasAnyLineBottom = true;
            }
            //Console.WriteLine("added " + xy.Row + " " + xy.Col + " = " + newCell.CellText.TextData);
        }

        public void UpdateTable()
        {
            //This methods depends only upon this._data to recreate the other elements
            //This is used when coordinates have changed, e.g. deleting or adding rows/cols, changing coords etc. etc.
            this._rowMaxNumber = 0;
            this._colMaxNumber = 0;
            this._rows = new Dictionary<int, RowInfo>();
            this._cols = new Dictionary<int, ColInfo>();
            List<CoordAndCell> coordAndCells = GetListOfKeyValuePairs(this);
            this._data = new Dictionary<Coord, Cell>();

            foreach (CoordAndCell coordAndCell in coordAndCells)
            {
                //Console.WriteLine(coordAndCell.coord.Row + " ooo " + coordAndCell.coord.Col);

                Cell cell = coordAndCell.cell;
                Coord xy = coordAndCell.coord;
                this._data.Add(xy, cell);  //it is re-entered in order for it to obtain the right hash code etc. in the Dictionary
                ColInfo colInfo = null;
                RowInfo rowInfo = null;
                if (this._rows.TryGetValue(xy.Row, out rowInfo))
                {
                }
                else
                {
                    rowInfo = new RowInfo();
                    _rows.Add(xy.Row, rowInfo);
                }

                if (this._cols.TryGetValue(xy.Col, out colInfo))
                {
                }
                else
                {
                    colInfo = new ColInfo();
                    _cols.Add(xy.Col, colInfo);
                }

                if (xy.Row > _rowMaxNumber) _rowMaxNumber = xy.Row;
                if (xy.Col > _colMaxNumber) _colMaxNumber = xy.Col;                
                if (xy.Row > colInfo.RowMaxNumber) colInfo.RowMaxNumber = xy.Row;
                if (xy.Col > rowInfo.ColMaxNumber) rowInfo.ColMaxNumber = xy.Col;
                //if (cell.borderTop) rowInfo.HasAnyLineTop = true;
                //if (cell.borderBottom) rowInfo.HasAnyLineBottom = true;
            }
        }

        private static string GetStringFromDate(Cell cell)
        {
            string s = cell.date.ToString();
                        
            string[] ss = s.Split('m');
            if (ss.Length == 2 && G.IsInteger(ss[0]) && G.IsInteger(ss[1]))
            {
                int x1 = int.Parse(ss[0]);
                int x2 = int.Parse(ss[1]);
                if (Program.options.table_mdateformat != "")
                {
                    string format = Program.options.table_mdateformat;
                    bool lower = false;
                    if (format.ToLower().EndsWith("-lower"))
                    {
                        format = format.Substring(0, format.Length - "-lower".Length);
                        lower = true;
                    }
                    if (G.Equal(format, "danish-short"))
                    {
                        if (x2 == 1) s = "jan. " + x1;
                        else if (x2 == 2) s = "feb. " + x1;
                        else if (x2 == 3) s = "mar. " + x1;
                        else if (x2 == 4) s = "apr. " + x1;
                        else if (x2 == 5) s = "maj " + x1;
                        else if (x2 == 6) s = "juni " + x1;
                        else if (x2 == 7) s = "juli " + x1;
                        else if (x2 == 8) s = "aug. " + x1;
                        else if (x2 == 9) s = "sep. " + x1;
                        else if (x2 == 10) s = "okt. " + x1;
                        else if (x2 == 11) s = "nov. " + x1;
                        else if (x2 == 12) s = "dec. " + x1;
                        else
                        {
                            new Error("Month number > 12 not allowed");
                            //throw new GekkoException();
                        }
                    }
                    else if (G.Equal(format, "danish-long"))
                    {
                        if (x2 == 1) s = "januar " + x1;
                        else if (x2 == 2) s = "februar " + x1;
                        else if (x2 == 3) s = "marts " + x1;
                        else if (x2 == 4) s = "april " + x1;
                        else if (x2 == 5) s = "maj " + x1;
                        else if (x2 == 6) s = "juni " + x1;
                        else if (x2 == 7) s = "juli " + x1;
                        else if (x2 == 8) s = "august " + x1;
                        else if (x2 == 9) s = "september " + x1;
                        else if (x2 == 10) s = "oktober " + x1;
                        else if (x2 == 11) s = "november " + x1;
                        else if (x2 == 12) s = "december " + x1;
                        else
                        {
                            new Error("Month number > 12 not allowed");
                            //throw new GekkoException();
                        }
                    }
                    else if (G.Equal(format, "english-short"))
                    {
                        if (x2 == 1) s = "jan. " + x1;
                        else if (x2 == 2) s = "feb. " + x1;
                        else if (x2 == 3) s = "mar. " + x1;
                        else if (x2 == 4) s = "apr. " + x1;
                        else if (x2 == 5) s = "may " + x1;
                        else if (x2 == 6) s = "june " + x1;
                        else if (x2 == 7) s = "july " + x1;
                        else if (x2 == 8) s = "aug. " + x1;
                        else if (x2 == 9) s = "sep. " + x1;
                        else if (x2 == 10) s = "oct. " + x1;
                        else if (x2 == 11) s = "nov. " + x1;
                        else if (x2 == 12) s = "dec. " + x1;
                        else
                        {
                            new Error("Month number > 12 not allowed");
                            //throw new GekkoException();
                        }
                    }
                    else if (G.Equal(format, "english-long"))
                    {
                        if (x2 == 1) s = "january " + x1;
                        else if (x2 == 2) s = "february " + x1;
                        else if (x2 == 3) s = "march " + x1;
                        else if (x2 == 4) s = "april " + x1;
                        else if (x2 == 5) s = "may " + x1;
                        else if (x2 == 6) s = "june " + x1;
                        else if (x2 == 7) s = "july " + x1;
                        else if (x2 == 8) s = "august " + x1;
                        else if (x2 == 9) s = "september " + x1;
                        else if (x2 == 10) s = "october " + x1;
                        else if (x2 == 11) s = "november " + x1;
                        else if (x2 == 12) s = "december " + x1;
                        else
                        {
                            new Error("Month number > 12 not allowed");
                            //throw new GekkoException();
                        }
                    }
                    else
                    {
                        new Error("OPTION table mdateformat = '" + Program.options.table_mdateformat + "' is not recognized");
                        //throw new GekkoException();
                    }
                    //Sets uppercase on first letter
                    if (!lower) s = char.ToUpper(s[0]) + s.Substring(1);
                }
            }

            return s;
        }

        private static string GetStringFromNumber(Cell cell, string format, Table table)
        {            
            return G.FormatNumber(cell.number, format, cell.numberShouldShowAsN, G.Equal(table.type, "table"));
        }

        /// <summary>
        /// Actually 'produce' the table
        /// </summary>
        /// <param name="ptype"></param>
        /// <returns></returns>
        public List<string> Print(string ptype)
        {
            ETablePrintType type = ETablePrintType.Txt;
            if (G.Equal(ptype, "html")) type = ETablePrintType.Html;
            if (type == ETablePrintType.Html)
            {
                return PrintHtml();
            }
            else
            {
                return PrintText();
            }
        }

        /// <summary>
        /// Actually 'produce' the table (default as txt)
        /// </summary>
        /// <returns></returns>
        public List<string> Print()
        {
            //TODO: it is BAD that this as a sideeffect pumps up the table object coordinates
            //do not use this method directly: use PrintTable() instead (it uses cloning to avoid
            //this problem). Or if using this method, be sure that the table object is not used
            //later on for other purposes.
            return PrintText();
        }

        public string GetCellTitleForHtml(double d, bool asN)
        {
            string s = d.ToString();
            if (double.IsNaN(d) && asN)
            {
                s = "Non-existing variable";  //non-existing
            }
            else if (double.IsNaN(d))
            {
                s = "Missing value";  //missing
            }
            return s;
        }

        public List<string> PrintHtml()
        {
            string width = "style=\"width:" + Program.options.table_html_datawidth.ToString() + "em\"";
            List<string> lines = new List<string>();
            
            //NOTE: all the css styles are located another place: search on "td.ws" for example
                        
            lines.Add("<table CLASS=\"gfsize gfont\" cellpadding=0 cellspacing=0>");
            for (int j = 1; j <= this._colMaxNumber; j++)
            {                
            }

            bool blue = true;
            string sblue = null;

            int firstNumber = int.MaxValue;
            for (int j = 1; j <= this._colMaxNumber; j++)
            {
                for (int i = 1; i <= this._rowMaxNumber; i++)
                {
                    Cell cell2 = null;
                    if (this._data.TryGetValue(new Coord(i, j), out cell2))
                    {
                        if (cell2.cellType == CellType.Number)
                        {
                            firstNumber = Math.Min(firstNumber, j);
                        }
                    }
                }
            }

            for (int i = 1; i <= this._rowMaxNumber; i++)
            {

                bool detectDate = false;
                for (int j = 1; j <= this._colMaxNumber; j++)
                {
                    Cell cell2 = null;
                    if (this._data.TryGetValue(new Coord(i, j), out cell2))
                    {
                        if (cell2.cellType == CellType.Date)
                        {
                            string s = GetStringFromDate(cell2);
                            if (s != "")
                            {
                                detectDate = true;
                                break;
                            }
                        }
                    }
                }

                string line = "<tr>";
                for (int j = 1; j <= this._colMaxNumber; j++)
                {
                    sblue = " pad w ws";
                    if (blue && (j <= firstNumber - 1 || detectDate)) sblue += " blue";
                    int icols = 0;
                    int irows = 0;
                    Cell cell2 = null;
                    if (this._data.TryGetValue(new Coord(i, j), out cell2))
                    {
                        string borders = "";
                        string rows = "";
                        string cols = "";
                        if (cell2.mergeCellAnchorRow != -12345)  //is a merge-cell
                        {
                            if (cell2.mergeCellAnchorRow == i && cell2.mergeCellAnchorCol == j)  //we are at anchor-cell (top-left of block)
                            {
                                icols = cell2.mergeCellShipCol - j + 1;
                                irows = cell2.mergeCellShipRow - i + 1;  //not used anyway
                                if (icols > 0) cols = " colspan = \"" + icols.ToString() + "\"";
                                if (irows > 0) rows = " rowspan = \"" + irows.ToString() + "\"";

                                Cell cell2ship = null;
                                if (this._data.TryGetValue(new Coord(i, cell2.mergeCellShipCol), out cell2ship))
                                {
                                    if (cell2ship.borderBottom && !cell2ship.borderBottomDisable) borders += " bottom";
                                    if (cell2ship.borderTop && !cell2ship.borderTopDisable) borders += " top";
                                    if (cell2ship.borderLeft && !cell2ship.borderLeftDisable) borders += " left";
                                    if (cell2ship.borderRight && !cell2ship.borderRightDisable)
                                    {
                                        if (cell2ship.borderRightColor == "gray")
                                        {
                                            borders += " rightlg";  //this coloring is the only one that works at the moment: used for <subcolborder> in XML table
                                        }
                                        else
                                        {
                                            borders += " right";
                                        }
                                    }
                                }
                            }
                        }

                        if (cell2.borderBottom && !cell2.borderBottomDisable) borders += " bottom";
                        if (cell2.borderTop && !cell2.borderTopDisable) borders += " top";
                        if (cell2.borderLeft && !cell2.borderLeftDisable) borders += " left";
                        if (cell2.borderRight && !cell2.borderRightDisable)
                        {
                            borders += " right";
                            if (cell2.borderRightColor == "gray")
                            {
                                borders += " rightlg";  //this coloring is the only one that works at the moment: used for <subcolborder> in XML table
                            }
                            else
                            {
                                borders += " right";
                            }
                        }
                        borders += sblue;
                        borders = ClassSnippet(borders);

                        //Consider using CSS "white-space:nowrap", but there may be a problem with old IE webbrowser.

                        string width2 = width;
                        if (cell2.cellType == CellType.Number)
                        {
                            width2 = "style=\"width:" + Program.options.table_html_datawidth.ToString() + "em; min-width:" + Program.options.table_html_datawidth.ToString() + "em\"";
                        }
                        if (j == 1)
                        {
                            width2 = "style=\"width:" + Program.options.table_html_firstcolwidth.ToString() + "em; min-width:" + Program.options.table_html_firstcolwidth.ToString() + "em\"";
                        }
                        if (j == 2)
                        {
                            width2 = "style=\"width:" + Program.options.table_html_secondcolwidth.ToString() + "em; min-width:" + Program.options.table_html_secondcolwidth.ToString() + "em\"";
                        }

                        if (cell2.cellType == CellType.Number)
                        {
                            string s = GetStringFromNumber(cell2, cell2.numberFormat, this);
                            if (s == "") s = "&nbsp;";
                            //seems IE webbrowser does not reac to <td width=...>
                            string number = s.Trim();
                            number = number.Replace(" ", "&nbsp;");
                            if (Program.options.table_html_specialminus) number = number.Replace("-", "&#8209;");
                            line += "<td " + width2 + " " + borders + " align = \"right\"" + cols + rows + " title=\"" + GetCellTitleForHtml(cell2.number, cell2.numberShouldShowAsN) + "\" >" + number + "</td>";
                        }
                        else if (cell2.cellType == CellType.Date)
                        {
                            string s = GetStringFromDate(cell2);
                            if (s == "") s = "&nbsp;";
                            line += "<td " + borders + " align = \"right\"" + cols + rows + ">" + s.Trim() + "</td>";
                        }
                        else if (cell2.cellType == CellType.Text)
                        {
                            string s = cell2.CellText.TextData[0];
                            //s = Program.SubstituteAssignVarsInExpression(s);  //probably superfluous, now that we substitute while reading the xml
                            if (s == "")
                            {
                                s = "&nbsp;";
                            }
                            string align = "\"left\"";
                            if (cell2.align == 0) align = "\"center\"";
                            if (cell2.align == 1) align = "\"right\"";
                            //replacing ' ' with &nbsp; avoids wrapping of long explanations in Internet Explorer (has a bug regarding nowrap option)
                            //line += "<td " + borders + cols + rows + ">" + s.Trim().Replace(" ", "&nbsp;").Replace("-", "&#8209;") + "</td>";
                            if (icols >= 2)
                            {
                                line += "<td " + borders.Replace(" w", " ") + " align = " + align + cols + rows + ">" + s.Trim().Replace(" ", "&nbsp;").Replace("-", "&#8209;") + "</td>";
                            }
                            else
                            {
                                line += "<td " + borders + " align = " + align + cols + rows + ">" + s.Trim().Replace(" ", "&nbsp;").Replace("-", "&#8209;") + "</td>";
                            }

                            int x = cell2.align;
                        }
                    }
                    else
                    {
                        line += "<td " + ClassSnippet(sblue) + ">&nbsp;</td>";
                    }
                    if (icols > 0)
                    {
                        j += icols - 1;
                    }
                }
                line += "</tr>";
                lines.Add(line);
            }
            lines.Add("</table>");            
            return lines;
        }

        private static string ClassSnippet(string s)
        {
            if (s.Length > 0) s = "CLASS=\"" + s + "\"";
            return s;
        }

        public void DeleteRow(int row)
        {
            //TODO: kvp.Value.mergeCellAnchorRow and kvp.Value.mergeCellShipRow
            //      for remaining cells should be corrected too, but for now we
            //      we skip this since merging over rows is not used at the moment
            //      anyway.

            DeleteRowHelper(row);
            this.UpdateTable();  //to get rowinfo etc updated
        }
        
        private void DeleteRowHelper(int row)
        {
            List<Coord> toRemove = new List<Coord>();
            foreach (KeyValuePair<Coord, Cell> kvp in this._data)
            {
                if (kvp.Key.Row == row)
                {
                    toRemove.Add(kvp.Key);
                }
                else if (kvp.Key.Row > row)
                {
                    kvp.Key.Row--;
                }
            }
            foreach (Coord cell in toRemove)
            {
                this._data.Remove(cell);
            }
        }

        public List<string> PrintText()
        {
            return PrintTextHelper(this);
        }

        public static List<string> PrintTextHelper(Table table)
        {
            Table clone = ObjectCopier.Clone<Table>(table);
            clone.UpdateTable();  //gets rendering stuff right etc.

            //PrintCellsForDebug();
            //pump up all coords with factor two, to create space for lines in between cells
            foreach (KeyValuePair<Coord, Cell> kvp in clone._data)
            {
                kvp.Key.Row *= 2;
                kvp.Key.Col *= 2;
                if (kvp.Value.mergeCellAnchorRow != -12345) kvp.Value.mergeCellAnchorRow *= 2;
                if (kvp.Value.mergeCellAnchorCol != -12345) kvp.Value.mergeCellAnchorCol *= 2;
                if (kvp.Value.mergeCellShipRow != -12345) kvp.Value.mergeCellShipRow *= 2;
                if (kvp.Value.mergeCellShipCol != -12345) kvp.Value.mergeCellShipCol *= 2;
            }
            clone.UpdateTable();  //to get rowinfo etc updated
            //clone.PrintCellsForDebug();

            List<CoordAndCell> temp = GetListOfKeyValuePairs(clone);

            foreach (CoordAndCell coordAndCell in temp)
            {
                if (coordAndCell.cell.borderLeft && !coordAndCell.cell.borderLeftDisable)
                {
                    clone.SetVerticalBorder(coordAndCell, -1, -1);
                    clone.SetVerticalBorder(coordAndCell, 0, -1);
                    clone.SetVerticalBorder(coordAndCell, 1, -1);
                }

                if (coordAndCell.cell.borderRight && !coordAndCell.cell.borderRightDisable)
                {
                    clone.SetVerticalBorder(coordAndCell, -1, 1);
                    clone.SetVerticalBorder(coordAndCell, 0, 1);
                    clone.SetVerticalBorder(coordAndCell, 1, 1);
                }

                if (coordAndCell.cell.borderBottom && !coordAndCell.cell.borderBottomDisable)
                {
                    clone.SetHorizontalBorder(coordAndCell, -1, -1);
                    clone.SetHorizontalBorder(coordAndCell, 0, -1);
                    clone.SetHorizontalBorder(coordAndCell, 1, -1);
                }

                if (coordAndCell.cell.borderTop && !coordAndCell.cell.borderTopDisable)
                {
                    clone.SetHorizontalBorder(coordAndCell, -1, 1);
                    clone.SetHorizontalBorder(coordAndCell, 0, 1);
                    clone.SetHorizontalBorder(coordAndCell, 1, 1);
                }
            }

            clone.UpdateTable();
            //clone.PrintCellsForDebug();

            //+++++++++++++++++++++++++++++++++++++
            //+++++++++++++++++++++++++++++++++++++
            //+++++++++++++++++++++++++++++++++++++

            for (int i = 1; i <= clone._rowMaxNumber; i++)
            {
                for (int j = 1; j <= clone._colMaxNumber; j++)
                {
                    Cell cell2 = null;
                    if (clone._data.TryGetValue(new Coord(i, j), out cell2))
                    {
                        if (cell2.cellType == CellType.Number)
                        {
                            string s = GetStringFromNumber(cell2, cell2.numberFormat, table);
                            s = " " + s + " ";
                            cell2.CellText = new Text();
                            cell2.CellText.TextData = new List<string>();
                            cell2.CellText.TextData.Add(s);
                            cell2.cellType = CellType.Text;
                            cell2.align = 1; //right
                        }
                        else if (cell2.cellType == CellType.Date)
                        {
                            string s = GetStringFromDate(cell2);
                            s = " " + s + " ";
                            if (cell2.CellText == null) cell2.CellText = new Text();
                            cell2.CellText.TextData = new List<string>();
                            cell2.CellText.TextData.Add(s);
                            cell2.cellType = CellType.Text;
                            cell2.align = 1; //right
                        }
                        else if (cell2.cellType == CellType.Text)
                        {
                            string s = cell2.CellText.TextData[0];
                            s = " " + s + " ";
                            cell2.CellText.TextData = new List<string>();
                            cell2.CellText.TextData.Add(s);
                        }
                    }
                }
            }

            for (int j = 1; j <= clone._colMaxNumber; j++)
            {

                //MUST be 1 if there is a column with at least one '|'

                int maxWidth = -12345;
                for (int i = 1; i <= clone._rowMaxNumber; i++)
                {
                    Cell cell2 = null;
                    if (clone._data.TryGetValue(new Coord(i, j), out cell2))
                    {
                        if (cell2.mergeCellAnchorRow != -12345) continue;  //don't do maxWidth for merge-cells!!!
                        if (cell2.cellType == CellType.Text)
                        {
                            
                            string s = cell2.CellText.TextData[0];
                            int length = s.Length;
                            if (s.Contains(Globals.linkActionStart))
                            {
                                length -= G.ExtraLinkLength(s);  //real length when link code is transformed to real link
                            }
                            
                            if (length > maxWidth) maxWidth = length;
                        }
                        else if (cell2.cellType == CellType.RenderingArtificialForDoingBorders)
                        {
                            if (cell2.renderingStuff.lineType[(int)Direction.VerticalAtBottom] || cell2.renderingStuff.lineType[(int)Direction.VerticalAtTop])
                            {
                                if (1 > maxWidth) maxWidth = 1;
                            }
                        }
                    }
                }
                if (maxWidth != -12345)
                {
                    clone._cols[j].renderingStuffCol.maxWidth = maxWidth;
                }
            }

            //Now, when printing, we look for text in the text field, or a line in renderingStuff
            //If not any of these, the cell is is completely empty
            List<string> resultingText = new List<string>();
            for (int i = 1; i <= clone._rowMaxNumber; i++)
            {
                bool isMerging = false;
                bool theRowContainsSomeHorizontalLine = false;
                string lineText = "";
                int height = -12345;  //default if there are no cells in that row (empty row printed as blank line)
                if (clone._rows.ContainsKey(i))
                {
                    height = clone._rows[i].renderingStuffRow.maxHeight;  //not used at the moment, assumes only 1 line per cell
                }
                for (int j = 1; j <= clone._colMaxNumber; j++)
                {
                    Cell cell2 = null;
                    int width = -12345;  //default if there are no cells in that col (empty col printed with width of 3 blanks)
                    if (clone._cols.ContainsKey(j))
                    {
                        width = clone._cols[j].renderingStuffCol.maxWidth;
                    }

                    if (width == -12345) width = 0;
                    if (height == -12345) height = 0;

                    if (clone._data.TryGetValue(new Coord(i, j), out cell2))
                    {
                        if (i % 2 == 0 && j % 2 == 0)  //only real cells
                        {
                            if (cell2.mergeCellAnchorRow != -12345)  //is a merge-cell
                            {
                                if (cell2.mergeCellAnchorRow == i && cell2.mergeCellAnchorCol == j)  //we are at anchor-cell (top-left of block)
                                {
                                    isMerging = true;
                                    string mergedText = "";
                                    int mergedTextAlign = 0;  //left
                                    Cell cell3 = null;
                                    if (clone._data.TryGetValue(new Coord(i, j), out cell3))
                                    {
                                        if (cell3.CellText != null && cell3.CellText.TextData != null)
                                        {
                                            mergedText += cell3.CellText.TextData[0];
                                            mergedTextAlign = cell3.align;
                                        }
                                    }
                                    int sum = 0;
                                    for (int jj = j; jj <= cell2.mergeCellShipCol; jj++)
                                    {
                                        int width2 = 0;
                                        if (clone._cols.ContainsKey(jj))
                                        {
                                            width2 = clone._cols[jj].renderingStuffCol.maxWidth;
                                            if (width2 == -12345) width2 = 0;
                                        }
                                        sum += width2;
                                    }
                                    string sCenter2 = StringManipulation.Align(mergedText, sum, 1, mergedTextAlign);
                                    lineText += sCenter2;
                                }
                                if (cell2.mergeCellShipRow == i && cell2.mergeCellShipCol == j)  //we are at ship-cell (bottom-right of block)
                                {
                                    isMerging = false;
                                    continue;  //don't print this -- will be if not continue here
                                }
                            }
                        }

                        if (isMerging) continue;

                        string s = "";
                        int align = 0;  //center
                        if (cell2.cellType == CellType.Text)
                        {
                            s = "";
                            if (cell2.CellText != null) s = cell2.CellText.TextData[0];  //takes first line, fix later on
                            if (s == null) s = "";
                            //s = Program.SubstituteAssignVarsInExpression(s);  //probably superfluous, now that we substitute while reading the xml
                            align = cell2.align;
                        }
                        else if (cell2.cellType == CellType.Number)
                        {
                            throw new GekkoException();
                        }
                        else if (cell2.cellType == CellType.Date)
                        {
                            throw new GekkoException();
                        }
                        else if (cell2.cellType == CellType.RenderingArtificialForDoingBorders)
                        {
                            if (cell2.renderingStuff.lineType[(int)Direction.HorizontalAtLeft] || cell2.renderingStuff.lineType[(int)Direction.HorizontalAtRight])
                            {
                                theRowContainsSomeHorizontalLine = true;
                            }
                            if (cell2.renderingStuff.lineType[(int)Direction.VerticalAtBottom] || cell2.renderingStuff.lineType[(int)Direction.VerticalAtTop])
                            {
                                s = "|";
                                if (Globals.prettyTextTableRendering)
                                {
                                    //this is pretty, but copy-paste to Kedit gives "?". Maybe an option?
                                    char c = '\u2502';
                                    s = c.ToString();
                                }
                            }
                            if (cell2.renderingStuff.lineType[(int)Direction.HorizontalAtLeft] || cell2.renderingStuff.lineType[(int)Direction.HorizontalAtRight])
                            {
                                s = new string('-', width);  //overwrites
                                if (Globals.prettyTextTableRendering)
                                {
                                    char c = '\u2500';
                                    s = new string(c, width);
                                }
                            }
                            if (cell2.renderingStuff.lineType[(int)Direction.HorizontalAtLeft] && cell2.renderingStuff.lineType[(int)Direction.HorizontalAtRight] && cell2.renderingStuff.lineType[(int)Direction.VerticalAtBottom] && cell2.renderingStuff.lineType[(int)Direction.VerticalAtTop])
                            {
                                s = "+";
                                if (Globals.prettyTextTableRendering)
                                {
                                    char c = '\u253C';
                                    s = c.ToString();
                                }
                            }
                        }
                        string sCenter = StringManipulation.Align(s, width, 1, align);
                        lineText += sCenter;
                    }
                    else
                    {
                        string s = "";
                        string sCenter = StringManipulation.Align(s, width, 1, 0);
                        lineText += sCenter;
                    }
                }
                //for artificial lines, nothing is printed unless there is some horizontal line somewhere
                if (i % 2 == 0 || (i % 2 != 0 && theRowContainsSomeHorizontalLine))
                {
                    resultingText.Add(lineText);
                }
            }
            return resultingText;
        }

        private void SetVerticalBorder(CoordAndCell coordAndCell, int type, int rightLeft)
        {
            //type: 1 is cell above, 0 is same row, -1 is below
            //rightleft: -1 is left, 1 is right
            if (rightLeft == 0) throw new GekkoException();
            Coord c2 = new Coord(coordAndCell.coord.Row + type, coordAndCell.coord.Col + rightLeft);
            Cell cell2 = null;
            if (!this._data.TryGetValue(c2, out cell2))
            {
                cell2 = new Cell();
                cell2.cellType = CellType.RenderingArtificialForDoingBorders;
                this.Set(c2, cell2);
            }
            if (cell2.renderingStuff == null) cell2.renderingStuff = new RenderingStuff();
            if (type == -1 || type == 0) cell2.renderingStuff.lineType[(int)Direction.VerticalAtTop] = true;
            if (type == 1 || type == 0) cell2.renderingStuff.lineType[(int)Direction.VerticalAtBottom] = true;
        }

        private void SetHorizontalBorder(CoordAndCell coordAndCell, int type, int topBottom)
        {
            //type: 1 is cell right, 0 is same col, -1 is left
            //topBottom: -1 is bottom, 1 is top
            if (topBottom == 0) throw new GekkoException();
            Coord c2 = new Coord(coordAndCell.coord.Row - topBottom, coordAndCell.coord.Col + type);
            Cell cell2 = null;
            if (!this._data.TryGetValue(c2, out cell2))
            {
                cell2 = new Cell();
                cell2.cellType = CellType.RenderingArtificialForDoingBorders;
                this.Set(c2, cell2);
            }
            if (cell2.renderingStuff == null) cell2.renderingStuff = new RenderingStuff();
            if (type == -1 || type == 0) cell2.renderingStuff.lineType[(int)Direction.HorizontalAtLeft] = true;
            if (type == 1 || type == 0) cell2.renderingStuff.lineType[(int)Direction.HorizontalAtRight] = true;
        }

        public void PrintCellsForDebug()
        {
            G.Writeln("==================================================");
            G.Writeln("Number of cells: " + this._data.Count);
            G.Writeln("Rows: " + this._rowMaxNumber);
            G.Writeln("Cols: " + this._colMaxNumber);
            G.Writeln("---");
            foreach (KeyValuePair<int, RowInfo> kvp in this._rows)
            {
                G.Writeln("Row #" + kvp.Key + ": max col = " + kvp.Value.ColMaxNumber + " render max height = " + kvp.Value.renderingStuffRow.maxHeight);
            }
            G.Writeln("---");
            foreach (KeyValuePair<int, ColInfo> kvp in this._cols)
            {
                G.Writeln("Col #" + kvp.Key + ": max row = " + kvp.Value.RowMaxNumber + " render max width = " + kvp.Value.renderingStuffCol.maxWidth);
            }
            G.Writeln("---");
            foreach (KeyValuePair<Coord, Cell> kvp in this._data)
            {
                string lines = "";
                string text = "<null>";

                text = GetCellContent(kvp.Value);

                if (kvp.Value.renderingStuff != null)
                {
                    //if (kvp.Value.renderingStuff.lineType == LineType.Horizontal) lines = "---";
                    //if (kvp.Value.renderingStuff.lineType == LineType.Vertical) lines = "|||";
                    //if (kvp.Value.renderingStuff.lineType == LineType.None) lines = "<none>";
                    lines = " is a render-cell";
                }
                if (lines != "" && lines != "<none>") G.Writeln(kvp.Key.Row + "," + kvp.Key.Col + " = " + text + "   line = " + lines);
                else G.Writeln(kvp.Key.Row + "," + kvp.Key.Col + " = " + text);
            }
            G.Writeln("==================================================");
            G.Writeln();
        }

        private static List<CoordAndCell> GetListOfKeyValuePairs(Table table)
        {
            //Used when there is a need to alter the keys in the table -- in such a situation
            //the Dictionary iterator refuses
            List<CoordAndCell> temp = new List<CoordAndCell>();
            foreach (KeyValuePair<Coord, Cell> kvp in table._data)
            {
                CoordAndCell coordAndCell = new CoordAndCell();
                coordAndCell.coord = kvp.Key;
                coordAndCell.cell = kvp.Value;
                temp.Add(coordAndCell);
            }
            return temp;
        }

        public void Set(int row, int col, string name, GekkoTime tStart, GekkoTime tEnd, string option, double scale, string format)
        {
            if (Globals.tableOption == "" || Globals.tableOption == "n" || Globals.tableOption == "m" || Globals.tableOption == Globals.operator_r)
            {
                //good
            }
            else
            {
                using (Error e = new Error())
                {
                    e.MainAdd("At the moment, the only table printcodes working are <m> or <r>. Of course,");
                    e.MainAdd("table printcodes such as <p> will be provided, but it has to be determined");
                    e.MainAdd("what to do if for instance <p> is used on a table containing variables with");
                    e.MainAdd("percentage transformation. Should this provoke an error or not, for instance? ");
                    e.MainAdd("Probably there will be some options determining what happens. Until this is");
                    e.MainAdd("sorted out, please be patient.");
                }
                throw new GekkoException();
            }
            string db = Globals.Work;
            if (G.Equal(Globals.tableOption, Globals.operator_r)) db = Globals.Ref;
            int counter = 0;
            foreach (GekkoTime t in new GekkoTimeIterator( tStart, tEnd))
            {
                if (Program.ShouldFilterPeriod(t)) continue;
                if (G.Equal(Globals.tableOption, "m"))
                {
                    if (G.Equal(option, "n"))
                    {
                        this.SetNumber(row, col + counter, scale * Program.MulLevel(name, t), format);
                    }
                    else if (G.Equal(option, "p"))
                    {
                        this.SetNumber(row, col + counter, scale * Program.MulPch(name, t), format);
                    }
                    else if (G.Equal(option, "d"))
                    {
                        this.SetNumber(row, col + counter, scale * Program.MulDif(name, t), format);
                    }
                    else
                    {
                        new Error("Table insert: expected code 'n' or 'p' or 'd'", false);
                    }
                }
                else
                {
                    if (G.Equal(option, "n"))
                    {
                        this.SetNumber(row, col + counter, scale * Program.Level(db, name, t), format);
                    }
                    else if (G.Equal(option, "p"))
                    {
                        this.SetNumber(row, col + counter, scale * Program.Pch(db, name, t), format);
                    }
                    else if (G.Equal(option, "d"))
                    {
                        this.SetNumber(row, col + counter, scale * Program.Dif(db, name, t), format);
                    }
                    else
                    {
                        new Error("Table insert: expected code 'n' or 'p' or 'd'", false);
                    }
                }
                counter++;
            }
        }

        public void Set(int row, int col, Series tsWork, Series tsBase, string varName, GekkoTime tStart, GekkoTime tEnd, string tableGlobalPrintCode, string printcode, double scale, string format)
        {
            if (tStart.IsNull() && tEnd.IsNull())
            {
                tStart = Globals.globalPeriodStart;
                tEnd = Globals.globalPeriodEnd;
            }

            int counter = 0;
            foreach (GekkoTime t in new GekkoTimeIterator(tStart, tEnd))
            {
                if (Program.ShouldFilterPeriod(t)) continue;

                double var1;
                double varPch; //not used here

                if (true)
                {
                    Program.ComputeValueForPrintPlotNew(out var1, out varPch, printcode, t, tsWork, tsBase, false, GekkoTime.tNull, true, EPrtCollapseTypes.None, 1);
                }                             

                this.SetNumber(row, col + counter, scale * var1, format);
                if (Globals.tableOption == "p" || Globals.tableOption == "q" || Globals.tableOption == "mp" || Globals.tableOption == "dp" || Globals.tableOption == Globals.operator_rp || Globals.tableOption == Globals.operator_rdp)
                {
                    Cell cell = this.Get(row, col + counter);
                    if (cell != null)
                    {
                        //should never be null, but for safety                        
                        if (cell.numberFormat != null && cell.numberFormat != "")
                        {
                            if (cell.numberFormat.EndsWith(".0"))
                            {
                                cell.numberFormat = cell.numberFormat.Substring(0, cell.numberFormat.Length - 2) + ".2";
                            }
                        }
                    }
                }
                counter++;
            }
        }

        private static void ReportSetError(string printcode, string s)
        {
            if (Program.IsOperatorShortBase(printcode))
            {
                new Error("You cannot use printcode '" + printcode + "' together with TABLE<" + s + ">");
                //throw new GekkoException();
            }
            if (Program.IsOperatorShortMultiplier(printcode))
            {
                new Error("You cannot use printcode '" + printcode + "' together with TABLE<" + s + ">");
                //throw new GekkoException();
            }
        }

    }



    [Serializable]
    public class RowInfo
    {
        public int ColMaxNumber { get; set; }
        public RenderingStuffRow renderingStuffRow = new RenderingStuffRow();
    }

    [Serializable]
    public class ColInfo
    {
        public int RowMaxNumber { get; set; }
        public RenderingStuffCol renderingStuffCol = new RenderingStuffCol();
    }

    [Serializable]
    public class RenderingStuffCol
    {
        public int maxWidth = -12345;
    }

    [Serializable]
    public class RenderingStuffRow
    {
        public int maxHeight = -12345;
    }

    [Serializable]
    public class Coord
    {

        public Coord(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; set; }

        public int Col { get; set; }


        public override int GetHashCode()
        {
            return 1000 * Row + Col;  //hardly ever a collision with this
        }

        public override bool Equals(object c2)
        {
            if (Col == ((Coord)c2).Col && Row == ((Coord)c2).Row) return true;
            else return false;
        }
    }

    /// <summary>
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx    ///
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>

    public static class ObjectCopier
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    public class CoordAndCell
    {
        public Coord coord;
        public Cell cell;
    }

    public enum Direction
    {
        HorizontalAtLeft,
        HorizontalAtRight,
        VerticalAtTop,
        VerticalAtBottom
    }

    public enum Align
    {
        Left,
        Center,
        Right
    }

    public enum BorderType
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public class BorderStyle2  //'2' because richtextboxex has class BorderStyle
    {
        //later on: thickness, style etc.
        public string color = null;
    }

    public enum CellType
    {
        Text,
        Date,
        VariableExpression,
        Number,
        RenderingArtificialForDoingBorders  //if so, there is no text, date, variableExpression, number... found
    }

    [Serializable]
    public class Cell
    {
        public CellType cellType = CellType.Text; //dates are special, since variables are looking for them (technical, does not show in layout)
        public int align = -1;  //-1 is left, looks best for text type

        //if one of these three it is a normal cell
        public Text CellText { get; set; }

        public string date;  //FIXME: quarters etc. Really ought to be a GekkoTime object
        public GekkoTime date_hack = GekkoTime.tNull;  //used to smuggle in dates, until above issue is fixed 
        public List<string> vars_hack = null;          //used to smuggle in "atomic" variables contained in a cell in a DECOMP table
        public double value_hack = double.NaN;         //used in decomp when sorting etc., so that the real background value is always known, even if something else is showing (for instance count or names or ...)

        public double number;
        public bool numberShouldShowAsN = false;
        public string numberFormat = "F15.4";  //default, way too wide and precise in most cases
        public string backgroundColor = "Transparent";

        public RenderingStuff renderingStuff = null; //if null, this cell is a "real" cell
        public bool borderLeft = false;
        public bool borderRight = false;
        public bool borderTop = false;
        public bool borderBottom = false;
        public bool borderLeftDisable = false;  //this overrules borderLeft = true
        public bool borderRightDisable = false;
        public bool borderTopDisable = false;
        public bool borderBottomDisable = false;
        public bool isDate = false;  //dates are special, since variables are looking for them (technical, does not show in layout)
        public string borderLeftColor = null;
        public string borderRightColor = null;
        public string borderTopColor = null;
        public string borderBottomColor = null;

        public int mergeCellAnchorRow = -12345;  //pointer to anchor position
        public int mergeCellAnchorCol = -12345;
        public int mergeCellShipRow = -12345;  //pointer to anchor position
        public int mergeCellShipCol = -12345;

        public Cell()
        {
            CellText = null;
        }
    }

    [Serializable]
    public class CurrentRow
    {
        public int _row = 1;
        public Table _table = null;
        public List<int> _hideLeftBorders = new List<int>();  //the int is col number
        public List<int> _hideRightBorders = new List<int>();  //the int is col number

        public void Next()
        {
            _row++;
            foreach (int col in _hideLeftBorders) _table.HideBorder(_row, col, _row, col, BorderType.Left);
            foreach (int col in _hideRightBorders) _table.HideBorder(_row, col, _row, col, BorderType.Right);
        }
        public void HideLeftBorder(int col)
        {
            _table.HideBorder(_row, col, _row, col, BorderType.Left);
            if (!_hideLeftBorders.Contains(col)) _hideLeftBorders.Add(col);
        }
        public void HideRightBorder(int col)
        {
            _table.HideBorder(_row, col, _row, col, BorderType.Right);
            if (!_hideRightBorders.Contains(col)) _hideRightBorders.Add(col);
        }
        public void ShowBorders()
        {
            foreach (int col in _hideLeftBorders) _table.ShowBorder(_row, col, _row, col, BorderType.Left);
            foreach (int col in _hideRightBorders) _table.ShowBorder(_row, col, _row, col, BorderType.Right);
            _hideLeftBorders.Clear();
            _hideRightBorders.Clear();
        }
        public void SetTopBorder(int col1, int col2)
        {
            _table.SetBorder(_row, col1, _row, col2, BorderType.Top);
        }
        public void SetBottomBorder(int col1, int col2)
        {
            _table.SetBorder(_row, col1, _row, col2, BorderType.Bottom);
        }
        public void SetLeftBorder(int col)
        {
            _table.SetBorder(1, col, _row, col, BorderType.Left);
        }
        public void SetLeftBorder(int col, string color)
        {
            BorderStyle2 bs = new BorderStyle2();
            bs.color = color;
            _table.SetBorder(1, col, _row, col, BorderType.Left, bs);
        }
        public void SetRightBorder(int col)
        {
            _table.SetBorder(1, col, _row, col, BorderType.Right);
        }
        public void SetRightBorder(int col, string color)
        {
            BorderStyle2 bs = new BorderStyle2();
            bs.color = color;
            _table.SetBorder(1, col, _row, col, BorderType.Right, bs);
        }
        public void SetText(int col1, string s)
        {
            _table.Set(_row, col1, s);
        }
        public void AlignLeft(int col)
        {
            _table.SetAlign(_row, col, _row, col, Align.Left);
        }
        public void AlignCenter(int col)
        {
            _table.SetAlign(_row, col, _row, col, Align.Center);
        }
        public void AlignRight(int col)
        {
            _table.SetAlign(_row, col, _row, col, Align.Right);
        }
        public void MergeCols(int col1, int col2)
        {
            _table.Merge(_row, col1, _row, col2);
        }
        public void SetDates(int col, string date1, string date2)
        {
            //Is probably obsolete, see method below
            GekkoTime gt1 = GekkoTime.FromStringToGekkoTime(date1);
            GekkoTime gt2 = GekkoTime.FromStringToGekkoTime(date2);
            _table.SetDates(_row, col, gt1, gt2);
        }
        public void SetDates(int col, GekkoTime gt1, GekkoTime gt2)
        {
            _table.SetDates(_row, col, gt1, gt2);
        }
        public void SetValues(int col, string name, string date1, string date2, string printcodes, double scale, string format)
        {
            GekkoTime gt1 = GekkoTime.FromStringToGekkoTime(date1);
            GekkoTime gt2 = GekkoTime.FromStringToGekkoTime(date2);
            _table.Set(_row, col, name, gt1, gt2, printcodes, scale, format);
        }
        public void SetValues(int col, Series tsWork, Series tsBase, string varName, GekkoTime gt1, GekkoTime gt2, string tableGlobalPrintCode, string printcode, double scale, string format)
        {
            //overload
            _table.Set(_row, col, tsWork, tsBase, varName, gt1, gt2, tableGlobalPrintCode, printcode, scale, format);
        }
    }

    [Serializable]
    public class RenderingStuff
    {
        public bool[] lineType = new bool[4];
    }

    [Serializable]
    public class Text
    {
        //maybe changed to List<string> to handle forced line breaks
        public List<string> TextData { get; set; }
        public Text(string s)
        {
            TextData = new List<string>();
            TextData.Add(s);
        }
        public Text()
        {
        }
    }

    [Serializable]
    public class LineType
    {
        bool[] lineType = new bool[4];
    }

    public static class StringManipulation
    {
        public static string Align(string s, int width, int height, int alignment)
        {
            if (s == null) s = "";
            int extra = 0;
            if (s.Contains(Globals.linkActionStart))
            {
                extra = G.ExtraLinkLength(s);
            }
            int realWidth = s.Length - extra;
            //string line1 = String.Format("{0," + width  + "}", s);
            //string line2 = String.Format("{0,-" + width + "}", String.Format("{0," + ((width + s.Length) / 2).ToString() + "}", s));
            //string line3 = String.Format("{0,-" + width + "}", s);  

            int excess = width - realWidth;
            int excess1 = excess / 2;
            int excess2 = excess - excess1;
                        
            string line1 = G.Blanks(excess) + s;
            string line2 = G.Blanks(excess1) + s + G.Blanks(excess2);
            string line3 = s + G.Blanks(excess);

            string s2 = "";
            if (alignment == -1) s2 = line3;
            else if (alignment == 0) s2 = line2;
            else if (alignment == 1) s2 = line1;
            else throw new GekkoException();
            if (s.Length > width && extra == 0)
            {
                return new string('*', width);
            }
            return s2;
        }
    }

    public static class TableStuff
    {
        public static void XmlTable(string filename, string html, string window, P p)
        {

            string tempfile = Globals.localTempFilesLocation + "\\" + "tablecode." + Globals.defaultCommandFileExtension;
            if (File.Exists(tempfile))
            {
                Program.WaitForFileDelete(tempfile);  //for safety
            }

            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("Parse/translate XML table start: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);

            XmlDocument doc = new XmlDocument();

            string xmlText = Program.GetTextFromFileWithWait(filename);

            XmlHelper xh = new XmlHelper();
            //using (FileStream fs = WaitForFileStream(filename, GekkoFileReadOrWrite.Read))
            //{

            //To convert to lower case: http://mel-green.com/2009/04/lowercase-xml-regex-csharp/
            //            Regex.Replace(
            //xml,
            //@"<[^<>]+>",
            //m => { return m.Value.ToLower(); },
            //RegexOptions.Multiline | RegexOptions.Singleline);

            //xmlText = xmlText.Replace("'", "´");
            //xmlText = xmlText.Replace("`", "´");

            try
            {
                doc.LoadXml(xmlText);
            }
            catch (Exception e)
            {                
                new Error("Table file: '" + filename + "'. " + Program.GetXmlError(e, filename));
            }

            VisitChildrenAndPutAssignvarsIntoNodes(doc, 0);

            CountRowsCols(doc, 0, xh);
            VisitChildren(doc, 0, xh);

            XmlNode rows = doc.SelectSingleNode("//rows");
            foreach (XmlNode row in rows.ChildNodes)
            {
                HandleXmlRow(xh, row);
            }

            bool isHtml = false;
            if (G.Equal(Program.options.table_type, "html")) isHtml = true;
            if (G.Equal(html, "yes")) isHtml = true;  //overrides if 'yes'

            StringBuilder s3 = new StringBuilder();
            if (isHtml && !G.Equal(window, "main"))
            {
                s3.AppendLine("TABLE tab.Print('html');");
            }
            else if (isHtml && G.Equal(window, "main"))
            {
                s3.AppendLine("TABLE tab.Print('html_main');");
            }
            else
            {
                //This will catch other cases, that is, txt
                s3.AppendLine("TABLE tab.Print();");
            }

            StringBuilder s2 = new StringBuilder();


            s2.AppendLine(Globals.tableConverterText1);
            s2.AppendLine(Globals.tableConverterText2);
            s2.AppendLine(Globals.tableConverterText3 + filename);
            s2.AppendLine(Globals.tableConverterText4);
            s2.AppendLine(Globals.tableConverterText5);

            s2.AppendLine("DATE %__t1 = %__tabletimestart;");
            s2.AppendLine("DATE %__t2 = %__tabletimeend;");
            s2.AppendLine("TABLE tab = new Table();");
            s2.AppendLine("VAL %__periods = %__t2 - %__t1 + 1 - filteredperiods(%__t1, %__t2);");
            s2.AppendLine("VAL %__c1 = 1;");

            string s = s2.ToString() + xh.s.ToString() + xh.sEnd.ToString() + s3.ToString();

            using (FileStream fs2 = Program.WaitForFileStream(tempfile, null, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter tempfile2 = G.GekkoStreamWriter(fs2))
            {
                tempfile2.Write(s);
                tempfile2.Flush();
                tempfile2.Close();
            }

            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("Parse/translate XML table end: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);
            //}
        }
        

        private static void HandleXmlRow(XmlHelper xh, XmlNode row)
        {

            if (row.Name == "row")
            {
                xh.rowCounter++;
                if (xh.rowCounter == xh.totalRows)
                    xh.isLastRow = true;
            }

            if (row.Name == "rowformat")
            {
                Attrib a = new Attrib();
                GetAttributes(row, a);
                MergeAttributes(xh.rowGlobal, a);
            }

            if (row.Name == "colbordershow")
            {
                if (row.ChildNodes.Count > 0)
                {
                    new Error("XML table: element '" + row.Name + "' should not have sub-elements");
                    //throw new GekkoException();
                }
                if (!xh.isLastRow)
                {
                    xh.s.AppendLine("TABLE tab.CurRow.ShowBorders();");
                }
            }

            if (row.Name == "colborderhide")
            {
                int totalBorders = xh.colLeftBorders.Count + xh.colRightBorderPlusOne.Count;
                string nodeText = row.InnerText;
                List<int> killCols = new List<int>();
                //nodeText="" means all borders
                if (row.ChildNodes.Count > 1)
                {
                    new Error("XML table: element '" + row.Name + "' should not have sub-elements");
                    //throw new GekkoException();
                }

                if (nodeText != "" && nodeText != "inner" && nodeText != "outer")
                {
                    string[] cols2 = nodeText.Split(',');
                    foreach (string s in cols2)
                    {
                        int col = -12345;
                        if (!int.TryParse(s, out col))
                        {
                            new Error("XML table: could not convert '" + s + "' to list of integers", false);
                        }
                        killCols.Add(col);
                    }
                }

                if (killCols.Count == 0)
                {
                    for (int i = 1; i <= totalBorders; i++)
                    {
                        if (nodeText == "outer" && (i > 1 && i < totalBorders)) continue;
                        if (nodeText == "inner" && (i == 1 || i == totalBorders)) continue;
                        XmlDisableBorder(xh, i);
                    }
                }
                else
                {
                    foreach (int i in killCols)
                    {
                        XmlDisableBorder(xh, i);
                    }
                }



            }

            if (row.Name == "row")
            {
                //G.Writeln("+++ " + row.Name + ":" + row.InnerText);

                Attrib rowA = new Attrib();
                GetAttributes(row, rowA);
                int counter = 0;
                foreach (XmlNode node in row.ChildNodes)
                {
                    string nodeName = node.Name;
                    string nodeText = node.InnerText;

                    if (nodeText.Contains("'"))
                    {                        
                        nodeText = nodeText.Replace("'", "´");
                    }

                    if (nodeText.Contains("`"))
                    {                        
                        nodeText = nodeText.Replace("`", "´");
                    }

                    if (nodeText.Contains("\""))
                    {
                        //nodeText = nodeText.Replace("\"", "´");  NO!
                    }

                    //nodeText = nodeText.Trim();  //also removes blank lines if there are any by accident

                    counter++;
                    if (counter > xh.cols.Count)
                    {
                        using (Error e = new Error())
                        {
                            e.MainAdd("Problem in XML table: there were more elements put into a <row> than defined columns.");
                            e.MainAdd("Please note that since Gekko 1.5.7, the behavoior regarding attribute 'colspan' has ");
                            e.MainAdd("been changed so that for instance after '<txt colspan = \"3\">', the next column");
                            e.MainAdd("will be the 4. column (and not the 2. column as in Gekko versions prior to 1.5.7.");
                            e.MainAdd("So if you use 'colspan', you may have to remove some empty <txt> tags...");
                        }                        
                    }
                    Attrib colInfo = xh.cols[counter - 1].attrib;
                    Attrib childA = new Attrib();
                    GetAttributes(node, childA);

                    Attrib a = new Attrib();
                    MergeAttributes(a, xh.table);       //completely global options
                    MergeAttributes(a, colInfo);        //options related to a specific column
                    MergeAttributes(a, xh.rowGlobal);   //options defined in <rowformat>
                    MergeAttributes(a, rowA);           //options defined in particular <row>
                    MergeAttributes(a, childA);         //options defined in particular item in row (i.e., column)

                    string a_vardisplay = a.Get("vardisplay");
                    if (a_vardisplay == null) a_vardisplay = "n";

                    string a_varscale = a.Get("varscale");
                    if (a_varscale == null) a_varscale = "1.0";

                    string a_varformat = a.Get("varformat");
                    if (a_varformat == null) a_varformat = "f12.2";  //we say that is default is nothing is stated at all

                    string a_datealign = a.Get("datealign");
                    if (a_datealign == null) a_datealign = "right";

                    string a_txtalign = a.Get("txtalign");
                    if (a_txtalign == null) a_txtalign = "left";

                    string a_colspan = a.Get("colspan");
                    if (a_colspan == null) a_colspan = "1";
                    int a_colspanint = GetIntFromAttrib(a_colspan);

                    //G.Writeln(nodeName);

                    if (nodeName == "txt")
                    {
                        if (nodeText == "$")
                        {
                            //text taken from next <var> tag
                            XmlNode next = node.NextSibling;
                            if (next != null)
                            {
                                string nextName = next.Name;
                                string nextText = next.InnerText;
                                if (nextName == "var")
                                {
                                    if (nextText != "")
                                    {
                                        nodeText = nextText;

                                        if (nodeText.Contains("'"))
                                        {
                                            nodeText = nodeText.Replace("'", "´");
                                        }

                                        if (nodeText.Contains("`"))
                                        {
                                            nodeText = nodeText.Replace("`", "´");
                                        }
                                    }
                                }
                            }
                        }

                        if ((nodeText == null || nodeText == "") && a_colspanint <= 1)
                        {
                            //#872483274
                            //do nothing, no need to do any MergeCols etc. for an empty <txt/> without span.
                            //and this would also interfere with subcolborder that would become broken.
                        }
                        else
                        {
                            xh.s.AppendLine("TABLE tab.CurRow.SetText(%__c" + counter + ", '" + nodeText + "');");
                            if (a_txtalign == "center")
                            {
                                xh.s.AppendLine("TABLE tab.CurRow.AlignCenter(%__c" + counter + ");");
                            }
                            else if (a_txtalign == "right")
                            {
                                xh.s.AppendLine("TABLE tab.CurRow.AlignRight(%__c" + counter + ");");
                            }
                            //could this not be omitted if a_colspanint is = 1???
                            //NO: THE COLS MIGHT GET SPANNED DUE TO DATES: SO THIS LOGIC IS NEEDED for stuff spanning through dates!!!!
                            xh.s.AppendLine("TABLE tab.CurRow.MergeCols(%__c" + counter + ", %__c" + (counter + a_colspanint) + " - 1);");
                        }

                        //If colspan is 2, this skips one more column to the right than normal
                        //Corresponds to HTML table behavior.
                        //Gekko versions < 1.5.7 did not have this skip.
                        counter += a_colspanint - 1;
                    }
                    else if (nodeName == "var")
                    {
                        string nodeText2 = nodeText.Trim();
                        if (nodeText2.ToLower().StartsWith("genr "))
                            nodeText2 = nodeText2.Substring("genr ".Length);
                        else if (nodeText2.ToLower().StartsWith("ser "))
                            nodeText2 = nodeText2.Substring("ser ".Length);
                        else if (nodeText2.ToLower().StartsWith("series "))
                            nodeText2 = nodeText2.Substring("series ".Length);

                        nodeText2 = nodeText2.Replace("´", "'");

                        if (colInfo.Get("type") == "expand")
                        {
                            xh.s.AppendLine("TABLE tab.CurRow.SetValues(%__c" + counter + ", %__t1, %__t2, " + nodeText2 + ", '" + a_vardisplay + "', " + a_varscale + ", '" + a_varformat + "');");
                        }
                        else if (colInfo.Get("period") != null)
                        {
                            string date = colInfo.Get("period");
                            xh.s.AppendLine("TABLE tab.CurRow.SetValues(%__c" + counter + ", " + date + ", " + date + ", " + nodeText2 + ", '" + a_vardisplay + "', " + a_varscale + ", '" + a_varformat + "');");
                        }
                        else
                        {
                            new Error("You are trying to put a variable into a column that is not expandable or of 'period' type");
                            //throw new GekkoException();
                        }
                        //G.Writeln(nodeName + ":" + nodeText);
                    }
                    else if (nodeName == "date")
                    {
                        if (colInfo.Get("type") == "expand")
                        {
                            xh.s.AppendLine("TABLE tab.CurRow.SetDates(%__c" + counter + ", %__t1, %__t2);");
                        }
                        else if (colInfo.Get("period") != null)
                        {
                            string date = colInfo.Get("period");
                            xh.s.AppendLine("TABLE tab.CurRow.SetDates(%__c" + counter + ", " + date + ", " + date + ");");
                        }
                        else
                        {
                            new Error("You are trying to put a date into a column that is not expandable or of 'period' type");
                            //throw new GekkoException();
                        }
                        //G.Writeln(nodeName + ":" + nodeText);
                    }
                }
            }

            if (row.Name == "rowborder")
            {
                if (xh.isLastRow)
                {
                    xh.s.AppendLine("TABLE tab.CurRow.SetBottomBorder(1, %__c" + (xh.cols.Count + 1) + " - 1);");
                }
                else
                {
                    xh.s.AppendLine("TABLE tab.CurRow.SetTopBorder(1, %__c" + (xh.cols.Count + 1) + " - 1);");
                }
            }
            if (row.Name == "row")  //this must be the last part of XML row handling
            {
                if (!xh.isLastRow)
                {
                    xh.s.AppendLine("TABLE tab.CurRow.Next();");
                }
            }
        }

        private static void XmlDisableBorder(XmlHelper xh, int i)
        {
            int totalBorders = xh.colLeftBorders.Count + xh.colRightBorderPlusOne.Count;

            if (i < 1 || i > totalBorders)
            {
                new Error("XML table: <colborderhide> should have numbers between 1 and " + totalBorders + " (inclusive)");
                //throw new GekkoException();
            }
            if (i < totalBorders)
            {
                //This can maybe fail ([i-1] call)
                if (i - 1 >= 0 && i - 1 < xh.colLeftBorders.Count)
                {
                    xh.s.AppendLine("TABLE tab.CurRow.HideLeftBorder(%__c" + xh.colLeftBorders[i - 1] + ");");
                }

            }
            else
            {
                //This can fail ([0] call)
                if (0 < xh.colRightBorderPlusOne.Count)
                {
                    xh.s.AppendLine("TABLE tab.CurRow.HideRightBorder(%__c" + xh.colRightBorderPlusOne[0] + " - 1);");
                }
            }
        }

        private static int GetIntFromAttrib(string input)
        {
            int parsed = -12345;
            if (!int.TryParse(input, out parsed))
            {
                new Error("Could not parse colspan '" + input + "' into an integer");
                //throw new GekkoException();
            }
            return parsed;
        }

        static void CountRowsCols(XmlNode node, int level, XmlHelper xh)
        {
            if (node.Name == "col") xh.totalCols++;
            if (node.Name == "row") xh.totalRows++;

            foreach (XmlNode echild in node.ChildNodes)
            {
                CountRowsCols(echild, level + 1, xh);
            }
        }

        static void VisitChildren(XmlNode node, int level, XmlHelper xh)
        {
            //G.Writeln("-".PadLeft(level + 1) + "" + node.Name + ": " + node.Value);

            if (node.Name == "cols")
            {
                Attrib a = new Attrib();
                GetAttributes(node, a);
            }

            if (node.Name == "col")
            {
                xh.cols.Add(new XmlColInfo());
            }

            if (node.Attributes != null)
            {
                Attrib a = null;
                if (node.Name == "table")
                {
                    a = xh.table;
                }
                else if (node.Name == "col")
                {
                    a = xh.cols[xh.cols.Count - 1].attrib;
                }
                if (a != null)
                {
                    GetAttributes(node, a);
                }
            }

            if (node.Name == "col")
            {
                int last = 0;
                if (xh.colB.Count > 0) last = xh.colB[xh.colB.Count - 1];
                xh.colA.Add(last + 1);
                Attrib aa = xh.cols[xh.cols.Count - 1].attrib;
                string add = "1";
                if (aa.Get("type") == "expand")
                {
                    add = "%__periods";
                }
                xh.s.AppendLine("VAL %__c" + (xh.cols.Count + 1) + " = %__c" + (xh.cols.Count) + " + " + add + ";");
            }

            if (node.Name == "colborder")
            {
                if (xh.cols.Count < xh.totalCols)
                {
                    int temp = xh.cols.Count + 1;
                    xh.sEnd.AppendLine("TABLE tab.CurRow.SetLeftBorder(%__c" + temp + ");");
                    xh.colLeftBorders.Add(temp);

                }
                else
                {
                    int temp = xh.cols.Count + 1;
                    xh.sEnd.AppendLine("TABLE tab.CurRow.SetRightBorder(%__c" + temp + " - 1);");
                    xh.colRightBorderPlusOne.Add(temp);
                }
            }

            if (node.Name == "subcolborder")  //must be child of <col>, puts data into this <col> (its parent)
            {
                Attrib a = new Attrib();
                if (node.Attributes != null)
                {
                    GetAttributes(node, a);
                }
                XmlColInfo xci = xh.cols[xh.cols.Count - 1];
                string per = a.Get("period");
                xci.subcolborders.Add(per);

                xh.sEnd.AppendLine("TABLE tab.CurRow.SetRightBorder(%__c" + xh.cols.Count + " + (date(" + per + ") - %__t1) - filteredperiods(%__t1, date(" + per + ")), 'gray');");


            }

            foreach (XmlNode echild in node.ChildNodes)
            {
                VisitChildren(echild, level + 1, xh);
            }
        }

        static void VisitChildrenAndPutAssignvarsIntoNodes(XmlNode node, int level)
        {
            //G.Write(G.Blanks(level) + node.Name + " ");
            if (node.Attributes != null)
            {
                foreach (XmlAttribute achild in node.Attributes)
                {
                    //achild.Value = achild.Value + "_12345";
                    //string name = achild.LocalName;
                    //string value = achild.Value;
                    //achild.Value = SubstituteAssignVarsInExpression(achild.Value);
                    //G.Write("[attrib] " + name + "=\"" + value + "\" ");
                }
            }

            //why isnt .Value used in other places (seems innertext or so)
            //here we could substitute, remember the \\# etc.
            //string text = null;
            if (node.NodeType == XmlNodeType.Text || node.NodeType == XmlNodeType.CDATA)
            {
                node.Value = node.Value;
                //text = node.Value;
            }
            //G.Writeln("[value] \"" + text + "\"");

            foreach (XmlNode echild in node.ChildNodes)
            {
                VisitChildrenAndPutAssignvarsIntoNodes(echild, level + 1);
            }
        }

        private static void GetAttributes(XmlNode node, Attrib a)
        {
            if (node.Attributes == null) return;
            foreach (XmlAttribute achild in node.Attributes)
            {
                string name = achild.LocalName;
                string value = achild.Value;
                if (a.Set(name, value) == false)
                {
                    new Error("XML table has duplicate attribute name: " + name, false);
                }
            }
        }

        private static void MergeAttributes(Attrib result, Attrib newItems)
        {
            foreach (var item in newItems.data)
            {
                result.data[item.Key] = item.Value;
            }
        }

        public static StringBuilder ConvertPcimTable(string file)
        {
            StringBuilder x = new StringBuilder();
            int start = 4;
            int longest = 0;

            for (int u = 0; u < 2; u++)
            {
                x = new StringBuilder();

                string colformat = "";

                x.AppendLine("<?xml version=`1.0` encoding=`Windows-1252`?>");
                x.AppendLine("<gekkotable>");
                x.AppendLine("  <tableversion>1.0</tableversion>");
                x.AppendLine("  <header>");
                x.AppendLine("    <printinfo type=`full` />");
                x.AppendLine("  </header>");
                x.AppendLine("  <table>");
                x.AppendLine("    <cols>");
                x.AppendLine("      <colborder />");
                x.AppendLine("      <col txtalign=`left`></col>");
                x.AppendLine("      <colglue />");
                x.AppendLine("      <col txtalign=`right` />");
                x.AppendLine("      <colborder />");
                x.AppendLine("      <col type=`expand` txtalign=`center`></col>");
                x.AppendLine("      <colborder />");
                x.AppendLine("    </cols>");
                x.AppendLine("    <rows>");

                string tab = ",";
                //string file = Program.options.folder_working + "\\s56b.TAB";
                List<string> lines = Stringlist.ExtractLinesFromText(Program.GetTextFromFileWithWait(file));
                int counter = 0;
                foreach (string line2 in lines)
                {
                    counter++;
                    string line = line2.Trim();
                    //char[] c = line.ToCharArray();
                    if (line.StartsWith("01"))
                    {
                        int ii = G.ConvertToInt(G.Substring(line, 3 - 1, 5 - 1));
                        if (ii == int.MaxValue || ii < 1)
                        {
                            TableConvertError(u, file, counter, line, "Number of variable fields < 1...??");
                        }
                        else if (ii > 1)
                        {
                            TableConvertError(u, file, counter, line, "Converter only works with 1 variable field.");
                        }
                        ii = 1;
                    }
                    else if (line.StartsWith("02"))
                    {
                        int w = G.ConvertToInt(G.Substring(line, 3 - 1, 4 - 1));
                        if (w == int.MaxValue || w < 1)
                        {
                            TableConvertError(u, file, counter, line, "Width problem....");
                        }
                        string code = G.Substring(line, 5 - 1, 5 - 1).ToLower();
                        if (code == "i")
                        {
                        }
                        else if (code == "f")
                        {
                        }
                        else
                        {
                            TableConvertError(u, file, counter, line, "Number format is not 'i' or 'f'...");
                        }
                        int decimals = int.MaxValue;
                        if (line.Length > 5) decimals = G.ConvertToInt(G.Substring(line, 6 - 1, 6 - 1));
                        if (decimals == int.MaxValue)
                        {
                            if (code == "i")
                            {
                            }
                            else
                            {
                                TableConvertError(u, file, counter, line, "Number of decimals not found for 'f'-number");
                            }
                            colformat = "f" + w + ".0";
                        }
                        else
                        {
                            colformat = "f" + w + "." + decimals;
                        }
                        x.AppendLine("      <rowformat varformat=`" + colformat + "`/>");
                    }
                    else if (line.StartsWith("03"))
                    {
                        //ignore these borders
                    }
                    else if (line.StartsWith("06"))
                    {
                        string s = line.Substring(start - 1);

                        s = G.ReplaceFirstOccurrence(s, tab, "€");

                        if (u == 0 && s != "" && !line.Substring(start - 2).StartsWith(" "))
                        {
                            start = 3;
                            //continue;  //for instance 06Investments
                        }
                        string[] ss = s.Split('€');
                        string s1 = ss[0].TrimEnd();
                        string s2 = null;
                        if (ss.Length > 1) s2 = ss[1].Trim();
                        if (ss.Length > 2)
                        {
                            TableConvertError(u, file, counter, line, "Only expected 2 delimiter characters...");
                        }
                        if (s2 == null || s2 == "")
                        {
                            string span = "2";
                            if (u == 1 && s1.Length > longest) span = "3";
                            x.AppendLine("      <row>");
                            x.AppendLine("        <txt colspan=`" + span + "`>" + s1 + "</txt>");
                            x.AppendLine("      </row>");
                            //if (s1.Length > longest) longest = s1.Length;
                        }
                        else
                        {
                            string lbl = "";
                            string text = "";
                            string var = "";
                            if (s1 != "" && s2 != "")
                            {
                                if (s1.ToLower().EndsWith(s2.ToLower()))
                                {
                                    //easy, same name both places
                                    if (s1.Length == s2.Length)
                                    {
                                        text = "";
                                        lbl = "$";
                                        var = s2;
                                    }
                                    else
                                    {
                                        string s3 = s1.Substring(s1.Length - s2.Length - 1, 1);
                                        if (s3 == " " || s3 == "." || s3 == ",")
                                        {
                                            string temp = s1.Substring(0, s1.Length - s2.Length).TrimEnd();
                                            if (temp.Length == 0) text = " ";
                                            else text = temp;
                                            lbl = "$";
                                            var = s1.Substring(s1.Length - s2.Length).Trim();
                                        }
                                    }
                                }
                            }

                            if (text == "")
                            {
                                string lastWord = "";
                                for (int i = s1.Length - 1; i >= 1; i--)
                                {
                                    if (!(G.IsLetterOrDigitOrUnderscore(s1[i]) || s1[i] == '+' || s1[i] == '-' || s1[i] == '*' || s1[i] == '/' || s1[i] == '(' || s1[i] == ')'))
                                    {
                                        if (s1[i] == ' ' && s1[i - 1] == ' ')  //two blanks before word
                                        {
                                            if (i < s1.Length - 1)
                                            {
                                                lastWord = s1.Substring(i + 1);
                                            }
                                        }
                                        break;
                                    }
                                }

                                if (lastWord != "")
                                {

                                    //try looking at last word
                                    text = s1.Substring(0, s1.Length - lastWord.Length).TrimEnd();
                                    lbl = lastWord;
                                    var = s2.Trim();
                                }
                            }

                            if (text == "")
                            {
                                text = s1.TrimEnd();
                                lbl = "";
                                var = s2.Trim();
                            }

                            if (var.ToLower().StartsWith("series")) var = var.Substring(4);
                            var = var.Replace("$", "");
                            var = var.Replace(";", "");
                            var = var.Trim();

                            text = Program.SpecialXmlChars(text);

                            x.AppendLine("      <row>");
                            x.AppendLine("        <txt>" + text + "</txt>");
                            x.AppendLine("        <txt>" + lbl + "</txt>");
                            if (text.Length + lbl.Length + 2 > longest) longest = text.Length + lbl.Length + 2;
                            if (var.ToLower() == "year")
                            {
                                x.AppendLine("        <date/>");
                            }
                            else
                            {
                                x.AppendLine("        <var>" + var + "</var>");
                            }
                            x.AppendLine("      </row>");
                        }
                    }
                    else if (line.StartsWith("07"))
                    {
                        //blank line
                        x.AppendLine("      <row/>");
                    }
                    else if (line.StartsWith("08"))
                    {
                        //sideskift
                    }
                    else if (line.StartsWith("11"))
                    {
                        string s = G.Substring(line, 3 - 1, 3 - 1);
                        if (s == null)
                        {
                            TableConvertError(u, file, counter, line, "Problem with delimiter character");
                        }
                        tab = s;
                    }
                    else if (line.StartsWith("12"))
                    {
                        if (line.Length > 2)
                        {
                            string s = line.Substring(2).Trim();
                            x.AppendLine("<!-- " + Program.SpecialXmlChars(s) + " -->");
                        }
                    }
                    else if (line.StartsWith("13"))
                    {
                        //finish of this table
                    }
                    else if (line.StartsWith("18"))
                    {
                        //new screen
                    }
                    else if (line.StartsWith("04") || line.StartsWith("05") || line.StartsWith("41"))
                    {
                        string s = "";
                        if (line.Length == 2)
                        {
                        }
                        else
                        {
                            s = line.Substring(start - 1);
                            if (u == 0 && s != "" && !line.Substring(start - 2).StartsWith(" "))
                            {
                                start = 3;
                                //continue;  //for instance 41Investments
                            }
                        }
                        s = Program.SpecialXmlChars(s);
                        string span = "2";
                        if (u == 1 && s.Length > longest) span = "3";
                        x.AppendLine("      <row>");
                        x.AppendLine("        <txt colspan=`" + span + "`>" + s + "</txt>");
                        x.AppendLine("      </row>");
                        //if (s.Length > longest) longest = s.Length;
                    }
                    else if (line.StartsWith("20") || line.StartsWith("21"))
                    {
                        TableConvertError(u, file, counter, line, "Code 20 and 21 (multiplier) are not converted");
                    }
                    else if (line.StartsWith("22"))
                    {
                        TableConvertError(u, file, counter, line, "Code 22 (scaling) is not converted -- please fix with 'varscale' in XML table");
                    }
                    else if (line.StartsWith("24"))
                    {
                        TableConvertError(u, file, counter, line, "Code 24 (% growth under variables) is not converted");
                    }
                    else if (line.StartsWith("27"))
                    {
                        if (line.Substring(2, 1).ToLower() == "t")
                        {
                            x.AppendLine("      <rowformat vardisplay=`p`/>");
                        }
                        if (line.Substring(2, 1).ToLower() == "f")
                        {
                            x.AppendLine("      <rowformat vardisplay=`n`/>");
                        }
                    }
                    else if (line.StartsWith("28"))
                    {
                        TableConvertError(u, file, counter, line, "Code 28 (differences) is not converted");
                    }
                    else if (line.StartsWith("30"))
                    {
                        TableConvertError(u, file, counter, line, "Code 28 (multiplier difference) is not converted");
                    }
                    else if (line.StartsWith("44") || line.StartsWith("45") || line.StartsWith("46") || line.StartsWith("47"))
                    {
                        //ignore table headers
                    }
                    else if (line.StartsWith("55"))
                    {
                        TableConvertError(u, file, counter, line, "Code 55 (alignment) is not converted");
                    }
                    else if (line.StartsWith("91"))
                    {
                        x.AppendLine("      <rowborder/>");
                    }
                    else if (line.StartsWith("92"))
                    {
                        x.AppendLine("      <rowborder/>");
                    }
                    else if (line.StartsWith("93"))
                    {
                        x.AppendLine("      <rowborder/>");
                    }
                    else if (line.StartsWith("94"))
                    {
                        x.AppendLine("      <rowborder/>");
                    }
                    else if (line.StartsWith("95"))
                    {
                        x.AppendLine("      <rowborder/>");
                    }
                    else if (line.StartsWith("96"))
                    {
                        x.AppendLine("      <rowborder/>");
                    }
                    else if (line.StartsWith("97"))
                    {
                        x.AppendLine("      <rowborder/>");
                    }
                    else if (line.StartsWith("98"))
                    {
                        x.AppendLine("      <rowborder/>");
                    }
                    else if (line.StartsWith("99"))
                    {
                        //finish tabledef
                    }
                    else if (line.StartsWith("menu"))
                    {
                        //ignore menu calls
                    }
                    else if (line == "")
                    {
                    }
                    else
                    {
                        string code = "";
                        if (line.Length >= 2) code = line.Substring(0, 2);
                        TableConvertError(u, file, counter, line, "Did not recognize this table code: " + code);
                    }
                }  //end of foreach

                x.AppendLine("    </rows>");
                x.AppendLine("  </table>");
                x.AppendLine("</gekkotable>");

                x.Replace("`", "\"");
                //break;
            }  // for u

            return x;
        }

        private static void TableConvertError(int u, string file, int counter, string line, string s)
        {
            if (u > 0)
            {
                Globals.convertTableErrorCounter++;
                G.Writeln();
                G.Writeln2("*** ERROR: In " + file + " line " + counter);
                G.Writeln("           Could not understand: " + line, Color.Red);
                G.Writeln("           " + s, Color.Red);
                G.Writeln("           " + "(Total error count: " + Globals.convertTableErrorCounter + ")");
                G.Writeln();
            }
        }

        private static void MenuConvertError(string file, int counter, string line, string s)
        {
            if (true)
            {
                Globals.convertMenuErrorCounter++;
                G.Writeln();
                G.Writeln2("*** ERROR: In " + file + " line " + counter);
                G.Writeln("           Could not understand: " + line, Color.Red);
                G.Writeln("           " + s, Color.Red);
                G.Writeln("           " + "(Total error count: " + Globals.convertMenuErrorCounter + ")");
                G.Writeln();
            }
        }

        public static StringBuilder ConvertPcimMenu(int depth, string file)
        {
            StringBuilder x = new StringBuilder();

            StringBuilder x2 = new StringBuilder();

            string title = null;

            List<string> lines = Stringlist.ExtractLinesFromText(Program.GetTextFromFileWithWait(file));
            int counter = 0;

            string path = "";
            for (int i = 0; i < depth; i++)
            {
                path = path + "../";
            }

            foreach (string line2 in lines)
            {
                if (line2.ToLower() == "case") continue;  //kind of empty line
                string line = line2.Trim();
                counter++;
                if (line.Contains("\""))
                {
                    MenuConvertError(file, counter, line, "Menu lines should not contain full quotes (\")");
                }


                if (line.ToLower().StartsWith("nymenu "))
                {
                }
                else if (line.ToLower().StartsWith("select "))
                {
                }
                else if (line.ToLower().StartsWith("case "))
                {
                    string lineTrunc = line.Substring(5);
                    string[] split = lineTrunc.Split('/');
                    if (split.Length == 1)
                    {
                        if (title == null)
                        {
                            title = split[0].Trim();  //only first time
                        }
                        else
                        {
                            //2. time and on
                            x2.AppendLine("      <li>" + split[0].Trim() + "</li>");
                        }
                    }
                    else if (split.Length == 2)
                    {
                        x2.AppendLine("      <li><a href=`" + split[1].Trim() + ".html`>" + split[0].Trim() + "</a></li>");
                    }
                    else if (split.Length >= 3)
                    {
                        if (split.Length > 3) MenuConvertError(file, counter, line, "There are more than 2 '/' in the line: cannot understand");

                        string s1 = split[0].Trim();

                        string lastWord = "";
                        for (int i = s1.Length - 1; i >= 1; i--)
                        {
                            if (!(G.IsLetterOrDigitOrUnderscore(s1[i]) || s1[i] == '-' || s1[i] == '(' || s1[i] == ')'))
                            {
                                if (s1[i] == ' ' && s1[i - 1] == ' ')  //two blanks before word
                                {
                                    if (i < s1.Length - 1)
                                    {
                                        lastWord = s1.Substring(i + 1);
                                    }
                                }
                                break;
                            }
                        }

                        string text = s1;
                        string lbl = "";

                        if (lastWord != "")
                        {
                            //try looking at last word
                            text = s1.Substring(0, s1.Length - lastWord.Length).TrimEnd();
                            lbl = lastWord;
                        }
                        text = text + " ";

                        string img = "<img src=`" + path + "table.png`>";

                        if (lbl != "")
                        {
                            x2.AppendLine("      <li><a href=`" + split[2].Trim() + "." + Globals.extensionTable + "`>" + img + " " + text + " <font color=`silver`>(" + lbl + ")</font>" + "</a>" + "</li>");
                        }
                        else
                        {
                            x2.AppendLine("      <li><a href=`" + split[2].Trim() + "." + Globals.extensionTable + "`>" + img + " " + text + "</a>" + "</li>");
                        }
                    }
                }
                else
                {
                    MenuConvertError(file, counter, line, "Could not recognize this as a menu line, skipping");
                }
            }

            if (title == null) title = "Menu";

            x.AppendLine("<!DOCTYPE HTML PUBLIC `-//W3C//DTD HTML 4.01 Transitional//EN`>");
            x.AppendLine("<html>");
            x.AppendLine("  <head>");
            x.AppendLine("    <link rel=`stylesheet` href=`" + path + "styles.css` type=`text/css`>");
            x.AppendLine("    <meta http-equiv=`Content-Type` content=`text/html; charset=iso-8859-1`>");
            x.AppendLine("    <title>" + title + "</title>");
            x.AppendLine("  </head>");
            x.AppendLine("  <body>");
            x.AppendLine("  <big><b>" + title + "</b></big><br>");
            x.AppendLine("    <ul>");
            x.Append(x2);
            x.AppendLine("    </ul>");
            x.AppendLine("  </body>");
            x.AppendLine("</html>");

            x.Replace("`", "\"");

            return x;
        }

        public static void ConvertMenu2()
        {
            string folder = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Program.options.folder_working;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                folder = fbd.SelectedPath;
            }
            if (folder != "")
            {
                string outputPath = Globals.localTempFilesLocation + "\\" + "menufilesconversion";
                G.DeleteFolder(outputPath, true);
                Directory.CreateDirectory(outputPath);

                Globals.convertMenuCounter = 0;
                Globals.convertMenuErrorCounter = 0;

                G.Writeln("Starting conversion of menus...");

                ConvertMenu(0, folder, folder, outputPath);

                bool ok = true;
                try
                {
                    string stylesFile = Application.StartupPath + "\\images\\" + "styles.css";
                    string tableIconFile = Application.StartupPath + "\\images\\" + "table.png";
                    Program.WaitForFileCopy(stylesFile, outputPath + "\\styles.css");
                    Program.WaitForFileCopy(tableIconFile, outputPath + "\\table.png");
                }
                catch (Exception e)
                {
                    ok = false;
                    new Error("Problem with style-sheets, may impact the menus (files styles.css and table.png)", false);
                }

                G.Writeln();
                G.Write("Zipping converted files... ");
                string zipFileNameInput = Program.CreateFullPathAndFileName(Program.options.folder_working + "\\converted_menus.zip");
                Program.WaitForZipWrite(outputPath, zipFileNameInput);
                G.Writeln("... ended");
                G.Writeln();
                G.Writeln("Converted " + Globals.convertMenuCounter + " menus from folder: " + folder);
                G.Writeln("There were " + Globals.convertMenuErrorCounter + " error messages.");
                G.Writeln("See the file converted_menus.zip in the working folder.");
                if (ok)
                {
                    G.Writeln("Note the two files styles.css and table.png in the root of the zip file: these");
                    G.Writeln("two files are used to style the menus, without them the menus will look ugly!");
                }
            }
            else
            {
                new Error("Please choose a folder, false");
            }
        }

        public static void ConvertMenu(int depth, string start, string folder, string outputPath)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.ToLower().EndsWith(".cmd"))
                {
                    //G.Writeln("table file: " + file);
                    string extra = file.Replace(start, "");

                    StringBuilder x = new StringBuilder();

                    try
                    {
                        x = TableStuff.ConvertPcimMenu(depth, file);
                    }
                    catch (Exception e)
                    {
                        G.Writeln();
                        G.Writeln("Conversion failed completely: " + file);
                        G.Writeln();
                        x = null;
                    }

                    if (x != null)
                    {
                        Globals.convertMenuCounter++;
                        string newfile = outputPath + extra;
                        if (newfile.ToLower().EndsWith(".cmd"))
                        {
                            newfile = newfile.Substring(0, newfile.Length - 4);
                            newfile = newfile + ".html";
                        }
                        else
                        {
                            new Error("*** ERROR: Converting menus: file name: " + newfile, false);
                        }
                        string path = Path.GetDirectoryName(newfile);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        using (FileStream fs = Program.WaitForFileStream(newfile, null, Program.GekkoFileReadOrWrite.Write))
                        using (StreamWriter sw = G.GekkoStreamWriter(fs))
                        {
                            sw.Write(x);
                            sw.Flush();
                        }
                    }
                }
            }
            if (true)  //we allow converting subfolders
            {
                foreach (string subDir in Directory.GetDirectories(folder))
                {
                    ConvertMenu(depth + 1, start, subDir, outputPath);
                }
            }
        }





    }
}
