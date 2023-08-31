using static System.Windows.Forms.Timer;

namespace KYPCOBA9l
{
    public partial class Form1 : Form
    {
        public Form1(int t1, int t2, int t3, decimal t4, int t5)
        {
            InitializeComponent();

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

            //применение
            dataGridView1.Columns.Add(column1);
            dataGridView1.Columns.Add(column2);
            dataGridView1.Columns.Add(column3);

            //запрещаем пользователю добавлять строки
            dataGridView1.AllowUserToAddRows = false;
            //-----

            //-----
            //Описываем таймеры
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 10;//Интервал времени
            timer1.Tick += new EventHandler(timer1_Tick);

            timer2 = new System.Windows.Forms.Timer();
            timer2.Interval = 20000;//Интервал времени
            //timer2.Tick += new EventHandler(timer2_Tick);

            timer3 = new System.Windows.Forms.Timer();
            timer3.Interval = 1000;//Интервал времени
            timer3.Tick += new EventHandler(timer3_Tick);
            //-----

            //-----
            //инициализация списков данными из предыдущих форм
            Random rnd = new Random();
            for (int i = 0; i<t1; i++)
            {
                Cook cook = new Cook();
                cook.experience = Math.Round(rnd.NextDouble() * (5 - 0.1) + 0.1, 3);
                cooks.Add(cook);
            }
            storage.size = t2;
            for (int i = 0; i < t3; i++)
            {
                Deliveryman deliveryman = new Deliveryman();
                deliveryman.size = rnd.Next(2,5);
                deliverymans.Add(deliveryman);
            }
            price = t4;
            time = t5;
            //------
            if (t4 == -1)
            {
                ExtraFreez();
                label7.Text = "Текущая фиксированная стоимость заказа\n(цена одной пиццы)(руб.): бесплатно";
                label8.Text = "Гарантированное время доставки (c): ---";
            }
            else
            {
                label7.Text = "Текущая фиксированная стоимость заказа\n(цена одной пиццы)(руб.): " + price.ToString();
                label8.Text = "Гарантированное время доставки (c): " + time.ToString();
            }
            //------
            //-----
            label3.Text = label3.Text + " " + t2.ToString() + " " + "пицц(ы)";
            Freez();
            //-----
        }

        //freez
        public void Freez()
        {
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button1.Enabled = false;
        }

        //unfreez
        public void Unfreez()
        {
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button1.Enabled = true;
        }

        //-----
        public void ExtraFreez()
        {
            button11.Enabled = false;
            button12.Enabled = false;
            textBox1.Enabled = false;
            textBox4.Enabled = false;
        }
        public void ExtraUnfreez()
        {
            button11.Enabled = true;
            button12.Enabled = true;
            textBox1.Enabled = true;
            textBox4.Enabled = true;
        }
        //-----

        //таймеры
        System.Windows.Forms.Timer timer1;
        System.Windows.Forms.Timer timer2;
        System.Windows.Forms.Timer timer3;

        //список поваров
        static public List<Cook> cooks = new List<Cook>();
        //список доставщиков
        static public List<Deliveryman> deliverymans = new List<Deliveryman>();
        //склад
        static public Storage storage = new Storage();

        static public bool IsWork = false;
        static public decimal price;
        static public int time;
        static public int day;

        //список заказов
        static public List<Order> orders = new List<Order>();
        int id = 1;

        // создаёт заказ
        private void button1_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.id = id;
            orders.Add(newOrder);
            id = id + 1;
        }

        //ПОВАРА
        async Task MyM1()
        {   
            foreach (var cook in cooks)
            {
                if (cook.state)
                {
                    foreach (var order in orders)
                    {
                        if (order.status == "Принят")
                        {
                            await Task.Run(() => cook.CookingPizza(order, storage)); //готовить
                        }
                    }
                }
            }  
        }

        //КУРЬЕРЫ
        async Task MyM2()
        {
            foreach (var deliveryman in deliverymans)
            {
                if (deliveryman.state)
                {
                    await Task.Run(() => deliveryman.DeliverPizza(storage)); //доставить
                }
            }
        }

        //метод, запускающий работу системы (работает каждую милисекунду)
        private void timer1_Tick(object sender, EventArgs e)
        {
            MyM1(); MyM2();
            dataGridView1.Rows.Clear();
            foreach (var order in orders.ToArray())
            {
                dataGridView1.Rows.Add(order.id, order.status, order.time);
            }
            if (dataGridView1.RowCount != 0)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
            }
        }

        /*
        //сборщик мусора => удаляет готовые заказы из общего списка (работает каждые 20 секунд)
        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int i = orders.Count - 1; i >= 0; i--)
            {
                if (orders[i].status == "Доставлен") orders.RemoveAt(i);
            }
        }
        */

        //подсчёт времени выполнения заказов
        private void timer3_Tick(object sender, EventArgs e)
        {
            for (int i = orders.Count - 1; i >= 0; i--)
            {
                if (orders[i].status != "Доставлен") orders[i].time++;
            }
        }


        //Кнопка "НАЧАТЬ рабочий день" / "ЗАВЕРШИТЬ рабочий день!"
        private void button8_Click(object sender, EventArgs e)
        {
            if (button8.Text == "ЗАВЕРШИТЬ\nрабочий день!")
            {
                bool f = false;
                for (int i = orders.Count - 1; i >= 0; i--)
                {
                    if (orders[i].status != "Доставлен") f = true; break;
                }
                if (f)
                {
                    MessageBox.Show("Вы не можете завершить день пока у вас есть активные заказы!");
                }
                else
                {
                    IsWork = false;
                    timer1.Stop();
                    timer2.Stop();
                    timer3.Stop();
                    //-----
                    if (price != -1)
                    {
                        ExtraUnfreez();
                    }
                    //-----
                    button8.Text = "НАЧАТЬ рабочий день!";
                    Freez();
                    //--------------------
                    //БЛОК АНАЛИТИКИ
                    if (orders.Count != 0)
                    { 
                        day++;
                        //0)заказы
                        string substr = "";
                        if (price != -1)
                        {
                            int allO = orders.Count;
                            int freeO = 0;
                            foreach (var order in orders)
                            {
                                if (order.time > time)
                                {
                                    freeO++;
                                }
                            }
                            substr = "Выполнено заказов: " + allO.ToString() + "\n" +
                            "Из них бесплатно: " + freeO.ToString() + "\n" +
                            "Общая выручка составила " + (price * (allO - freeO)).ToString() + " рубля(ей)!\n\n";
                        }
                        //1)повара
                        Cook cuurentCook_max = cooks[0];
                        Cook cuurentCook_min = cooks[0];
                        int c_max = cooks[0].numberOfCookingOrder;
                        int c_min = cooks[0].numberOfCookingOrder;
                        int c_max_id = 1;
                        int c_min_id = 1;
                        if (cooks.Count > 1)
                        {
                            int i = 1;
                            foreach (var cook in cooks)
                            {
                                if (cook.numberOfCookingOrder > c_max)
                                {
                                    cuurentCook_max = cooks[i-1];
                                    c_max = cook.numberOfCookingOrder;
                                    c_max_id = i;
                                }
                                //при прочих равных смотрим, чтобы опыт был меньше
                                else if (cook.numberOfCookingOrder == c_max)
                                {
                                    if (cook.experience < cuurentCook_max.experience)
                                    {
                                        cuurentCook_max = cooks[i-1];
                                        c_max = cook.numberOfCookingOrder;
                                        c_max_id = i;
                                    }
                                }
                                if (cook.numberOfCookingOrder < c_min)
                                {
                                    cuurentCook_min = cooks[i-1];
                                    c_min = cook.numberOfCookingOrder;
                                    c_min_id = i;
                                }
                                //при прочих равных смотрим, чтобы опыт был больше
                                else if (cook.numberOfCookingOrder == c_min)
                                {
                                    if (cook.experience > cuurentCook_min.experience)
                                    {
                                        cuurentCook_min = cooks[i-1];
                                        c_min = cook.numberOfCookingOrder;
                                        c_min_id = i;
                                    }
                                }
                                i++;
                            }
                        }
                        //2)курьеры
                        int d_max = cooks[0].numberOfCookingOrder;
                        int d_min = cooks[0].numberOfCookingOrder;
                        int d_max_id = 1;
                        int d_min_id = 1;
                        if (deliverymans.Count > 1)
                        {
                            int i = 1;
                            foreach (var deliveryman in deliverymans)
                            {
                                if (deliveryman.numberOfDeliveryOrder > d_max)
                                {
                                    d_max = deliveryman.numberOfDeliveryOrder;
                                    d_max_id = i;
                                }
                                if (deliveryman.numberOfDeliveryOrder <= d_min)
                                {
                                    d_min = deliveryman.numberOfDeliveryOrder;
                                    d_min_id = i;
                                }
                                i++;
                            }
                        }
                        //3)склад {flag}
                        string sub = "";
                        if (Form32.flag)
                        {
                            sub = "В какой-то момент склад был полностью заполнен!\n" +
                                "(РЕКОМЕНДУЕМ! его увеличить...)";
                        }
                        MessageBox.Show("За сегодняшний день:\n\n" +
                            substr +
                            "Лучший повар под номером \"" + c_max_id.ToString() + "\" (" + c_max + " заказа(ов))!\n" +
                            "Худший повар под номером \"" + c_min_id.ToString() + "\" (" + c_min + " заказа(ов))!\n\n" +
                            "Лучший курьер под номером \"" + d_max_id.ToString() + "\" (" + d_max + " заказа(ов))!\n" +
                            "Худший курьер под номером \"" + d_min_id.ToString() + "\" (" + d_min + " заказа(ов))!\n\n" +
                            sub, 
                            "Добро пожаловать в аналитику! " + "День " + day.ToString());
                        //ОПЫТ ПОВАРАМ
                        foreach (var cook in cooks)
                        {
                            if (cook.numberOfCookingOrder != 0)
                            {
                                cook.experience = Math.Round(cook.experience+0.003, 3);
                            }
                        }
                    }
                    //--------------------
                    //---
                    dataGridView1.Rows.Clear(); //очистил таблицу
                    //---
                }
            }
            else
            {
                if (cooks.Count == 0)
                {
                    MessageBox.Show("Невозможно начать рабочий день без бригады поваров!!!");
                }
                else if (deliverymans.Count == 0)
                {
                    MessageBox.Show("Невозможно начать рабочий день без команды курьеров!!!");
                }
                else
                {
                    //---
                    orders.Clear(); //очистил оставщиеся доставленные заказы, если такие не были удалены сборщиком мусора
                    //чищу подсчёт заказов у курьеров
                    foreach (var deliveryman in deliverymans)
                    {
                        deliveryman.numberOfDeliveryOrder = 0;
                    }
                    //чищу подсчёт заказов у поваров
                    foreach (var cook in cooks)
                    {
                        cook.numberOfCookingOrder = 0;
                    }
                    Form32.flag = false;
                    //---
                    //-----
                    if (price != -1)
                    {
                        ExtraFreez();
                    }
                    //-----
                    IsWork = true;
                    timer1.Start();
                    timer2.Start();
                    timer3.Start();
                    button8.Text = "ЗАВЕРШИТЬ\nрабочий день!";
                    Unfreez();
                }
            }
        }

        //Кнопка "НА КУХНЕ"
        private void button2_Click(object sender, EventArgs e)
        {
            Form21 frm21 = new Form21();
            frm21.Show();
            button2.Enabled = false;
        }

        //Кнопка "В ДОСТАВКЕ"
        private void button3_Click(object sender, EventArgs e)
        {
            Form22 frm22 = new Form22();
            frm22.Show();
            button3.Enabled = false;
        }

        //↑
        private void button9_Click(object sender, EventArgs e)
        {
            if (IsWork)
            {
                MessageBox.Show("Завершите рабочий день, чтобы увеличить склад!");
            }
            else
            {
                if (storage.size >= 9)
                {
                    DialogResult dialogResult = MessageBox.Show("Кажется, что слишком большой склад...\n" +
                            "Желаете продолжить?", "", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        storage.size++;
                        label3.Text = "Ваш склад вмещет: " + storage.size.ToString() + " пицц(ы)";

                    }
                }
                else
                {
                    storage.size++;
                    label3.Text = "Ваш склад вмещет: " + storage.size.ToString() + " пицц(ы)";
                }
            }
        }

        //↓
        private void button10_Click(object sender, EventArgs e)
        {
            if (IsWork)
            {
                MessageBox.Show("Завершите рабочий день, чтобы уменьшить склад!");
            }
            else
            {
                if (storage.size == 1)
                {
                    MessageBox.Show("Размер склада не может быть равен нулю!");
                }
                else
                {
                    storage.size--;
                    label3.Text = "Ваш склад вмещет: " + storage.size.ToString() + " пицц(ы)";
                }
            }
        }

        //ПРАВИЛА
        private void button7_Click(object sender, EventArgs e)
        {
            int f = 1;
            Form000 frm000 = new Form000(f);
            frm000.Show();
            button7.Enabled = false;
        }

        //СИСТЕМЫ УЧЁТА ЗАКАЗОВ
        //КУХНИ
        private void button4_Click(object sender, EventArgs e)
        {
            Form31 frm31 = new Form31();
            frm31.Show();
            button4.Enabled = false;
        }
        //СКЛАДА
        private void button5_Click(object sender, EventArgs e)
        {
            Form32 frm32 = new Form32();
            frm32.Show();
            button5.Enabled = false;
        }
        //КУРЬЕРОВ
        private void button6_Click(object sender, EventArgs e)
        {
            Form33 frm33 = new Form33();
            frm33.Show();
            button6.Enabled = false;
        }

        //Закрытие формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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

        //ОБНОВИТЬ (price)
        private void button11_Click(object sender, EventArgs e)
        {
            if (IsWork)
            {
                MessageBox.Show("Завершите рабочий день, чтобы обновить стоимость заказа!");
            }
            else
            {
                string MBt4 = textBox4.Text;
                bool b4 = decimal.TryParse(MBt4, out decimal t4);
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
                    price = t4;
                    label7.Text = "Текущая фиксированная стоимость заказа\n(цена одной пиццы)(руб.): " + price.ToString();
                    textBox4.Text = "";
                    MessageBox.Show("Стоимость заказа успешно обновлена!");
                }
            }
        }

        //ОБНОВИТЬ (time)
        private void button12_Click(object sender, EventArgs e)
        {
            if (IsWork)
            {
                MessageBox.Show("Завершите рабочий день, чтобы обновить гарантированное время доставки!");
            }
            else
            {
                string MBt5 = textBox1.Text;
                bool b5 = int.TryParse(MBt5, out int t5);
                if (!b5)
                {
                    MessageBox.Show("Введите ЦЕЛОЕ ЧИСЛО в поле, отвечающее за гарантированное время доставки!");
                    textBox1.Text = "";
                }
                else if (t5 == 0)
                {
                    MessageBox.Show("Введите ПОЛОЖИТЕЛЬНОЕ целое число в поле, отвечающее за гарантированное время доставки!");
                    textBox1.Text = "";
                }
                else
                {
                    if (t5 > 90)
                    {
                        DialogResult dialogResult3 = MessageBox.Show("Большой промежуток времени для гарантированной доставки...\n" +
                            "Желаете продолжить?", "", MessageBoxButtons.YesNo);
                        if (dialogResult3 == DialogResult.No)
                        {
                            textBox1.Text = "";
                            return;
                        }
                    }
                    time = t5;
                    label8.Text = "Гарантированное время доставки (c): " + time.ToString();
                    textBox1.Text = "";
                    MessageBox.Show("Гарантированное время доставки успешно обновлено!");
                }
            }
        }
    }
}