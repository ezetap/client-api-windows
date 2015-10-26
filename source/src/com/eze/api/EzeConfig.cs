using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.eze.api
{
    public class EzeConfig
    {

        String userName;
        ServerType serverType;
        LoginMode mode;
        String currencyCode;
        Boolean checkSignature;
        String appKey;

        public EzeConfig(String userName, String password, ServerType serverType)
        {
            this.userName = userName;
            this.appKey = password;
            this.serverType = serverType;
        }

        public EzeConfig(LoginMode mode, String appKey, String userName, String currencyCode, Boolean checkSignature, ServerType serverType)
        {
            this.mode = mode;
            this.appKey = appKey;
            this.userName = userName;
            this.currencyCode = currencyCode;
            this.checkSignature = checkSignature;
            this.serverType = serverType;
        }


        public String getUserName()
        {
            return userName;
        }
        public void setUserName(String uname)
        {
            this.userName = uname;
        }
        
        public ServerType getServerType()
        {
            return serverType;

        }
        public String getAppKey()
        {
            return appKey;
        }
        public LoginMode getLoginMode()
        {
            return mode;
        }
       
        public String getCurrencyCode()
        {
            return currencyCode;
        }
             public Boolean getCheckSignature()
        {
            return checkSignature;

        }
    


    }
}
