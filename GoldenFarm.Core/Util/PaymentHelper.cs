using GoldenFarm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using GoldenFarm;

namespace GoldenFarm.Util
{
    public class PaymentHelper
    {
        static readonly string AlipayUrl = WebConfigurationManager.AppSettings["PaymentAlipayUrl"];
        static readonly string WeixinUrl = WebConfigurationManager.AppSettings["PaymentWeixinUrl"];
        static readonly string CustId = WebConfigurationManager.AppSettings["PaymentCustId"];
        static readonly string PaymentKey = WebConfigurationManager.AppSettings["PaymentKey"];
        
        static readonly string LocalUrl = WebConfigurationManager.AppSettings["LocalUrl"];

        public static string Alipay(UserDeposit deposit)
        {

            string param = "customerid={0}&sdcustomno={1}&ordermoney={2}&cardno=34&faceno=zfb&noticeurl={3}&endcustomer={4}&endip={5}&remarks={6}&mark={7}";
            string notifyUrl = LocalUrl + "/Payment/Alipay";            
            param = string.Format(param, CustId, deposit.FlowNum, deposit.Amount, HttpUtility.UrlEncode(notifyUrl), deposit.UserId, deposit.IP, "Charge" , "Charge");
            string sign = StringHelper.MD5Hash(param + "&key=" + PaymentKey).ToUpper();

            string url = string.Format("{0}?{1}&sign={2}&superid=101651", AlipayUrl, param, sign);
            return url;
        }


        public static string Weixin(UserDeposit deposit)
        {
            string param = "customerid={0}&sdcustomno={1}&orderAmount={2}&cardno=32&noticeurl={3}&backurl={4}";
            string notifyUrl = LocalUrl + "/Payment/Weixin";
            string backUrl = LocalUrl + "/User/PaymentSuccess";
            //金额以分为单位
            param = string.Format(param, CustId, deposit.FlowNum, (int)(deposit.Amount * 100), notifyUrl, backUrl);
            string sign = StringHelper.MD5Hash(param + PaymentKey).ToUpper();

            string url = string.Format("{0}?{1}&sign={2}&superid=101651&mark=charge", WeixinUrl, param, sign);
            return url;
        }
    }
}
