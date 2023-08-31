using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KYPCOBA9l
{
    //класс "заказ"
    public class Order
    {
        public int id { get;  set; } //идентификатор
        public string status { get; set; } //статус
        public int time = 0; //время выполнения
        //конструтор (по умолчанию: статус - "принят")
        public Order()
        {
            status = "Принят";
        }
    }
}
