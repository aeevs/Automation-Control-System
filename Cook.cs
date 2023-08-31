using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KYPCOBA9l
{
    //класс "повар"
    public class Cook
    {
        public double experience; //опыт
        public bool state; //состояние
        public int numberOfCookingOrder = 0; //количество приготовленных заказов
        //конструктор
        //(по умолчанию: опыт - "полгода", состояние - "готов к работе")
        public Cook()
        {
            experience = 0.5;
            state = true;
        }
        //логика "работы" повара
        public void CookingPizza(Order order, Storage storage)
        {
            if (state)
            {
                state = false; //принял заказ => занят
                order.status = "Начал готовиться"; 
                if (experience <= 1)
                {
                    Thread.Sleep(10000);
                }
                else if ((experience > 1) && (experience <= 3))
                {
                    Thread.Sleep(5000);
                }
                else
                {
                    Thread.Sleep(3000);
                }
                storage.Take(order);
                numberOfCookingOrder++;
                state = true; //выполнил заказ и передал на склад => свободен
            }
        }
    }
}
