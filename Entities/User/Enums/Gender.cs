using Common.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public enum Gender : int
    {
        [Order(1)]
        [Value(-1)]
        [Name("Please Select")]
        [Display(Name = "Please Select")]
        PleaseSelect = -1,

        [Order(2)]
        [Value(1)]
        [Name("Male")]
        Male = 1,

        [Order(3)]
        [Value(2)]
        [Name("Female")]
        Female = 2,

        [Order(4)]
        [Value(4)]
        [Name("Prefer not to say")]
        [Display(Name = "Prefer not to say")]
        Prefernottosay = 3,
    }
}
