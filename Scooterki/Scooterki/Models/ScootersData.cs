using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Scooterki.Models
{
    [MetadataType(typeof(ScootersData))]
    partial class Scooters_table
    {
    }
    public class ScootersData
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Name")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Name must have 10 do 100 characters")]
        [Required(ErrorMessage = "Add scooter name!")]
        public string Name { get; set; }

        [DisplayName("Power")]
        [Required(ErrorMessage = "Add scooter's power!")]
        [Range(1, 999, ErrorMessage = "Scooters can't have more than 1000W!")]
        public int Power { get; set; }

        [DisplayName("Max. speed")]
        [Required(ErrorMessage = "Add scooter's max speed!")]
        [Range(1, 100, ErrorMessage = "Scooters can't have speed of above 100 or negative")]
        public int Vmax { get; set; }

        [DisplayName("Price")]
        [Required(ErrorMessage = "Add scooter's price!")]
        [Range(1, 999, ErrorMessage = "Price is negative or above 1000")]
        public decimal Price { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Add description!")]
        [DataType(DataType.MultilineText)]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "Description must be 50 to 250 words")]
        public string Description { get; set; }

        [DisplayName("IsAvilable")]
        [Required(ErrorMessage = "Set availability")]
        public byte IsAvilable { get; set; }

        [DisplayName("PictureSrc")]
        [Required(ErrorMessage = "Add image source")]
        public string PictureSrc { get; set; }

        [DisplayName("UserId")]
        [IsReserved]
        public string UserId { get; set; }
    }
}