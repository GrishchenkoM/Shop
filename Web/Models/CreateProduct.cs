using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class CreateProduct
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название товара")]
        public string Name { get; set; }

        public byte[] Image { get; set; }

        [Required(ErrorMessage = "Добавьте описание товара")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Введите стоимость товара")]
        //[RegularExpression(@"(([0-9]+))[,.]([0-9]*)", ErrorMessage = "Введите число. Маска: '999.99'")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Введите количество товара")]
        [RegularExpression(@"([0-9]+)", ErrorMessage = "Введите только число")]
        public int Count { get; set; }

        public bool IsAvailable { get; set; }
        public bool IsMine { get; set; }

    }
}