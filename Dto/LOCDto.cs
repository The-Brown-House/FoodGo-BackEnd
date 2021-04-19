﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FoodYeah.Commons.Enums;

namespace FoodYeah.Dto
{
    public class LOCDto
    {
        public int LOCId { get; set; }
        public decimal Rate { get; set; }
        public TypeRate TypeRate { get; set; }

        public decimal LineOfCredit { get; set; }
        public decimal TotalLineOfCredit { get; set; }
        public decimal AvalibleLineOfCredit { get; set; }
        public List<SimpleQuoteDetailsDtos> QuoteDetails { get; set; }

        
    }
    public class CreateLOCDto
    {
        public int CustomerId { get; set; }
        public decimal Rate { get; set; }
        public TypeRate TypeRate { get; set; }
        public decimal TotalLineOfCredit { get; set; }
        
    }
    public class UpdateLOCDto
    {
        public int LOCId { get; set; }
        public decimal Rate { get; set; }
        public TypeRate TypeRate { get; set; }
        public decimal AvalibleLineOfCredit { get; set; }
        public decimal TotalLineOfCredit { get; set; }
       
    }
    public class DeleteLOCDto
    {
        public int LOCId { get; set; }
    }
}
