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
    public partial class Form31 : Form
    {
        public Form31()
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
            int i = 0, j = 0, k = 0;
            foreach (var order in Form1.orders.ToArray())
            {
                if (order.status == "Принят")
                {
                    i++;
                    dataGridView1.Rows.Add(order.id, order.status, order.time);
                }
                else if (order.status == "Начал готовиться")
                {
                    j++;
                    dataGridView1.Rows.Add(order.id, order.status, order.time);
                }
                else if (order.status == "Ожидает место на складе")
                {
                    k++;
                    dataGridView1.Rows.Add(order.id, order.status, order.time);
                }
            }
            label2.Text = "Принято заказов (число пицц, ожидающих приготовления): " + i.ToString();
            label3.Text = "Готовятся: " + j.ToString();
            label4.Text = "Готовы и ожидают места на складе: " + k.ToString();
        }

        //Закрытие формы
        private void Form31_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Form1.IsWork)
            {
                Form1 frm = (Form1)Application.OpenForms["Form1"];
                frm.button4.Enabled = true;
            }
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Form1.IsWork)
            {
                Form1 frm = (Form1)Application.OpenForms["Form1"];
                frm.button4.Enabled = true;
            }
            this.Hide();
        }
    }
}
