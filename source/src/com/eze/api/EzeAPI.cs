using System;
using System.IO;
using System.Diagnostics;
using com.eze.ezecli;
using Google.ProtocolBuffers;
using com.eze.api;
using System.Collections.Generic;

namespace com.eze.api 
{
    /// <summary>
    /// This is the Primary interface API class for performing functions on the Ezetap platform.
    /// </summarygetres
    public class EzeAPI 
    {

	    private Process p;
	    private BinaryWriter output;
	    private BinaryReader input;
	    private static EzeAPI API;
        private static Boolean initialized = false;
        private  event EzeNotification EzeEvent;
        public delegate void EzeNotification(string notifyMessage, EventArgs args);
        private static String ezecliPath = "";

        /// <summary>
        /// This method provides an mechanism to set the message handler that receives notifications about the status of 
        /// API transactions
        /// </summary>
        /// <param name="handler">a delegate of EzeNotification</param>
        public void setMessageHandler(EzeNotification handler)
        {
            EzeEvent += handler;
        }

        public  EzeResult close()
        {
            if (null != API)
            {
                EzeResult result = API.logout();
                if (result.getStatus() == Status.SUCCESS)
                {
                    result = API.exit();
                    if (result.getStatus() == Status.SUCCESS)
                    {
                        API.destroyInstance();
                        result = new EzeResult();
                        result.setEventName(EventName.LOGOUT);
                        result.setStatus(Status.SUCCESS);
                        return result;
                    }
                    else
                    {
                        EzeEvent("Exit UnSuccesful", new EventArgs());
                        return result;
                    }
                }
                else
                {
                    EzeEvent("Close UnSuccesful", new EventArgs());
                    return result;
                }
            }
            else
            {
                EzeEvent("Close UnSuccesful", new EventArgs());
                EzeResult result = new EzeResult();
                result.setEventName(EventName.LOGOUT);
                result.setStatus(Status.FAILURE);
                return result;
            }
        }
        public static void setPath(String path)
        {
            ezecliPath = path;
        }
        /**
         * Method creates an instance of EzeAPI with the given API configuration settings
         */
        public static EzeAPI create()
        {
            if (null == API)
            {
                API = new EzeAPI();
          
            }
            return API;
        }

	    private EzeResult login(LoginMode mode, string userName, string passkey) 
        {
		    Console.WriteLine("...Login User <"+mode+":"+userName+":"+passkey+">");

            LoginInput loginInput = LoginInput.CreateBuilder()
                            .SetLoginMode(MapLoginMode(mode))
                            .SetUsername(userName)
						    .SetPasskey(passkey).Build();
         
            ApiInput apiInput = ApiInput.CreateBuilder()
						    .SetMsgType(ApiInput.Types.MessageType.LOGIN)
						    .SetMsgData(loginInput.ToByteString()).Build();
		
            this.send(apiInput);

            EzeResult result = null;
	
		    while (true) 
            {
			    result = this.getResult(this.receive());
                if (result.getEventName() != EventName.LOGIN) continue;
			    if ((result.getStatus().ToString() == com.eze.ezecli.ApiOutput.Types.ResultStatus.FAILURE.ToString())) 
                {
                   throw new EzeException("Login failed. " + result.ToString());
                  
			   }
			    break;
		    }
            Console.WriteLine("2......"+result);
		    return result;
	    }

		
	    /// <summary>
        /// 
        /// </summary>
        /// <returns>EzeResult - instance </returns>
        public EzeResult prepareDevice() {
		    Console.WriteLine("...Preparing Device");
		
		    ApiInput apiInput = ApiInput.CreateBuilder()
				    .SetMsgType(ApiInput.Types.MessageType.PREPARE_DEVICE)
				    .Build();

		    this.send(apiInput);
		    EzeResult result = null;
		
		    while (true) 
            {
			    result = this.getResult(this.receive());
                if (result.getEventName() == EventName.PREPARE_DEVICE) break;
		    }
            Console.WriteLine("99: "+result);
		    return result;
	    }


        public EzeResult cardTransaction( double amount, PaymentMode mode, OptionalParams options)
        {
            EzeResult result = null;
            Console.WriteLine("...Take Payment <" + mode + ",amount=" + amount + "," + ">");
            TxnInput.Types.TxnType txnType = TxnInput.Types.TxnType.CARD_AUTH;

            if (amount <= 0) throw new EzeException("Amount is 0 or negative");
            
            TxnInput tInput = TxnInput.CreateBuilder()
                    .SetTxnType(txnType)
                    .SetAmount(amount)
                    .Build();

            if (null != options)
            {
                if (null != options.getReference())
                {
                    if (null != options.getReference().getReference1()) tInput = TxnInput.CreateBuilder(tInput).SetOrderId(options.getReference().getReference1()).Build();
                    if (null != options.getReference().getReference2()) tInput = TxnInput.CreateBuilder(tInput).SetExternalReference2(options.getReference().getReference2()).Build();
                    if (null != options.getReference().getReference3()) tInput = TxnInput.CreateBuilder(tInput).SetExternalReference3(options.getReference().getReference3()).Build();
                }
                if (0 != options.getAmountCashback()) tInput = TxnInput.CreateBuilder(tInput).SetAmountOther(options.getAmountCashback()).Build();
                //  if (0 != options.getAamountTip()) tInput = TxnInput.CreateBuilder(tInput).Set(options.getBankCode()).Build();
                if (null != options.getCustomer())
                {
                    String mobileNumber = options.getCustomer().getMobileNumber();
                    String emailId = options.getCustomer().getEmailId();
                    if (null != mobileNumber) tInput = TxnInput.CreateBuilder(tInput).SetCustomerMobile(mobileNumber).Build();
                    if (null != emailId) tInput = TxnInput.CreateBuilder(tInput).SetCustomerEmail(emailId).Build();

                }
            }

            ApiInput apiInput = ApiInput.CreateBuilder()
                    .SetMsgType(ApiInput.Types.MessageType.TXN)
                    .SetMsgData(tInput.ToByteString()).Build();

            this.send(apiInput);

            while (true)
            {
                result = this.getResult(this.receive());

                if (result.getEventName() == EventName.TAKE_PAYMENT)
                {
                    if (result.getStatus() == Status.SUCCESS) EzeEvent("Payment Successful", new EventArgs());
                    else EzeEvent("Payment Failed", new EventArgs());
                    break;
                }
            }

            return result;
        }

        public EzeResult cashTransaction(double amount, OptionalParams options)
        {
            EzeResult result = null;
            Console.WriteLine("...Take Payment By Cash <" +  ",amount=" + amount + "," + ">");
            TxnInput.Types.TxnType txnType = TxnInput.Types.TxnType.CASH;

            if (amount <= 0) throw new EzeException("Amount is 0 or negative");

            TxnInput tInput = TxnInput.CreateBuilder()
                    .SetTxnType(txnType)
                    .SetAmount(amount)
                    .Build();

            if (null != options)
            {
                if (null != options.getReference())
                {
                    if (null != options.getReference().getReference1()) tInput = TxnInput.CreateBuilder(tInput).SetOrderId(options.getReference().getReference1()).Build();
                    if (null != options.getReference().getReference2()) tInput = TxnInput.CreateBuilder(tInput).SetExternalReference2(options.getReference().getReference2()).Build();
                    if (null != options.getReference().getReference3()) tInput = TxnInput.CreateBuilder(tInput).SetExternalReference3(options.getReference().getReference3()).Build();
                }
               
                if (null != options.getCustomer())
                {
                    String mobileNumber = options.getCustomer().getMobileNumber();
                    String emailId = options.getCustomer().getEmailId();
                    if (null != mobileNumber) tInput = TxnInput.CreateBuilder(tInput).SetCustomerMobile(mobileNumber).Build();
                    if (null != emailId) tInput = TxnInput.CreateBuilder(tInput).SetCustomerEmail(emailId).Build();

                }
            }

            ApiInput apiInput = ApiInput.CreateBuilder()
                    .SetMsgType(ApiInput.Types.MessageType.TXN)
                    .SetMsgData(tInput.ToByteString()).Build();

            this.send(apiInput);

            while (true)
            {
                result = this.getResult(this.receive());

                if (result.getEventName() == EventName.TAKE_PAYMENT)
                {
                    if (result.getStatus() == Status.SUCCESS) EzeEvent("Payment Successful", new EventArgs());
                    else EzeEvent("Payment Failed", new EventArgs());
                    break;
                }
            }

            return result;
        }


        public EzeResult chequeTransaction(double amount,Cheque cDetails, OptionalParams options)
        {
            EzeResult result = null;
            Console.WriteLine("...Take Payment By Cash <" + ",amount=" + amount + "," + ">");
            TxnInput.Types.TxnType txnType = TxnInput.Types.TxnType.CHEQUE;

            if (amount <= 0) throw new EzeException("Amount is 0 or negative");

            TxnInput tInput = TxnInput.CreateBuilder()
                    .SetTxnType(txnType)
                    .SetAmount(amount)
                    .Build();

            if ((null == cDetails) ||
                (null == cDetails.getChequeNumber()) || (cDetails.getChequeNumber().Length == 0) ||
                (null == cDetails.getChequeDate()) || (cDetails.getChequeDate().Length == 0) ||
                (null == cDetails.getBankName()))
            {
                throw new EzeException("Cheque details not passed for a Cheque transaction");
            }

            if (null != cDetails.getChequeNumber()) tInput = TxnInput.CreateBuilder(tInput).SetChequeNumber(cDetails.getChequeNumber()).Build();
            if (null != cDetails.getChequeDate()) tInput = TxnInput.CreateBuilder(tInput).SetChequeDate(cDetails.getChequeDate()).Build();
            if (null != cDetails.getBankCode()) tInput = TxnInput.CreateBuilder(tInput).SetBankCode(cDetails.getBankCode()).Build();
          //  if (null != cDetails.getBankName()) tInput = TxnInput.CreateBuilder(tInput).SetBankName(cDetails.getBankName()).Build();
          //  if (null != options.getChequeDate()) tInput = TxnInput.CreateBuilder(tInput).SetChequeDate(options.getChequeDate().ToString()).Build();


            if (null != options)
            {
                if (null != options.getReference())
                {
                    if (null != options.getReference().getReference1()) tInput = TxnInput.CreateBuilder(tInput).SetOrderId(options.getReference().getReference1()).Build();
                    if (null != options.getReference().getReference2()) tInput = TxnInput.CreateBuilder(tInput).SetExternalReference2(options.getReference().getReference2()).Build();
                    if (null != options.getReference().getReference3()) tInput = TxnInput.CreateBuilder(tInput).SetExternalReference3(options.getReference().getReference3()).Build();
                }

                if (null != options.getCustomer())
                {
                    String mobileNumber = options.getCustomer().getMobileNumber();
                    String emailId = options.getCustomer().getEmailId();
                    if (null != mobileNumber) tInput = TxnInput.CreateBuilder(tInput).SetCustomerMobile(mobileNumber).Build();
                    if (null != emailId) tInput = TxnInput.CreateBuilder(tInput).SetCustomerEmail(emailId).Build();

                }
            }

            ApiInput apiInput = ApiInput.CreateBuilder()
                    .SetMsgType(ApiInput.Types.MessageType.TXN)
                    .SetMsgData(tInput.ToByteString()).Build();

            this.send(apiInput);

            while (true)
            {
                result = this.getResult(this.receive());

                if (result.getEventName() == EventName.TAKE_PAYMENT)
                {
                    if (result.getStatus() == Status.SUCCESS) EzeEvent("Payment Successful", new EventArgs());
                    else EzeEvent("Payment Failed", new EventArgs());
                    break;
                }
            }

            return result;
        }

      /*  public EzeResult takePayment(PaymentType type, double amount, PaymentOptions options) 
        {
		    EzeResult result = null;
		    Console.WriteLine("...Take Payment <"+type.ToString()+",amount="+amount+","+">");
		    TxnInput.Types.TxnType txnType = TxnInput.Types.TxnType.CARD_PRE_AUTH;
		
            switch(type) 
            {
    		    case PaymentType.CARD: 
                {
                    txnType = TxnInput.Types.TxnType.CARD_AUTH;
			        break;
                }
		        case PaymentType.CASH: 
                {
			        txnType = TxnInput.Types.TxnType.CASH;
			        break;
                }
		        case PaymentType.CHEQUE: 
                {
			        txnType = TxnInput.Types.TxnType.CHEQUE;
			        break;
                }
		        default: 
                {
			        txnType = TxnInput.Types.TxnType.CARD_PRE_AUTH;
                    break;
                }
		    }

            if (amount <= 0) throw new EzeException("Amount is 0 or negative");
		    if (txnType == TxnInput.Types.TxnType.CHEQUE) 
            {
			    if ((null == options) ||
				    (null == options.getChequeNo()) || (options.getChequeNo().Length == 0) ||
				    (null == options.getBankCode()) || (options.getBankCode().Length == 0) ||
				    (null == options.getChequeDate())) 
                {
                        throw new EzeException("Cheque details not passed for a Cheque transaction");
			    }
		    }
		
		    TxnInput tInput = TxnInput.CreateBuilder()
                
				    .SetTxnType(txnType)
				    .SetAmount(amount)
				    .Build();
		
		    if (null != options) {
			    if (null != options.getOrderId()) tInput = TxnInput.CreateBuilder(tInput).SetOrderId(options.getOrderId()).Build();
			    if (null != options.getReceiptType()) tInput = TxnInput.CreateBuilder(tInput).SetReceiptType(options.getReceiptType()).Build();
			    if (null != options.getChequeNo()) tInput = TxnInput.CreateBuilder(tInput).SetChequeNumber(options.getChequeNo()).Build();
			    if (null != options.getBankCode()) tInput = TxnInput.CreateBuilder(tInput).SetBankCode(options.getBankCode()).Build();
			    if (null != options.getChequeDate()) tInput = TxnInput.CreateBuilder(tInput).SetChequeDate(options.getChequeDate().ToString()).Build();
		    }
		
		    ApiInput apiInput = ApiInput.CreateBuilder()
				    .SetMsgType(ApiInput.Types.MessageType.TXN)
				    .SetMsgData(tInput.ToByteString()).Build();

		    this.send(apiInput);
				
		    while (true) 
            {
			    result = this.getResult(this.receive());

                if (result.getEventName() == EventName.TAKE_PAYMENT)
                {
                    if (result.getStatus() == Status.SUCCESS) EzeEvent("Payment Successful", new EventArgs());
                    else EzeEvent("Payment Failed", new EventArgs());
                    break;
                }
		    }
       
            return result;
	    } */

        public EzeResult sendReceipt(string txnId, string mobileNo, String email)
        { 
		    Console.Error.WriteLine("...sendReceipt <"+txnId+">");
		
		    ForwardReceiptInput receiptInput = ForwardReceiptInput.CreateBuilder()
				    .SetTxnId(txnId)
				    .SetCustomerMobile(mobileNo)
                    .SetCustomerEmail(email).Build();
		
		    ApiInput apiInput = ApiInput.CreateBuilder()
				    .SetMsgType(ApiInput.Types.MessageType.FORWARD_RECEIPT)
				    .SetMsgData(receiptInput.ToByteString()).Build();

		    this.send(apiInput);
            EzeResult result = null;
		
            while (true) 
            {
			    result = this.getResult(this.receive());
			    if (result.getEventName() == EventName.SEND_RECEIPT) break;
		    }
            
            return result;
          
	    }

        public EzeResult getTransactionHistory(string startDate,string endDate)
        {
            Console.Error.WriteLine("...Transaction History < >");

            TxnHistoryInput historyInput = TxnHistoryInput.CreateBuilder().SetStrtDate(startDate).SetEndDate(endDate)
                    .Build();

            ApiInput apiInput = ApiInput.CreateBuilder()
                    .SetMsgType(ApiInput.Types.MessageType.TXN_HISTORY)
                    .SetMsgData(historyInput.ToByteString()).Build();

            this.send(apiInput);
            EzeResult result = null;

            while (true)
            {
                result = this.getResult(this.receive());
                if (result.getEventName() == EventName.HISTORY_RESULT) break;
            }
            return result;
        }

        /**
         * Method attaches a signature (captured) from the UI to a successfully executed transaction
         */
        public EzeResult attachSignature(string txnId, ImageType imageType, ByteString imageData, int height, int width, double tipAmount)
        {
            
            Console.Error.WriteLine("...attachSignature <" + txnId + ">");

            SignatureInput signatureInput = SignatureInput.CreateBuilder()
                    .SetTxnId(txnId)
                    .SetImageType(MapImageType(imageType))                   
                    .SetImageBytes(imageData)
                    .SetHeight(height)
                    .SetWidth(width)
                    .SetTipAmount(tipAmount)
                    .Build();

            ApiInput apiInput = ApiInput.CreateBuilder()
                    .SetMsgType(ApiInput.Types.MessageType.ATTACH_SIGNATURE)
                    .SetMsgData(signatureInput.ToByteString()).Build();

            this.send(apiInput);
            EzeResult result = null;

            while (true)
            {
                result = this.getResult(this.receive());
                if (result.getEventName() == EventName.ATTACH_SIGNATURE) break;
            }
            return result;
        }

        private EzeAPI()
        {
        }

        /** 
         * This method destroys the ezecli exe and gracefully closes the API.
         */
        private void destroyInstance()
        {
            if (null != p)
            {
                try
                {
                    p.Kill();
                }
                catch (Exception e)
                {
                    //Console.Write(e.Message);
                }
            }
        }

       //// private static EzeAPI getAPI()
       // {
         //   if (null == API)
        //    {
        //        API = new EzeAPI();
         //       API.initialize();
        //    }
         //   return API;
      //  }


       public EzeResult initialize(EzeConfig config)
        {
            if (!initialized)
            {
                    Boolean init = initializeEzeCli();
                    if (init)
                    { 
                        Console.WriteLine(" " + config.getLoginMode() + " " + config.getUserName() + " " + config.getAppKey()+" "+API);
                        API.setServerType(config.getServerType());
                        EzeResult result = API.login(config.getLoginMode(), config.getUserName(), config.getAppKey());
                        Console.WriteLine(result);
                        if (result.getStatus() == Status.SUCCESS)
                        {
                            Console.WriteLine("Logged in succesfully");
                            result = API.prepareDevice();
                            if (result.getStatus() == Status.SUCCESS)
                            {
                                EzeEvent("Initialization succesful", new EventArgs());
                                return result;
                            }
                            else
                            {
                            result = API.prepareDevice();
                            if ((result.getStatus() == Status.SUCCESS) && (EzeEvent!=null))
                                EzeEvent("Initialization succesful", new EventArgs());
                            else if   (EzeEvent != null)
                            EzeEvent("Prepare Device failed", new EventArgs());
                                return result;
                            }
                        }
                        else
                        {
                            EzeEvent("Login Failed", new EventArgs());
                        Console.WriteLine("--------------- failed");
                           return result;

                    }

                     }
                    else
                    {
                         EzeEvent("Initialization failed", new EventArgs());
                        EzeResult result = new EzeResult();
                        result.setStatus(Status.FAILURE);
                        result.setEventName(EventName.INITIALIZATION);
                    Error err = new Error();
                    err.setMessage("Initialization Failed");
                    result.setError(err);
                         return result;
                    }
            }
            else
            {
                EzeResult result = new EzeResult();
                result.setStatus(Status.FAILURE);
                result.setEventName(EventName.INITIALIZATION);
                Error err = new Error();
                err.setMessage("Already initialized");
                result.setError(err);
                return result;
            }
        
        }
        /**
         * This method instantiates the Ezecli and setup the input 
         * and output buffers for reading and writing through protocol buffers.
         */
        private Boolean initializeEzeCli()
        {
            try
            {
                if (null != p)
                {
                    Console.WriteLine("Killing process");
                    p.Kill();
                }
                try {
                    Process[] proc = Process.GetProcessesByName("ezecli");
                    proc[0].Kill();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Killing process failed");
                }
                ProcessStartInfo startInfo = new ProcessStartInfo(getEzecliFile());
                startInfo.CreateNoWindow = true;
                startInfo.ErrorDialog = false;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;

                p = new Process();
                p.StartInfo = startInfo;
                p.Start();
                input = new BinaryReader(p.StandardOutput.BaseStream);
                output = new BinaryWriter(p.StandardInput.BaseStream);
                //err = p.StandardError;
                initialized = true;
                Console.WriteLine("initialized with "+p.Id);
                return initialized;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);

                try
                {
                    if (p != null) p.Kill();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                throw new EzeException("Initialize failed. e=" + e.Message);
            }
        }

        private void setServerType(ServerType type)
        {
            Console.WriteLine("...Setting server type to <" + type.ToString() + ">");

            ServerTypeInput serverTypeInput = ServerTypeInput.CreateBuilder()
                            .SetServertype(MapServerType(type)).Build();

            ApiInput apiInput = ApiInput.CreateBuilder()
                            .SetMsgType(ApiInput.Types.MessageType.SERVER_TYPE)
                            .SetMsgData(serverTypeInput.ToByteString()).Build();

            this.send(apiInput);

            /*
            EzeResult result = null;

            while (true)
            {
                result = this.getResult(this.receive());
                if (result.getEventName() == EventName.SET_SERVER_TYPE) break;
            }*/
        }

        private EzeResult logout()
        {
            Console.WriteLine("...logging out");

            ApiInput apiInput = ApiInput.CreateBuilder()
                    .SetMsgType(ApiInput.Types.MessageType.LOGOUT)
                    .Build();

            this.send(apiInput);
            EzeResult result = null;
            while (true)
            {
                result = this.getResult(this.receive());
                if (result.getEventName() != EventName.LOGOUT) continue;
                if ((result.getStatus().ToString() == ApiOutput.Types.ResultStatus.FAILURE.ToString()))
                {
                    Console.WriteLine("Error logout");
                    
                }
                else
                {
                    Console.WriteLine(" logout success");
                    break;
                }
                
            }
            return result;
        }

        private EzeResult exit()
        {
            Console.WriteLine("...exiting");
            ApiInput apiInput = ApiInput.CreateBuilder()
                    .SetMsgType(ApiInput.Types.MessageType.EXIT)
                    .Build();

            this.send(apiInput);
            EzeResult result = null;
            while (true)
            {
                result = this.getResult(this.receive());
                if (result.getEventName() != EventName.EXIT) continue;
                if ((result.getStatus().ToString() == ApiOutput.Types.ResultStatus.FAILURE.ToString()))
                {
                    EzeEvent("Initialization succesful", new EventArgs());
                    return result; ;
                }
                break;
            }
            return result;
        }
        public EzeResult voidTransaction(String txnId)
        {
            Console.WriteLine("Void....");
            VoidTxnInput voidTaxInput = VoidTxnInput.CreateBuilder().SetTxnId(txnId).Build();
            ApiInput apiInput = ApiInput.CreateBuilder().SetMsgType(ApiInput.Types.MessageType.VOID_TXN).SetMsgData(voidTaxInput.ToByteString()).Build();
            this.send(apiInput);
            EzeResult result = null;
            while (true)
            {
                result = this.getResult(this.receive());
                if (result.getEventName() == EventName.VOID_PAYMENT)
                {
                   Console.WriteLine(result);
                    break;
                }
            }
            return result;
        } 
        public EzeResult getTransaction(String txId)
        {
            Console.WriteLine("Fetching transaction details for "+ txId);
            TxnDetailsInput txnInput = TxnDetailsInput.CreateBuilder().SetTxnId(txId).Build();
            ApiInput input = ApiInput.CreateBuilder().SetMsgType(ApiInput.Types.MessageType.TXN_DETAILS).SetMsgData(txnInput.ToByteString()).Build();
            this.send(input);
            EzeResult result = null;
            while (true)
            {
                result = this.getResult(this.receive());

                if (result.getEventName() == EventName.TRANSACTION_DETAILS)
                {
                    if (result.getStatus() == Status.SUCCESS) EzeEvent("Fetching Transaction Successful", new EventArgs());
                    else EzeEvent("Fetching Transaction Details Failed", new EventArgs());
                    break;
                }
            }

            return result;
           
        }
        
        private EzeResult getResult(ApiOutput apiOutput)
        {
            EzeResult result = new EzeResult();
            Result paymentResult = new Result();
            paymentResult.setMessage("Ezeeeeeeeeeeeeeeeee");
            if (null == apiOutput) throw new EzeException("Invalid response from EPIC. ApiOutput is null");
            Console.WriteLine("1.."+apiOutput.EventType);
            
            Console.WriteLine("2. "+ MapEventName(apiOutput.EventType));
            result.setEventName(MapEventName(apiOutput.EventType));
            if (apiOutput.HasStatus) result.setStatus(MapStatus(apiOutput.Status));
            if (apiOutput.HasMsgText)
            {

                paymentResult.setMessage(apiOutput.MsgText);
                result.setResult(paymentResult);
            }
            Console.WriteLine("3.."+apiOutput.MsgText);
            Console.WriteLine("4.." + result);
            if (apiOutput.HasOutData) 
            {
                 try 
                 {
                     StatusInfo statusInfo = StatusInfo.ParseFrom(apiOutput.OutData);
                     if (apiOutput.HasStatus)
                     {


                         if (result.getStatus() == Status.FAILURE)
                         {
                             Console.WriteLine("Failure");
                             if (null != statusInfo)
                             { 
                                 Error error = new Error();
                             error.setCode(statusInfo.Code);
                             error.setMessage(statusInfo.Message);
                             result.setError(error);
                             }
                         }

                     }

                 } 
                 catch (InvalidProtocolBufferException e) 
                 {
                     Console.WriteLine(e.Message);
                 }

                if ((apiOutput.Status == ApiOutput.Types.ResultStatus.SUCCESS) && (apiOutput.EventType.Equals(ApiOutput.Types.EventType.TXN_HISTORY_RESULT)))
                {
                    try
                    {
                        Console.WriteLine("history parsing begins... ");
                        TxnHistory txHistory = TxnHistory.ParseFrom(apiOutput.OutData);
                        Console.WriteLine("Null history "+txHistory);
                        
                        if (null != txHistory)
                        {

                            Console.WriteLine(txHistory.TotalCount);
                          //  IList<Txn> list = txHistory.

                        }
                        else
                        {
                            Console.WriteLine("Null histpry");
                        }
                    }
                    catch (InvalidProtocolBufferException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                if ((apiOutput.Status == ApiOutput.Types.ResultStatus.SUCCESS) && (apiOutput.EventType.Equals(ApiOutput.Types.EventType.TXN_RESULT) || (apiOutput.EventType.Equals(ApiOutput.Types.EventType.TXN_DETAILS_RESULT)))) 
                {
                  
				    
                    TransactionDetails tDetails = new TransactionDetails();
                    Card card = new Card();
                    Merchant merchant = new Merchant();
                    Customer cust = new Customer();
                    Receipt receipt = new Receipt();
                    Cheque cheque = new Cheque();
				    Txn txnOutput;
				    try 
                    {
					    txnOutput = Txn.ParseFrom(apiOutput.OutData);
                        tDetails.setPmtType(txnOutput.TxnType.ToString());
                        tDetails.setStatus(txnOutput.Status);
                        tDetails.setTxnId(txnOutput.TransactionId);
                        tDetails.setAmount(txnOutput.Amount);
                        tDetails.setSettlementStatus(txnOutput.SettlementStatus);
                        tDetails.setVoidable(txnOutput.Voidable);
                        tDetails.setAuthCode(txnOutput.AuthCode);
                        tDetails.setCardType(txnOutput.CardBrand);
                        tDetails.setOrderId(txnOutput.OrderId);
                        tDetails.setTid(txnOutput.Tid);
                        paymentResult.setTransactionDetails(tDetails);
                       merchant.setMerchantCode(txnOutput.Mid);
                        // merchant.setMerchantName(txnOutput.m);
                        paymentResult.setMerchant(merchant);
                        cust.setMobileNumber(txnOutput.CustomerMobileNumber);
                        // cust.setemailId(txnOutput.);
                        paymentResult.setCustomer(cust);
                        receipt.setReceiptUrl(txnOutput.ReceiptUrl);
                        receipt.setReceiptDate(txnOutput.Timestamp);
                        paymentResult.setReceipt(receipt);
                        cheque.setChequeNumber(txnOutput.ChequeNumber);
                        cheque.setChequeDate(txnOutput.ChequeDate);
                        cheque.setBankCode(txnOutput.BankCode);
                        // cheque.setBankName(txnOutput.b);
                        paymentResult.setCheque(cheque);
                        card.setCardBrand(txnOutput.CardBrand);
                        //  card.setMaskedCardNumber(txnOutput.LastFoundDigits);
                        paymentResult.setCard(card);
                    } 
                    catch (InvalidProtocolBufferException e) 
                    {
                        throw new EzeException("Error reading payment result. ex=" + e.Message);
				    }
                   // PaymentResult list = new List<PaymentResult>();
				    result.setResult(paymentResult);
			    }
		    }

            //Console.Write("ApiOutput: " + apiOutput.ToString());

            if ((result.getEventName() == EventName.NOTIFICATION) && (null != EzeEvent))
            {
                Result res = result.getResult();
                Console.WriteLine("5.."+res);
                if (res!=null)
                EzeEvent(result.getResult().getMessage(), new EventArgs());
            }

		    return result;
	    }
	
	    private void send(ApiInput apiInput) {

            //Console.Write(apiInput.ToJson());
		    byte[] length = new byte[4];

		    try {
			    length = this.intToBytes(apiInput.SerializedSize);
			    //p.StandardInput.Write(length);
			    //p.StandardInput.Write(apiInput.ToByteString());
			    //p.StandardInput.Flush();
                output.Write(length);
                byte[] arr = apiInput.ToByteArray();
                output.Write(arr);
                //Console.Write(apiInput.ToByteArray());
                output.Flush();
		    } catch (InvalidProtocolBufferException e) {
			    Console.WriteLine("Parse Error " + e.ToString());
		    } catch (IOException e) {
			    Console.WriteLine("Error readline " + e.ToString());
		    }
	    }

	    private ApiOutput receive() {
		
		    ApiOutput apiOutput = null;
		    byte[] length = new byte[4];

		    try {
			    this.readWithTimeout(length, 30000);
			    int lengthInt = getIntegerFromByte(length);

			    if (lengthInt > 0) {
				    byte[] data = new byte[lengthInt];
				    this.readWithTimeout(data, 30000);
				    apiOutput = ApiOutput.ParseFrom(data);
			    }
		    } catch (InvalidProtocolBufferException e) {
			    Console.WriteLine("Parse Error " + e.ToString());
		    } catch (IOException e) {
			    Console.WriteLine("Error readline " + e.ToString());
		    }
		    return apiOutput;
	    }

	    private int readWithTimeout(byte[] data, int timeoutMillis) {
		
            int offset = 0;
		    int dataLength = data.Length;
		    long maxTimeMillis = CurrentTimeMillis() + timeoutMillis;
		    while (CurrentTimeMillis() < maxTimeMillis && offset < dataLength) {
			    //long length = Math.Min(input.BaseStream.Length, dataLength-offset);
	    		
			    // can alternatively use bufferedReader, guarded by isReady():
			    int result = input.Read(data, offset, dataLength);
			    if (result == -1) break;
			    offset += result;
		    }
		    return offset;

            //input.BaseStream.ReadTimeout = timeoutMillis;
            //data = this.input.ReadBytes(data.Length);
	    }

        private static readonly DateTime Jan1st1970 = new DateTime
        (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

	    private string getEzecliFile() {
            if (!ezecliPath.Equals(""))
                return ezecliPath;
            else
                return "c:\\program files (x86)\\Ezetap\\cli\\ezecli.exe";
	    }
	
	    private byte[] intToBytes(int intValue) {
            byte[] intBytes = BitConverter.GetBytes(intValue);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            byte[] result = intBytes;
	        return result;
        }

	    private static byte[] reverseArray(byte[] array) {
		    byte[] reversedArray = new byte[array.Length];
            for(int i = 0; i < array.Length; i++){
                reversedArray[i] = array[array.Length - i - 1];
            }
            return reversedArray;
        }

        private static int getIntegerFromByte(byte[] byteArr)
        {
		    return (byteArr[3]) << 24 | (byteArr[2] & 0xFF) << 16
				    | (byteArr[1] & 0xFF) << 8 | (byteArr[0] & 0xFF);
	    }

        private static Status MapStatus(ApiOutput.Types.ResultStatus status)
        {
            if (status == ApiOutput.Types.ResultStatus.SUCCESS) return Status.SUCCESS;
            else return Status.FAILURE;
        }
        private static ServerTypeInput.Types.ServerType MapServerType(ServerType type)
        {
            if (type == ServerType.DEMO) return ServerTypeInput.Types.ServerType.SERVER_TYPE_DEMO;
            else return ServerTypeInput.Types.ServerType.SERVER_TYPE_PROD;
        }

        private static LoginInput.Types.LoginMode MapLoginMode(LoginMode mode)
        {
            if (mode == LoginMode.APPKEY) return LoginInput.Types.LoginMode.APPKEY;
            else return LoginInput.Types.LoginMode.PASSWORD;
        }

        private static SignatureInput.Types.ImageType MapImageType(ImageType imageType)
        {
            switch (imageType)
            {
                case ImageType.BMP: return SignatureInput.Types.ImageType.BMP;
                case ImageType.GIF: return SignatureInput.Types.ImageType.GIF;
                case ImageType.JPEG: return SignatureInput.Types.ImageType.JPEG;
                case ImageType.PNG: return SignatureInput.Types.ImageType.PNG;
                default: return SignatureInput.Types.ImageType.JPEG;
            }
        }

        private static EventName MapEventName(com.eze.ezecli.ApiOutput.Types.EventType eType)
        {
            switch (eType)
            {
                case ApiOutput.Types.EventType.LOGIN_RESULT: return EventName.LOGIN;
                case ApiOutput.Types.EventType.LOGOUT_RESULT: return EventName.LOGOUT;
                case ApiOutput.Types.EventType.EXIT_RESULT: return EventName.EXIT;
                case ApiOutput.Types.EventType.SETSERVER_RESULT: return EventName.SET_SERVER_TYPE;
                case ApiOutput.Types.EventType.PREPARE_DEVICE_RESULT: return EventName.PREPARE_DEVICE;
                case ApiOutput.Types.EventType.TXN_RESULT: return EventName.TAKE_PAYMENT;
                case ApiOutput.Types.EventType.FORWARD_RECEIPT_RESULT: return EventName.SEND_RECEIPT;
                case ApiOutput.Types.EventType.API_NOTIFICATION: return EventName.NOTIFICATION;
                case ApiOutput.Types.EventType.API_PROGRESS: return EventName.NOTIFICATION;
                case ApiOutput.Types.EventType.ATTACH_SIGNATURE_RESULT: return EventName.ATTACH_SIGNATURE;
                case ApiOutput.Types.EventType.TXN_HISTORY_RESULT: return EventName.HISTORY_RESULT;
                case ApiOutput.Types.EventType.TXN_DETAILS_RESULT: return EventName.TRANSACTION_DETAILS;
                default: return EventName.OTHER;
           }
        }
    }
}