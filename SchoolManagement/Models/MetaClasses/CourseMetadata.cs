using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class CourseMetadata
    {
        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]        
        public string Title { get; set; }

        [Range(1, 8)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Credits are required")]
        public int Credits { get; set; }
    }

    [MetadataType(typeof(CourseMetadata))]
    public partial class Course
    {

    }
}