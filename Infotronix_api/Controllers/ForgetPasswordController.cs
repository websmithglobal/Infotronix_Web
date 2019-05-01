using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ENT = Josheph.Framework.Entity;
using BAL = Josheph.Framework.BusinessLayer;
using COM = Josheph.Framework.Common;

namespace Infotronix_api.Controllers
{
    public class ForgetPasswordController : ApiController
    {
        [HttpPost]
        //[Authorize]
        [ActionName("SendOtpEmail")]
        public HttpResponseMessage SendOtpEmail(String UserName)
        {
            BAL.AdminMasterBAL objUser = new BAL.AdminMasterBAL();
            List<ENT.AspNetUsersSUB> lstResult = new List<ENT.AspNetUsersSUB>();
            string ResponseMessage = "";
            string OTPGenerated = "";
            try
            {
                lstResult = objUser.GetUserInfoByName(UserName);
                if (lstResult.Count > 0)
                {
                    int OTP = COM.ExtendedMethods.GenerateOTP();
                    BAL.SMTPManagement objSMTP = new BAL.SMTPManagement();
                    ResponseMessage = objSMTP.SendForgetPasswordOTP(lstResult[0].Email, OTP);
                    ENT.OTPCodeMaster objENTOTP = new ENT.OTPCodeMaster();
                    objENTOTP.otp_user_id = new Guid(lstResult[0].Id);
                    objENTOTP.otp_code = OTP;
                    if (new BAL.OTPCodeMaster().Insert(objENTOTP))
                    {
                        OTPGenerated = "OTP Generated Successfully.";
                    }
                    else {
                        OTPGenerated = "Internal Server Error.";
                    }
                }
            }
            catch (Exception ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { lstResult, ResponseMessage, ErrorMessage = ex.Message.ToString(), OTPGenerated = OTPGenerated });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { lstResult, ResponseMessage, OTPGenerated = OTPGenerated });
        }

        [HttpPost]
        //[Authorize]
        [ActionName("VerifyOtp")]
        public HttpResponseMessage VerifyOtp(String UserName, String OTPCode)
        {
            BAL.AdminMasterBAL objUser = new BAL.AdminMasterBAL();
            List<ENT.AspNetUsersSUB> lstResult = new List<ENT.AspNetUsersSUB>();
            List<ENT.OTPCodeMaster> lstOTP = new List<ENT.OTPCodeMaster>();
            string ResponseMessage = "";
            try
            {
                lstResult = objUser.GetUserInfoByName(UserName);
                if (lstResult.Count > 0)
                {
                    lstOTP = new BAL.OTPCodeMaster().GetVerifyOTP(lstResult[0].Id, OTPCode);
                    if (lstOTP.Count > 0)
                    {
                        ResponseMessage = "OTP Verified Successfully.";
                    }
                    else
                    {
                        ResponseMessage = "OTP Not Found. OR Time Is Expired.";
                    }
                }
                else
                {
                    ResponseMessage = "User Name Not Found.";
                }
            }
            catch (Exception ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { lstResult, lstOTP, ResponseMessage, ErrorMessage = ex.Message.ToString()});
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { lstResult, lstOTP, ResponseMessage });
        }
    }
}
