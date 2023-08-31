namespace KYPCOBA9l
{
    //класс "склад"
    public class Storage
    {
        public int size; //размер
        public List<Order> stockOrders = new List<Order>(); //заказы на складе
        //конструкторы
        //(по умолчанию: размер - 3 пиццы)
        public Storage()
        {
            size = 5;
        }
        public Storage(int size)
        {
            this.size = size;
        }
        //"на складе"
        public void Take(Order order)
        {
            order.status = "Ожидает место на складе";
            l1:
            if (stockOrders.Count < size)
            {
                order.status = "На складе";
                stockOrders.Add(order);
            }
            else
            {
                goto l1;
            }
        }

        //"в доставке"
        public void Give(Order order)
        {
            Order currentOrder = stockOrders.Find(x => x.id == order.id);
            stockOrders.Remove(currentOrder);
        }
    }
}
