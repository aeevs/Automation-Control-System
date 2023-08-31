using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KYPCOBA9l
{
    public partial class Form000 : Form
    {
        public Form000(int f)
        {
            InitializeComponent();
            this.f = f;
        }

        int f;

        //Закрытие формы
        private void Form000_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f == 0)
            {
                Form0 frm0 = (Form0)Application.OpenForms[0]; //ВНИМАНИЕ! "костыль"...
                frm0.button1.Enabled = true;
            } 
            else if (f == 1)
            {
                Form1 frm1 = (Form1)Application.OpenForms["Form1"];
                frm1.button7.Enabled = true;
            }
            this.Hide();
        }

        //НАЗАД
        private void button4_Click(object sender, EventArgs e)
        {
            if (f == 0)
            {
                Form0 frm0 = (Form0)Application.OpenForms[0]; //ВНИМАНИЕ! "костыль"...
                frm0.button1.Enabled = true;
            }
            else if (f == 1)
            {
                Form1 frm1 = (Form1)Application.OpenForms["Form1"];
                frm1.button7.Enabled = true;
            }
            this.Hide();
        }
    }
}
