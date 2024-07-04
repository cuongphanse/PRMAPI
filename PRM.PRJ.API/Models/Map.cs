using System.ComponentModel.DataAnnotations;

namespace PRM.PRJ.API.Models
{
    public class Map
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string local { get; set; }
    }
}
