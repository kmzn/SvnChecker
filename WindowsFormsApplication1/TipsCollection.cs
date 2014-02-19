using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


class TipsCollection
{
    static System.Windows.Forms.Label label;
    static private Dictionary<string, string> tips = new Dictionary<string, string>();

    static public void Initialize(System.Windows.Forms.Label l)
    {
        label = l;
        tips["AddButton"] = "Add New URL";
    }
    static public void MouseHover(object sender, EventArgs e)
    {
        label.Text = tips[((Control)sender).Name];
    }
}

