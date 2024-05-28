using Interfaces.Logic;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.Extensions.Logging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.UniversalAccessibility.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using Logic.QRRelated;
using Newtonsoft.Json.Serialization;

namespace VPTExtra.Pages.Account
{
    public class ProfileModel : PageModel
    {
        public List<Event> visitorEvents { get; set; }
        public User user { get; set; }
        public string ErrorMessage { get; set; }
        public string QrCodeImageBase64 { get; set; }

        private readonly IUserProfileService _userProfileDataService;
        private readonly IUserService _userService;
        public ProfileModel(IUserProfileService userProfileDataService, IUserService userService)
        {
            _userProfileDataService = userProfileDataService;
            _userService = userService;
            user = new User();
            visitorEvents = new List<Event>();
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("uId") == null)
            {
                return RedirectToPage("/Account/Login");
            }

            int userId = (int)HttpContext.Session.GetInt32("uId");

            try
            {
                user = _userService.GetVisitorById(userId);

                visitorEvents = _userProfileDataService.RetrieveUserEvents(userId);

                foreach (var item in visitorEvents)
                {
                    int eventId = item.Id;
                    string url = Url.Page("/QRCode/TicketDownloadPage", "Scan", new { userId = userId, eventId = eventId }, Request.Scheme);

                    item.EventQr = GenerateQRCode(url);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error retrieving profile data.";
            }
            return Page();
        }

        private string GenerateQRCode(string url)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                using (QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData))
                using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        qrCodeImage.Save(ms, ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        return Convert.ToBase64String(byteImage);
                    }
                }
            }
        }
    }
}
