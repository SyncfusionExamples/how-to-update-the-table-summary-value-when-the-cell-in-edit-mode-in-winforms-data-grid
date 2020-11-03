using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Update_Live
{
    public class OrderInfo : INotifyPropertyChanged
    {
        int orderID;
        string customerId;
        string country;
        string customerName;
        string shippingCity;
        int unitPrice;

        public int OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }
        public string CustomerID
        {
            get { return customerId; }
            set { customerId = value; }
        }
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        public string ShipCity
        {
            get { return shippingCity; }
            set { shippingCity = value; }
        }

        public int UnitPrice
        {
            get
            {
                return unitPrice;
            }
            set
            {
                unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public OrderInfo(int orderId, string customerName, string country, string customerId, string shipCity, int unitPrice)
        {
            this.OrderID = orderId;
            this.CustomerName = customerName;
            this.Country = country;
            this.CustomerID = customerId;
            this.ShipCity = shipCity;
            this.UnitPrice = unitPrice;
        }
    }
}
