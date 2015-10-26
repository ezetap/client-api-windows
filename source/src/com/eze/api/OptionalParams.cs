using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.eze.api
{
    public class OptionalParams
    {
        double amountCashback;
        double amountTip;
        Reference reference;
        Customer customer;

        public double getAmountCashback()
        {
            return amountCashback;
        }
        public double getAmountTip()
        {
            return amountTip;
        }
        public void setAmountCashback(double amountCashback)
        {
            this.amountCashback = amountCashback;
        }
        public void setAmountTip(double amountTip)
        {
            this.amountTip = amountTip;
        }
        public Reference getReference()
        {
            return reference;
        }
        public void setReference(Reference reference)
        {
            this.reference = reference;
        }
        public Customer getCustomer()
        {
            return customer;
        }
        public void setCustomer(Customer customer)
        {
            this.customer = customer;
        }

    }
}
