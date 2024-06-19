using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuc.Services
{
    public static class EmailContentBuilder
    {
        public static string EmailConfirmationMessageBody(string name, string surname, string code)
        {
            return $@"
            <p>Hello {name} {surname},</p>
            <p>Thank you for registering with our service. To confirm your email address, please use the following code:</p>
            <p><strong>Confirmation Code:</strong> {code}</p>
            <p>If you did not request this registration, please ignore this email.</p>
            <p>Best regards,</p>
            <p>Your Service Team</p>";
        }

    }
}
