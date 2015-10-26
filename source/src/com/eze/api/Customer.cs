using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.eze.api
{ 
    public class Customer
    {
        String mobileNumber;
        String emailId;

        public void setMobileNumber(String mobileNumber)
        {
            this.mobileNumber = mobileNumber;
        }

        public void setEmailId(String emailId)
        {
            this.emailId = emailId;
        }

        public String getMobileNumber()
        {
            return mobileNumber;
        }

        public String getEmailId()
        {
           return  emailId;
        }

    }
}
