using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models.MetaClasses
{
    public class EnrollmentMetadata
    {
        [Display(Name = "Student Grade")]
        public Nullable<decimal> Grade { get; set; }

        [Display(Name = "Course")]
        public int CourseID { get; set; }

        [Display(Name = "Course")]
        public virtual Course Course { get; set; }

        [Display(Name = "Student")]
        public int StudentID { get; set; }

        [Display(Name = "Student")]
        public virtual Student Student { get; set; }

        [Display(Name = "Lecturer")]
        public Nullable<int> LecturerId { get; set; }

        [Display(Name = "Lecturer")]
        public virtual Lecturers Lecturers { get; set; }

    }

    [MetadataType(typeof(EnrollmentMetadata))]
    public partial class Enrollment { }
}