﻿using Twilio.Rest.Api.V2010.Account;

namespace fgssr.CustomMethodsServices
{
    public interface ISMSSystem
    {
        Task<bool> SendSms(string receiverPhoneNumber, string code);
    }
}
