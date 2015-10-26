
namespace com.eze.api 
{
    public enum Status 
    {
        SUCCESS, FAILURE
    }
    public enum PaymentMode
    {
        SALE , CASHBACK , CASHATPOS
    }

    public enum LoginMode
    {
        PASSWORD, APPKEY
    }

    public enum ServerType
    {
        PROD, DEMO
    }

    public enum EventName
    {
       INITIALIZATION, LOGIN, LOGOUT, EXIT, PREPARE_DEVICE, NOTIFICATION, TAKE_PAYMENT, CREATE, SET_SERVER_TYPE, SEND_RECEIPT, ATTACH_SIGNATURE, HISTORY_RESULT, TRANSACTION_DETAILS, VOID_PAYMENT,OTHER
    }

    public enum ImageType
    {
        PNG, GIF, JPEG, BMP
    }
}