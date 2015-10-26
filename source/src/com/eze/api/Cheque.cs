using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.eze.api
{ 
   public  class Cheque
    {
        String chequeNumber;
        String chequeDate;
        String bankName;
        String bankCode;

        public String getChequeNumber()
        {
            return chequeNumber;
        }

        public void setChequeNumber(String chequeNumber)
        {
            this.chequeNumber = chequeNumber;
        }

        public String getChequeDate()
        {
            return chequeDate;
        }

        public void setChequeDate(String chequeDate)
        {
            this.chequeDate = chequeDate;
        }
        public String getBankName()
        {
            return bankName;
        }

        public void setBankName(String bankName)
        {
            this.bankName = bankName;
        }
        public String getBankCode()
        {
            return bankCode;
        }

        public void setBankCode(String bankCode)
        {
            this.bankCode = bankCode;
        }

    }
}
