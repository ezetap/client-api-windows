using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.eze.api
{
    public class Receipt
    {
        String receiptDate;
        String receiptUrl;

        public void setReceiptDate(String receiptDate)
        {
            this.receiptDate = receiptDate;
        }

        public String getReceiptUrl()
        {
            return this.receiptUrl;
        }

        public void setReceiptUrl(String ReceiptUrl)
        {
            this.receiptUrl = ReceiptUrl;
        }

        public String getReceiptDate()
        {
            return this.receiptDate;
        }
    }
}
