using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace KYPCOBA9l
{
    public partial class Form21 : Form
    {
        public Form21()
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
                column1.HeaderText = "Опыт работы\n(количество лет работы)";
                column1.Name = "experience";
                column1.CellTemplate = new DataGridViewTextBoxCell();

                var column2 = new DataGridViewColumn();
                column2.HeaderText = "Текущее состояние";
                column2.Name = "state";
                column2.CellTemplate = new DataGridViewTextBoxCell();

                var column3 = new DataGridViewColumn();
                column3.HeaderText = "Всего приготовил за последний день";
                column3.Name = "AllInfo";
                column3.CellTemplate = new DataGridViewTextBoxCell();

                //запрещаем пользователю добавлять строки
                dataGridView1.AllowUserToAddRows = false;
                //-----

                //применение
                dataGridView1.Columns.Add(column0);
                dataGridView1.Columns.Add(column1);
                dataGridView1.Columns.Add(column2);
                dataGridView1.Columns.Add(column3);
                //-----
            }
            int i = 1;
            foreach (var cook in Form1.cooks.ToArray())
            {
                string state;
                if (cook.state)
                {
                    state = "Готов к работе";
                }
                else
                {
                    state = "Занят";
                }
                dataGridView1.Rows.Add(i.ToString(), cook.experience.ToString(), state, cook.numberOfCookingOrder.ToString());
                i++;
            }
        }

        //Кнопка "Добавить случайного повара!"
        private void button1_Click(object sender, EventArgs e)
        {
            if (Form1.IsWork)
            {
                MessageBox.Show("Завершите рабочий день, чтобы добавить нового случайного повара!");
            }
            else
            {
                Random rnd = new Random();
                Cook cook = new Cook();
                cook.experience = Math.Round(rnd.NextDouble() * (5 - 0.1) + 0.1, 3);
                Form1.cooks.Add(cook);
                MessageBox.Show("Успех!!!\nСлучайный повар добавлен!");
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

        //Кнопка "Удалить повара!"
        private void button2_Click(object sender, EventArgs e)
        {
            if (Form1.IsWork)
            {
                MessageBox.Show("Завершите рабочий день, чтобы удалить повара!");
            }
            else
            {
                string MBt = textBox1.Text;
                bool b = int.TryParse(MBt, out int t);
                if (!b)
                {
                    MessageBox.Show("Введите ЦЕЛОЕ ЧИСЛО, отвечающее за идентификатор повара!");
                    textBox1.Text = "";
                }
                else if (t<=0)
                {
                    MessageBox.Show("Введите ПОЛОЖИТЕЛЬНОЕ целое число!\nИдентификатор не может быть меньше нуля или равняться ему!");
                    textBox1.Text = "";
                }
                else if (t > Form1.cooks.Count)
                {
                    MessageBox.Show("Выход за пределы списка!\nПовара с таким идентификатором не обнаружено!");
                    textBox1.Text = "";
                }
                else
                {
                    Form1.cooks.RemoveAt(t-1);
                    MessageBox.Show("Успех!!!\nПовар с порядковым номером \""+(t).ToString()+"\" больше не работает!");
                    textBox1.Text = "";
                }
            }
        }

        //проверка на дробные числа
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 127 && number != 44)
            {
                e.Handled = true;
            }
            if (e.KeyChar == ',' && textBox2.Text.Contains(","))
            {
                e.Handled = true; // Отменить событие, если запятая уже присутствует
            }
            if (char.IsDigit(e.KeyChar) && textBox2.Text.Contains(","))
            {
                int commaIndex = textBox2.Text.IndexOf(",");
                int selectionStart = textBox2.SelectionStart;
                // Отменить событие, если количество знаков после запятой превышает три
                if (selectionStart > commaIndex && textBox2.Text.Length - commaIndex > 3)
                {
                    e.Handled = true;
                }
            }
        }

        //Кнопка "Добавить повара!"
        private void button3_Click(object sender, EventArgs e)
        {
            if (Form1.IsWork)
            {
                MessageBox.Show("Завершите рабочий день, чтобы добавить нового повара!");
            }
            else
            {
                string MBt = textBox2.Text;
                bool b = decimal.TryParse(MBt, out decimal t);
                if (!b)
                {
                    MessageBox.Show("Введите ВЕЩЕСТВЕННОЕ ЧИСЛО, отвечающее за опыт работы повара!");
                    textBox2.Text = "";
                }
                else if (t < 0)
                {
                    MessageBox.Show("Введите НЕОТРИЦАТЕЛЬНОЕ вещественное число, отвечающее за опыт работы повара!");
                    textBox2.Text = "";
                }
                else if (t > 5)
                {
                    if (t > 80)
                    {
                        MessageBox.Show("Ошибка! Слишком большой опыт работы!");
                        textBox2.Text = "";
                    } else
                    {
                        DialogResult dialogResult1 = MessageBox.Show("Вы ввели достаточно большой опыт работы...\n" +
                            "Желаете продолжить?", "", MessageBoxButtons.YesNo);
                        if (dialogResult1 == DialogResult.No)
                        {
                            textBox2.Text = "";
                        }
                        else
                        {
                            Cook cook = new Cook();
                            cook.experience = decimal.ToDouble(t);
                            Form1.cooks.Add(cook);
                            textBox2.Text = "";
                            MessageBox.Show("Успех!!!\nПовар добавлен!");
                        }
                    }
                }
                else
                {
                    Cook cook = new Cook();
                    cook.experience = decimal.ToDouble(t);
                    Form1.cooks.Add(cook);
                    textBox2.Text = "";
                    MessageBox.Show("Успех!!!\nПовар добавлен!");
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
            frm.button2.Enabled = true;
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
            frm.button2.Enabled = true;
            this.Hide();
        }
    }
}
