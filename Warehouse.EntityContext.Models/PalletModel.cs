﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.EntityContext.Models
{
    public partial class PalletModel
    {
        public int? Id { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        public ICollection<BoxModel>? Boxes{ get; set; }
    }
}
