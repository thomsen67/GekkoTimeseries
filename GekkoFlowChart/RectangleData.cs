using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace GekkoFlowChart
{
    /// <summary>
    /// Defines the data-model for a simple displayable rectangle.
    /// </summary>
    public class RectangleData : INotifyPropertyChanged
    {
        #region Data Members

        /// <summary>
        /// The X coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        private double x = 0;

        /// <summary>
        /// The Y coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        private double y = 0;

        /// <summary>
        /// The width of the rectangle (in content coordinates).
        /// </summary>
        private double width = 0;

        /// <summary>
        /// The height of the rectangle (in content coordinates).
        /// </summary>
        private double height = 0;

        /// <summary>
        /// The color of the rectangle.
        /// </summary>
        private Color color;

        /// <summary>
        /// Set to 'true' when the rectangle is selected in the ListBox.
        /// </summary>
        private bool isSelected = false;

        private string label;

        private string labelBig;

        #endregion Data Members

        public RectangleData()
        {
        }

        public RectangleData(double x, double y, double width, double height, string label, string labelBig, Color color)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
            this.label = label;
            this.labelBig = labelBig;
        }

        /// <summary>
        /// The X coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;

                OnPropertyChanged("X");
            }
        }

        /// <summary>
        /// The Y coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;

                OnPropertyChanged("Y");
            }
        }

        /// <summary>
        /// The width of the rectangle (in content coordinates).
        /// </summary>
        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;

                OnPropertyChanged("Width");
            }
        }

        /// <summary>
        /// The height of the rectangle (in content coordinates).
        /// </summary>
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;

                OnPropertyChanged("Height");
            }
        }

        /// <summary>
        /// The color of the rectangle.
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;

                OnPropertyChanged("Color");
            }
        }

        /// <summary>
        /// The label of the rectangle.
        /// </summary>
        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;

                OnPropertyChanged("Label");
            }
        }

        /// <summary>
        /// The label of the rectangle.
        /// </summary>
        public string LabelBig
        {
            get
            {
                return labelBig;
            }
            set
            {
                labelBig = value;

                OnPropertyChanged("LabelBig");
            }
        }

        /// <summary>
        /// Set to 'true' when the rectangle is selected in the ListBox.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;

                OnPropertyChanged("IsSelected");
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the data model has changed.
        /// </summary>
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// 'PropertyChanged' event that is raised when the value of a property of the data model has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public class ArrowData : INotifyPropertyChanged
    {
        #region Data Members

        /// <summary>
        /// The X coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        private double x = 0;

        /// <summary>
        /// The Y coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        private double y = 0;

        /// <summary>
        /// The width of the rectangle (in content coordinates).
        /// </summary>
        private double width = 0;

        /// <summary>
        /// The height of the rectangle (in content coordinates).
        /// </summary>
        private double height = 0;

        /// <summary>
        /// The color of the rectangle.
        /// </summary>
        private Color color;

        private double weight = 0d;

        private string label;        

        /// <summary>
        /// Set to 'true' when the rectangle is selected in the ListBox.
        /// </summary>
        private bool isSelected = false;

        #endregion Data Members

        public ArrowData()
        {
        }

        public ArrowData(double x, double y, double width, double height, Color color, double weight)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
            this.weight = weight;
        }

        /// <summary>
        /// The X coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;

                OnPropertyChanged("X");
            }
        }

        /// <summary>
        /// The Y coordinate of the location of the rectangle (in content coordinates).
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;

                OnPropertyChanged("Y");
            }
        }

        /// <summary>
        /// The width of the rectangle (in content coordinates).
        /// </summary>
        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;

                OnPropertyChanged("Width");
            }
        }

        /// <summary>
        /// The height of the rectangle (in content coordinates).
        /// </summary>
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;

                OnPropertyChanged("Height");
            }
        }

        /// <summary>
        /// The color of the rectangle.
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;

                OnPropertyChanged("Color");
            }
        }

        /// <summary>
        /// The weicht of the arrow.
        /// </summary>
        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;

                OnPropertyChanged("Weight");
            }
        }

        /// <summary>
        /// Set to 'true' when the rectangle is selected in the ListBox.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;

                OnPropertyChanged("IsSelected");
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the data model has changed.
        /// </summary>
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// 'PropertyChanged' event that is raised when the value of a property of the data model has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

}
