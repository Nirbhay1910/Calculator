using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CalculatorLibrary;

namespace CalculatorUI
{
    public partial class Calculator : Form
    {
        private ExpressionEvaluator _evaluator = new ExpressionEvaluator();
        private SplitContainer _splitContainer;
        private TextBox _display;
        private TableLayoutPanel _tableLayout;
        private List<Button> _buttonList;
        private Button _buttonClear;
        private Button _buttonDelete;
        private MenuStrip _mainMenuStrip;
        private bool _decimalPressed = false;
        private ToolStripMenuItem _editToolStripMenuItem;
        private ToolStripMenuItem _exitToolStripMenuItem;
        private ToolStripMenuItem _helpToolStripMenuItem;
        private ToolStripItem _copy;
        private ToolStripItem _paste;
        private Font _fontStyle = new Font("Microsoft Sans Serif", 17F);
        private Stack<string> _lastButtonPressed = new Stack<string>();
        private string _memory = "";

        public Calculator()
        {
            InitializeComponent();
            
            /// Split container Layout
            _splitContainer = new SplitContainer()
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                Location = new Point(0, 0),
                Size = new Size(150,250),
                SplitterDistance = 30,
                IsSplitterFixed = true
            };
            Controls.Add(_splitContainer);

            /// TextBox or Display
            _display = new TextBox()
            {
                Font = new Font("Microsoft Sans Serif", 18F),
                Dock = DockStyle.Bottom,
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true,
            };
            _splitContainer.Panel1.Controls.Add(_display);

            /// Adding numeric Buttons to ButtonList
            
            _buttonList = new List<Button>();

            _buttonList.Add(new Button()
            {
                Text = ".",
                Dock = DockStyle.Fill,
                Font = _fontStyle,
            });
            _buttonList.Add(new Button()
            {
                Text = "0",
                Dock = DockStyle.Fill,
                Font = _fontStyle,
            });
            _buttonList.Add(new Button()
            {
                Text = ButtonsName.EQUALS_KEY,
                Dock = DockStyle.Fill,
                Font = _fontStyle,
            });

            for(int numeric = 1; numeric <= 9; numeric++)
            {
                _buttonList.Add(new Button() { 
                    Text = numeric.ToString(),
                    Dock = DockStyle.Fill,
                    Font = _fontStyle,
                });
            }          

            /// Adding Buttons to ArithmeticPad

            _buttonList.Add(new Button()
            {
                Text = "(",
                Dock = DockStyle.Fill,
                Font = _fontStyle,
            });
            _buttonList.Add(new Button()
            {
                Text = ")",
                Dock = DockStyle.Fill,
                Font = _fontStyle,
            });
            foreach (var op in _evaluator.myOperators)
            {
                _buttonList.Add(new Button()
                {
                    Text = op.Value.symbol,
                    Dock = DockStyle.Fill,
                    Font = _fontStyle,
                });
            }
            _buttonList.Add(new Button()
            {
                Text = ButtonsName.MEMORY_SUBTRACTION,
                Dock = DockStyle.Fill,
                Font = _fontStyle,
            });
            _buttonList.Add(new Button()
            {
                Text = ButtonsName.MEMORY_ADD,
                Dock = DockStyle.Fill,
                Font = _fontStyle,
            });
            _buttonList.Add(new Button()
            {
                Text = ButtonsName.MEMORY_CLEAR,
                Dock = DockStyle.Fill,
                Font = _fontStyle,
            });
            _buttonList.Add(new Button()
            {
                Text = ButtonsName.MEMORY_STORE,
                Dock = DockStyle.Fill,
                Font = _fontStyle,
            });
            /// Adding Event Handler to _buttonList
            for (int i = 0; i < _buttonList.Count; i++)
            {
                _buttonList[i].Click += new EventHandler(btn_Click);
            }


            ///Table Layout
            _tableLayout = new TableLayoutPanel()
            {
                RowCount = 5,
                ColumnCount = (int)Math.Ceiling(Convert.ToDouble(_buttonList.Count / 4)),
                Dock = DockStyle.Fill,
            };
            for (int i = 0; i < _tableLayout.ColumnCount; i++)
            {
                _tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / _tableLayout.ColumnCount));
            }
            for (int i = 0; i < _tableLayout.RowCount; i++)
            {
                _tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / _tableLayout.RowCount));
            }
            _splitContainer.Panel2.Controls.Add(_tableLayout);

            /// Adding Clear, Delete and MemoryOperation buttons to top right of calculator
            _buttonClear = new Button()
            {
                Text = ButtonsName.CLEAR_KEY,
                Dock = DockStyle.Fill,
                Font = _fontStyle
            };
            _buttonClear.Click+= new EventHandler(btn_Click);


            _buttonDelete = new Button()
            {
                Text = ButtonsName.DELETE_KEY,
                Dock = DockStyle.Fill,
                Font = _fontStyle
            };
            _buttonDelete.Click += new EventHandler(btn_Click);

            _tableLayout.Controls.Add(_buttonDelete, _tableLayout.ColumnCount - 1, 0); 
            _tableLayout.Controls.Add(_buttonClear, _tableLayout.ColumnCount - 2, 0);

            /// Adding Buttons to table layout panel

            int count = 0;
            /// Adding Numerical keyPad
            for (int i = _tableLayout.RowCount - 1; i > 0; i--)
            {
                for (int j = _tableLayout.ColumnCount-3; j < _tableLayout.ColumnCount ; j++)
                {
                    if (count < _buttonList.Count)
                    {
                        _tableLayout.Controls.Add(_buttonList[count++], j, i);
                    }
                }
            }

            /// Adding Arithmetic Pad

            for (int i=_tableLayout.RowCount-1; i>0; i--)
            {
                for(int j = 0; j < _tableLayout.ColumnCount-3; j++)
                {
                    if (count < _buttonList.Count)
                    {
                        _tableLayout.Controls.Add(_buttonList[count++],j,i);
                    }
                }
            }

            /// MenuStrip
            _mainMenuStrip = new MenuStrip()
            {
                Dock = DockStyle.Top,
            };
            _editToolStripMenuItem = new ToolStripMenuItem()
            {
                Text = ButtonsName.EDIT_MENU,
            };
            _exitToolStripMenuItem = new ToolStripMenuItem()
            {
                Text = ButtonsName.EXIT_MENU
            };
            _helpToolStripMenuItem = new ToolStripMenuItem()
            {
                Text = ButtonsName.HELP_MENU
            };
            _exitToolStripMenuItem.Click+=new EventHandler(menu_Click);

            /// Adding Copy and Paste menu items
            _copy = _editToolStripMenuItem.DropDownItems.Add(ButtonsName.COPY_MENU_ITEM);
            _paste = _editToolStripMenuItem.DropDownItems.Add(ButtonsName.PASTE_MENU_ITEM);
            _copy.Click += new EventHandler(menuItem_Click);
            _paste.Click += new EventHandler(menuItem_Click);

            _mainMenuStrip.Items.Add(_editToolStripMenuItem);
            _mainMenuStrip.Items.Add(_exitToolStripMenuItem);
            _mainMenuStrip.Items.Add(_helpToolStripMenuItem);
            Controls.Add(_mainMenuStrip);

        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            
        }
        private void menu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.Text == ButtonsName.EXIT_MENU)
            {
                Close();
            }
        }
        private void menuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if(item.Text == ButtonsName.COPY_MENU_ITEM)
            {
                Clipboard.SetText(_display.Text);
            }
            if(item.Text == ButtonsName.PASTE_MENU_ITEM)
            {
                _display.Text = Clipboard.GetText();
            }
        }
        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == ButtonsName.EQUALS_KEY)
            {
                try
                {
                    _display.Text = Convert.ToString(_evaluator.Evaluate(_display.Text));
                    _lastButtonPressed.Clear();
                    _lastButtonPressed.Push(_display.Text);
                    _decimalPressed = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if(btn.Text == ButtonsName.MEMORY_STORE)
            {
                _memory = _display.Text;
            }
            else if (btn.Text == ButtonsName.MEMORY_CLEAR)
            {
                _memory = "";
            }
            else if (btn.Text == ButtonsName.MEMORY_ADD)
            {
                if (_memory == "")
                {
                    MessageBox.Show(ButtonsName.MEMORY_MESSAGE);
                }
                else _display.Text = "("+_memory+")"+_evaluator.myOperators["ADD_OPERATOR"].symbol+_display.Text;
            }
            else if (btn.Text == ButtonsName.MEMORY_SUBTRACTION)
            {
                if (_memory == "")
                {
                    MessageBox.Show(ButtonsName.MEMORY_MESSAGE);
                }
                else _display.Text = "(" + _memory + ")" + _evaluator.myOperators["SUBTRACT_OPERATOR"].symbol+ _display.Text;
            }
            else if(btn.Text == ".")
            {
                if (_lastButtonPressed.Count==0||(_lastButtonPressed.Peek()[0] < '0' || _lastButtonPressed.Peek()[0] > '9') && _decimalPressed==false)
                {
                    _display.Text += "0.";
                    _lastButtonPressed.Push("0.");
                    _decimalPressed = true;
                }
                else if (_decimalPressed == false)
                {
                    _display.Text += ".";
                    _lastButtonPressed.Push(".");
                    _decimalPressed = true;
                }
            }
            else if(btn.Text == ButtonsName.DELETE_KEY)
            {
                string lastBtn = "";
                if (_lastButtonPressed.Count() > 0)
                {
                    lastBtn = _lastButtonPressed.Pop();
                    _display.Text = _display.Text.Remove(_display.Text.Length - lastBtn.Length);
                    if(lastBtn[lastBtn.Length-1] == '.')
                    {
                        _decimalPressed=false;
                    }
                }
                else if(_display.Text.Length > 0)
                {
                    _display.Text = _display.Text.Remove(_display.Text.Length - 1);
                }

            }
            else if (btn.Text == ButtonsName.CLEAR_KEY)
            {
                _display.Text = "";
                _lastButtonPressed.Clear();
                _decimalPressed = false;
            }
            else
            {
                if (btn.Text[0] >= 'a' && btn.Text[0] <= 'z')
                {
                    _lastButtonPressed.Push(btn.Text + "(");
                    _display.Text += (btn.Text + "(");
                    _decimalPressed = false;
                }
                else
                {
                    if(btn.Text[0] < '0' || btn.Text[0] > '9') _decimalPressed = false;
                    _lastButtonPressed.Push(btn.Text);
                    _display.Text += (btn.Text);
                }
            }
        }
    }

}
