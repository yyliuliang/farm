using GoldenFarm.Entity;
using GoldenFarm.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldenFarm.Web.Controllers
{
    public class PaymentController : Controller
    {

        public ActionResult Alipay()
        {
            bool suc = Request["state"] == "1";
            var deposit = new UserDeposit
            {
                GatewayOrderNum = Request["sd51no"],
                FlowNum = Request["sdcustomno"],
                Amount = decimal.Parse(Request["ordermoney"]),
                Gateway = "alipay",
                UserId = int.Parse(Request["sdcustomno"].Substring(8, 5)),
                Params = Request.Url.Query,
                Status = suc ? 1 : 0,
                CreateTime = DateTime.Now
            };

            var ur = new UserRepository();
            ur.CreateDeposity(deposit);

            if(suc)
            {
                var user = ur.Get(deposit.UserId);
                if(user != null)
                {
                    user.TotalScore += deposit.Amount;
                    ur.Update(user);
                }
                var score = new UserScore
                {
                    CreateTime = DateTime.Now,
                    UserId = user.Id,
                    Num = deposit.FlowNum,
                    ChargeFee = 0,
                    Score = deposit.Amount,
                    TypeId = 2, //充值
                    Status = 1,
                    UserPath = user.RefUserPath
                };
                ur.CreateUserScore(score);
            }
            return Content("<result>1</result>");
        }


        public ActionResult Weixin()
        {
            bool suc = Request["state"] == "1";
            var deposit = new UserDeposit
            {
                GatewayOrderNum = Request["sd51no"],
                FlowNum = Request["sdcustomno"],
                Amount = decimal.Parse(Request["ordermoney"]),
                Gateway = "weixinpay",
                UserId = int.Parse(Request["sdcustomno"].Substring(8, 5)),
                Params = Request.Url.Query,
                Status = suc ? 1 : 0,
                CreateTime = DateTime.Now
            };

            var ur = new UserRepository();
            ur.CreateDeposity(deposit);

            if (suc)
            {
                var user = ur.Get(deposit.UserId);
                if (user != null)
                {
                    user.TotalScore += deposit.Amount;
                    ur.Update(user);
                }
                var score = new UserScore
                {
                    CreateTime = DateTime.Now,
                    UserId = user.Id,
                    Num = deposit.FlowNum,
                    ChargeFee = 0,
                    Score = deposit.Amount,
                    TypeId = 2, //充值
                    Status = 1,
                    UserPath = user.RefUserPath
                };
                ur.CreateUserScore(score);
            }
            return Content("<result>1</result>");
        }

    }
}