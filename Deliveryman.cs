using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KYPCOBA9l
{
    //класс "доставщик"
    public class Deliveryman
    {
        public int size; //"размер" багажника
        public List<Order> deliveryOrder = new List<Order>(); //заказы у курьера
        public int currentNumberOfDeliveryOrder = 0; //число доставленных заказов из списка текущих
        public int numberOfDeliveryOrder = 0; //общее число доставленных заказов
        public bool state; //состояние
        //конструктор
        //(по умолчанию: багажник - 2 пиццы, состояние - "готов к работе")
        public Deliveryman()
        {
            size = 2;
            state = true;
        }
        //логика "работы" доставщика
        public void DeliverPizza(Storage storage)
        {
            if (state)
            {
                state = false;
                //пытаемся взять как можно больше заказов со склада
                foreach (var order in storage.stockOrders.ToArray())
                {
                    if ((order.status == "На складе") && (deliveryOrder.Count < size))
                    {
                        order.status = "В доставке";
                        deliveryOrder.Add(order);
                        storage.Give(order);
                    }
                }
                //...
                if (deliveryOrder.Count > 0)
                {
                    foreach (var order in deliveryOrder)
                    {
                        Random rnd = new Random();
                        Thread.Sleep(rnd.Next(5000, 15000));
                        order.status = "Доставлен";
                        currentNumberOfDeliveryOrder++;
                        numberOfDeliveryOrder++;
                    }
                    deliveryOrder.Clear();
                    currentNumberOfDeliveryOrder = 0;
                }
                state = true;
            }
        }
    }
}
