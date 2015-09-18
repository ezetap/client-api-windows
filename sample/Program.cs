using com.eze.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzeAPIWinSample
{
    class Program
    {
        static void messageListener(String message, EventArgs args)
        {
            Console.WriteLine("MessageListener: " + message);

        }

        static void Main(string[] args)
        {
            EzeAPI api = null;

            try
            {
                api = EzeAPI.create(ServerType.DEMO);
                api.setMessageHandler(messageListener);
                // method used to login to ezetap. Use your existing username and password.
                EzeResult result = api.login(LoginMode.PASSWORD, "9538003339", "11demo");
                Console.WriteLine("Login result="+result);
                
                if (result.getStatus().Equals(Status.SUCCESS))
                {
                    // Prepare the device before any payment is taken. It is only required to prepare a device once. 
                    // If a takePayment method fails (due to device shutdown etc) please invoke prepareDevice as needed.
                    result = api.prepareDevice();
                    Console.WriteLine("PrepareDevice result="+result);

                    if (result.getStatus() == Status.SUCCESS)
                    {
                        int count = 0;
                        while (count < 10)
                        {
                            //method to take card transaction..
                            result = api.takePayment(PaymentType.CARD, 20.0, null);
                            Console.WriteLine("CardPayment Result=" + result);

                            if (result.getStatus() == Status.SUCCESS)
                            {
                                //This method is used for sending a receipt to the customer's email address. 
                                result = api.sendReceipt(result.getPaymentResult().getTxnId(), "9538003331", "karthik.k@ezetap.com");
                                Console.WriteLine("SendReceipt Result=" + result);
                            }
                            else
                            {
                                result = api.prepareDevice();
                                //if (result.getStatus() == Status.FAILURE) break;
                            }

                            System.Threading.Thread.Sleep(10000);
                            count++;
                        }
                    }
                    /*
                    // method to take a cash transaction..
                    result = api.takePayment(PaymentType.CASH, 25.0, null);
                    Console.WriteLine("CashPayment Result=" + result);

                    // method to take a cheque transaction..
                    PaymentOptions options = PaymentOptions.build()
                            .setBankCode("HDFC")
                            .setChequeDate("2014-07-21")
                            .setChequeNo("1234");

                    result = api.takePayment(PaymentType.CHEQUE, 30.0, options);
                    Console.WriteLine("ChequePayment Result=" + result);
                    */
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            // method to quit from ezetap device (closes ezecli and all connections).
            EzeAPI.destroy();
            Console.WriteLine("....destroy application []");

            Console.ReadLine();
            // use this method at the end to close ezecli and all connections in case of any exceptions. 
            // should be in your finally block.
        }
    }
}
