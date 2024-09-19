﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Lead.Models
{
    [Table("incomestreams")]
    public class IncomeStream : AuditBase, IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
