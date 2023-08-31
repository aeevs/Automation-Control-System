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
    public partial class Form22 : Form
    {
        public Form22()
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
                var column0 = new DataGridViewColumn();
                column0.HeaderText = "id";
                column0.Name = "id";
                column0.CellTemplate = new DataGridViewTextBoxCell();

                var column1 = new DataGridViewColumn();
                column1.HeaderText = "Объём багажника (кол-во пицц)";
                column1.Name = "size";
                column1.CellTemplate = new DataGridViewTextBoxCell();

                var column2 = new DataGridViewColumn();
                column2.HeaderText = "Текущее состояние";
                column2.Name = "state";
                column2.CellTemplate = new DataGridViewTextBoxCell();

                var column3 = new DataGridViewColumn();
                column3.HeaderText = "Текущий заказ\n" +
                    "Взял/Доставил";
                column3.Name = "currentInfo";
                column3.CellTemplate = new DataGridViewTextBoxCell();

                var column4 = new DataGridViewColumn();
                column4.HeaderText = "Всего доставил за последний день";
                column4.Name = "AllInfo";
                column4.CellTemplate = new DataGridViewTextBoxCell();

                //запрещаем пользователю добавлять строки
                dataGridView1.AllowUserToAddRows = false;
                //-----

                //применение
                dataGridView1.Columns.Add(column0);
                dataGridView1.Columns.Add(column1);
                dataGridView1.Columns.Add(column2);
                dataGridView1.Columns.Add(column3);
                dataGridView1.Columns.Add(column4);
                //-----
            }
            int i = 1;
            foreach (var deliveryman in Form1.deliverymans.ToArray())
            {
                string state;
                if (deliveryman.state)
                {
                    state = "Готов к работе";
                    dataGridView1.Rows.Add(i.ToString(), deliveryman.size.ToString(), state, "", deliveryman.numberOfDeliveryOrder.ToString());
                }
                else
                {
                    state = "Занят";
                    dataGridView1.Rows.Add(i.ToString(), deliveryman.size.ToString(), state, deliveryman.currentNumberOfDeliveryOrder.ToString() + "/" + deliveryman.deliveryOrder.Count.ToString(), deliveryman.numberOfDeliveryOrder.ToString());
                }
                i++;
            }
        }

        //Кнопка "Добавить случайного курьера!"
        private void button1_Click(object sender, EventArgs e)
        {
            if (Form1.IsWork)
            {
                MessageBox.Show("Завершите рабочий день, чтобы добавить нового случайного курьера!");
            }
            else
            {
                Random rnd = new Random();
                Deliveryman deliveryman = new Deliveryman();
                deliveryman.size = rnd.Next(2, 5);
                Form1.deliverymans.Add(deliveryman);
            }
        }

        //проверка на числа
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 127)
            {
                e.Handled = true;
            }
            //разработка для нулей
            var textBox = (TextBox)sender;
            textBox.Text = textBox.Text.TrimStart('0');
        }

        //Кнопка "Удалить курьера!"
        private void button2_Click(object sender, EventArgs e)
        {
            if (Form1.IsWork)
            {
                MessageBox.Show("Завершите рабочий день, чтобы удалить курьера!");
            }
            else
            {
                string MBt = textBox1.Text;
                bool b = int.TryParse(MBt, out int t);
                if (!b)
                {
                    MessageBox.Show("Введите ЦЕЛОЕ ЧИСЛО, отвечающее за идентификатор курьера!");
                    textBox1.Text = "";
                }
                else if (t <= 0)
                {
                    MessageBox.Show("Введите ПОЛОЖИТЕЛЬНОЕ целое число!\nИдентификатор не может быть меньше нуля или равняться ему!");
                    textBox1.Text = "";
                }
                else if (t > Form1.cooks.Count)
                {
                    MessageBox.Show("Выход за пределы списка!\nКурьера с таким идентификатором не обнаружено!");
                    textBox1.Text = "";
                }
                else
                {
                    Form1.deliverymans.RemoveAt(t - 1);
                    MessageBox.Show("Успех!!!\nКурьер с порядковым номером \"" + (t).ToString() + "\" больше не работает!");
                    textBox1.Text = "";
                }
            }
        }

        //Кнопка "Добавить курьера!"
        private void button3_Click(object sender, EventArgs e)
        {
            if (Form1.IsWork)
            {
                MessageBox.Show("Завершите рабочий день, чтобы добавить нового курьера!");
            }
            else
            {
                string MBt = textBox2.Text;
                bool b = int.TryParse(MBt, out int t);
                if (!b)
                {
                    MessageBox.Show("Введите ЦЕЛОЕ ЧИСЛО, отвечающее за объем багажника курьера!");
                    textBox2.Text = "";
                }
                else if (t == 0)
                {
                    MessageBox.Show("Введите НЕОТРИЦАТЕЛЬНОЕ целое число, отвечающее за объем багажника курьера!");
                    textBox2.Text = "";
                }
                else if (t > 5)
                {
                    if (t > 25)
                    {
                        MessageBox.Show("Ошибка! Слишком большой объем багажника!");
                        textBox2.Text = "";
                    }
                    else
                    {
                        DialogResult dialogResult1 = MessageBox.Show("Вы ввели достаточно большой объем багажника...\n" +
                            "Желаете продолжить?", "", MessageBoxButtons.YesNo);
                        if (dialogResult1 == DialogResult.No)
                        {
                            textBox2.Text = "";
                        }
                        else
                        {
                            Deliveryman deliveryman = new Deliveryman();
                            deliveryman.size = t;
                            Form1.deliverymans.Add(deliveryman);
                            textBox2.Text = "";
                            MessageBox.Show("Успех!!!\nКурьер добавлен!");
                        }
                    }
                }
                else
                {
                    Deliveryman deliveryman = new Deliveryman();
                    deliveryman.size = t;
                    Form1.deliverymans.Add(deliveryman);
                    textBox2.Text = "";
                    MessageBox.Show("Успех!!!\nКурьер добавлен!");
                }
            }
        }

        //Закрытие формы
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((textBox1.Text != "") || (textBox2.Text != ""))
            {
                DialogResult dialogResult3 = MessageBox.Show("Есть заполненные поля...\n" +
                            "Уверены, что хотите выйти?", "", MessageBoxButtons.YesNo);
                if (dialogResult3 == DialogResult.No)
                {
                    return;
                }
            }
            textBox1.Text = ""; textBox2.Text = "";
            Form1 frm = (Form1)Application.OpenForms["Form1"];
            frm.button3.Enabled = true;
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "") || (textBox2.Text != ""))
            {
                DialogResult dialogResult3 = MessageBox.Show("Есть заполненные поля...\n" +
                            "Уверены, что хотите выйти?", "", MessageBoxButtons.YesNo);
                if (dialogResult3 == DialogResult.No)
                {
                    return;
                }
            }
            textBox1.Text = ""; textBox2.Text = "";
            Form1 frm = (Form1)Application.OpenForms["Form1"];
            frm.button3.Enabled = true;
            this.Hide();
        }
    }
}
