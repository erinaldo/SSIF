﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sadara.Models.V2.POCO
{

    public class AccountToPayEntity
    {

        public string ProviderCode { get; set; }

        public string DNI { get; set; }

        public string ProviderName { get; set; }

        public string BusinessName { get; set; }

        public string Phone { get; set; }

        public int CreditTerm { get; set; }

        public decimal CreditLimit { get; set; }

        public decimal Billed { get; set; }

        public decimal AmountExpired { get; set; }

        public decimal AmountAvailable { get; set; }

    }

}