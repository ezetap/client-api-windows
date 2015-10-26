using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.eze.api
{
   public class Card
    {
        String maskedCardNumber;
        String cardBrand;

        public void setMaskedCardNumber(String maskedCardNumber)
        {
            this.maskedCardNumber = maskedCardNumber;
        }

        public void setCardBrand(String cardBrand)
        {
            this.cardBrand = cardBrand;
        }

        public String getMaskedCardNumber()
        {
            return maskedCardNumber;
        }

        public String getCardBrandd()
        {
            return cardBrand;
        }
    }
}
