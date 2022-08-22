using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Params
{
    public class PatchParticipantsParams
    {
        public string? Name { get; set; }
        public string? Rut { get; set; }
        public string? Verification_Code { get; set; }
        public string? Business_Name { get; set; }
        public string? Commercial_Business { get; set; }
        public string? Dte_Reception_Email { get; set; }
        public string? Bank_Account { get; set; }
        public int? bank { get; set; }
        public string? Commercial_address { get; set; }
        public string? Postal_address { get; set; }
        public string? Manager { get; set; }
        public string? Pay_Contact_First_Name { get; set; }
        public string? Pay_contact_last_name { get; set; }
        public string? Pay_contact_address { get; set; }
        public string? Pay_contact_phones { get; set; }
        public string? Pay_contact_email { get; set; }
        public string? Bills_contact_last_name { get; set; }
        public string? Bills_contact_first_name { get; set; }
        public string? Bills_contact_address { get; set; }
        public string? Bills_contact_phones { get; set; }
        public string? Bills_contact_email { get; set; }
    }
}
