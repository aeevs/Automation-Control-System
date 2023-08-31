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
    public partial class Form0 : Form
    {
        public Form0()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int f = 0;
            Form000 frm000 = new Form000(f);
            frm000.Show();
            button1.Enabled = false;
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

        //проверка на дробные числа
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 127 && number != 44)
            {
                e.Handled = true;
            }
            if (e.KeyChar == ',' && textBox4.Text.Contains(","))
            {
                e.Handled = true; // Отменить событие, если запятая уже присутствует
            }
            if (char.IsDigit(e.KeyChar) && textBox4.Text.Contains(","))
            {
                int commaIndex = textBox4.Text.IndexOf(",");
                int selectionStart = textBox4.SelectionStart;
                // Отменить событие, если количество знаков после запятой превышает три
                if (selectionStart > commaIndex && textBox4.Text.Length - commaIndex > 2)
                {
                    e.Handled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string MBt1 = textBox1.Text;
            string MBt2 = textBox2.Text;
            string MBt3 = textBox3.Text;
            string MBt4 = textBox4.Text;
            string MBt5 = textBox5.Text;
            bool b1 = int.TryParse(MBt1, out int t1);
            bool b2 = int.TryParse(MBt2, out int t2);
            bool b3 = int.TryParse(MBt3, out int t3);
            bool b4 = decimal.TryParse(MBt4, out decimal t4);
            bool b5 = int.TryParse(MBt5, out int t5);
            if (!b1)
            {
                MessageBox.Show("Введите ЦЕЛОЕ ЧИСЛО поваров!");
                textBox1.Text = "";
            }
            else if (t1 <= 0)
            {
                MessageBox.Show("Введите ПОЛОЖИТЕЛЬНОЕ целое число поваров!");
                textBox1.Text = "";
            }
            else if (!b2)
            {
                MessageBox.Show("Введите ЦЕЛОЕ ЧИСЛО пицц на складе!");
                textBox2.Text = "";
            }
            else if (t2 <= 0)
            {
                MessageBox.Show("Введите ПОЛОЖИТЕЛЬНОЕ целое число пицц на складе!");
                textBox2.Text = "";
            }
            else if (!b3)
            {
                MessageBox.Show("Введите ЦЕЛОЕ ЧИСЛО курьеров!");
                textBox3.Text = "";
            }
            else if (t3 <= 0)
            {
                MessageBox.Show("Введите ПОЛОЖИТЕЛЬНОЕ целое число курьеров!");
                textBox3.Text = "";
            }
            else
            {
                if (t1 > 10)
                {
                    DialogResult dialogResult1 = MessageBox.Show("Кухней с таким количеством поваров будет тяжело управлять...\n" +
                        "Желаете продолжить?", "", MessageBoxButtons.YesNo);
                    if (dialogResult1 == DialogResult.No)
                    {
                        textBox1.Text = "";
                        return;
                    }
                }
                if (t2 > 10)
                {
                    DialogResult dialogResult2 = MessageBox.Show("Кажется, что слишком большой склад...\n" +
                        "Желаете продолжить?", "", MessageBoxButtons.YesNo);
                    if (dialogResult2 == DialogResult.No)
                    {
                        textBox2.Text = "";
                        return;
                    }
                }
                if (t3 > 10)
                {
                    DialogResult dialogResult3 = MessageBox.Show("Доставкой с таким количеством курьеров будет тяжело управлять...\n" +
                        "Желаете продолжить?", "", MessageBoxButtons.YesNo);
                    if (dialogResult3 == DialogResult.No)
                    {
                        textBox3.Text = "";
                        return;
                    }
                }
                //---
                if ((MBt4 == "") && (MBt5 == ""))
                {
                    DialogResult dialogResult3 = MessageBox.Show("Уверены, что НЕ желаете расчитать \"статистику\" заказов?\n\n" +
                        "(Нажав \"Да\" Вы подтверждаете, что НЕ сможете в дальнейшем взаимодействовать с параметрами заказа {так, например, все заказы будут бесплатными}!)", "", MessageBoxButtons.YesNo);
                    if (dialogResult3 == DialogResult.No)
                    {
                        MessageBox.Show("Тогда Вам следует заполить поля выкладки \"Заказ\"!!! :)");
                        return;
                    } else
                    {
                        foreach (Form f in Application.OpenForms)
                        {
                            f.Hide();
                        }
                        Form1 frm1 = new Form1(t1,t2,t3, -1, -1);
                        frm1.Show();
                    }
                }
                else
                {
                    if (!b4)
                    {
                        MessageBox.Show("Введите ВЕЩЕСТВЕННОЕ ЧИСЛО в поле, отвечающее за стоимость заказа!");
                        textBox4.Text = "";
                    }
                    else if (t4 == 0)
                    {
                        MessageBox.Show("Введите ПОЛОЖИТЕЛЬНОЕ вещественное число в поле, отвечающее за стоимость заказа!!");
                        textBox4.Text = "";
                    }
                    else if (!b5)
                    {
                        MessageBox.Show("Введите ЦЕЛОЕ ЧИСЛО в поле, отвечающее за гарантированное время доставки!");
                        textBox5.Text = "";
                    }
                    else if (t5 == 0)
                    {
                        MessageBox.Show("Введите ПОЛОЖИТЕЛЬНОЕ целое число в поле, отвечающее за гарантированное время доставки!");
                        textBox5.Text = "";
                    }
                    else
                    {
                        if (t4 > 1000)
                        {
                            DialogResult dialogResult3 = MessageBox.Show("Высокая стоимость заказа...\n" +
                                "Желаете продолжить?", "", MessageBoxButtons.YesNo);
                            if (dialogResult3 == DialogResult.No)
                            {
                                textBox4.Text = "";
                                return;
                            }
                        }
                        if (t5 > 90)
                        {
                            DialogResult dialogResult3 = MessageBox.Show("Большой промежуток времени для гарантированной доставки...\n" +
                                "Желаете продолжить?", "", MessageBoxButtons.YesNo);
                            if (dialogResult3 == DialogResult.No)
                            {
                                textBox5.Text = "";
                                return;
                            }
                        }
                        foreach (Form f in Application.OpenForms)
                        {
                            f.Hide();
                        }
                        Form1 frm1 = new Form1(t1, t2, t3, t4, t5);
                        frm1.Show();
                    }
                }
                //---
            }            
        }
    }
}
