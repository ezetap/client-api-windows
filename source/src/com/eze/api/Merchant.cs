using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.eze.api
{
    public class Merchant
    {
        String merchantName;
        String merchantCode;
        public void setMerchantName(String merchantName)
        {
            this.merchantName = merchantName;
        }

        public void setMerchantCode(String merchantCode)
        {
            this.merchantCode = merchantCode;
        }

        public String getMerchantName()
        {
            return merchantName;
        }

        public String getMerchantCode()
        {
            return merchantCode;
        }
    }
}
