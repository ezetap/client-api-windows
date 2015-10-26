using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.eze.api
{
    public class Result
    {
        TransactionDetails tDetails;
        Cheque cheque;
        Card card;
        Merchant merchant;
        Customer customer;
        Receipt receipt;
        string message;


        public string getMessage()
        {
            return message;
        }
        public void setMessage(string message)
        {
            this.message = message;
        }
        public TransactionDetails getTransactionDetails()
        {
            return tDetails;
        }

        public void setTransactionDetails(TransactionDetails transactionDetails)
        {
            this.tDetails = transactionDetails;
        }
        public Cheque getCheque()
        {
            return cheque;
        }

        public void setCheque(Cheque cheque)
        {
            this.cheque = cheque;
        }
        public Card getCard()
        {
            return card;
        }

        public void setCard(Card card)
        {
            this.card = card;
        }

        public Merchant getMerchant()
        {
            return merchant;
        }

        public void setMerchant(Merchant merchant)
        {
            this.merchant = merchant;
        }

        public Customer getCustomer()
        {
            return customer;
        }

        public void setCustomer(Customer customer)
        {
            this.customer = customer;
        }

        public Receipt getReceipt()
        {
            return receipt;
        }

        public void setReceipt(Receipt receipt)
        {
            this.receipt = receipt;
        }

    }
}
