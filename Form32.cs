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
    public partial class Form32 : Form
    {
        public Form32()
        {
            InitializeComponent();

            //-----
            //Описываем таймер
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 10;//Интервал времени
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
            //-----
        }

        public static bool flag;

        //таймер
        System.Windows.Forms.Timer timer1;

        //метод, запускающий работу системы (работает каждую милисекунду)
        private void timer1_Tick(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (dataGridView1.Columns.Count < 3)
            {
                //-----
                //Создаём шапку таблицы
                //создание столбцов таблицы
                var column1 = new DataGridViewColumn();
                column1.HeaderText = "ID";
                column1.Name = "id";
                column1.CellTemplate = new DataGridViewTextBoxCell();

                var column2 = new DataGridViewColumn();
                column2.HeaderText = "Состояние заказа";
                column2.Name = "status";
                column2.CellTemplate = new DataGridViewTextBoxCell();

                var column3 = new DataGridViewColumn();
                column3.HeaderText = "Время выполнения (сек)";
                column3.Name = "time";
                column3.CellTemplate = new DataGridViewTextBoxCell();

                //запрещаем пользователю добавлять строки
                dataGridView1.AllowUserToAddRows = false;
                //-----

                //применение
                dataGridView1.Columns.Add(column1);
                dataGridView1.Columns.Add(column2);
                dataGridView1.Columns.Add(column3);
                //-----
            }
            int i = 0;
            foreach (var order in Form1.storage.stockOrders.ToArray())
            {
                i++;
                
                dataGridView1.Rows.Add(order.id, order.status, order.time);
            }
            if (Form1.storage.size == i)
            {
                flag = true;
            }
            label3.Text = Form1.storage.size.ToString() + " заказ(ов)";
            label4.Text = i.ToString() + " заказ(ов)";
        }

        //Закрытие формы
        private void Form32_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Form1.IsWork)
            {
                Form1 frm = (Form1)Application.OpenForms["Form1"];
                frm.button5.Enabled = true;
            }
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Form1.IsWork)
            {
                Form1 frm = (Form1)Application.OpenForms["Form1"];
                frm.button5.Enabled = true;
            }
            this.Hide();
        }
    }
}
