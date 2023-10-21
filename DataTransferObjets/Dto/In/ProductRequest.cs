using Context.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransferObjets.Dto.In
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public double Cost { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool State { get; set; }
        public int CategoryId { get; set; }
        public int CategoryName { get; set; }
        public int MarkName { get; set; }
        public int MarkId { get; set; }                
        public int? ParentId { get; set; }
        public int? ParentName { get; set; }
    }
}
