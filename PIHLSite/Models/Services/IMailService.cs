using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIHLSite.Models.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
