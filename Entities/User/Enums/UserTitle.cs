using Common.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public enum UserTitle : int
    {
        [Display(Name = "Please Select")]
        [Order(1)]
        [Name("Please Select")]
        [Value(-1)]
        PleaseSelect = -1,

        [Order(2)]
        [Name("Ms")]
        [Value(1)]
        Ms = 1,

        [Order(3)]
        [Name("Miss")]
        [Value(2)]
        Miss = 2,

        [Order(4)]
        [Name("Mrs")]
        [Value(3)]
        Mrs = 3,

        [Order(5)]
        [Name("Mr")]
        [Value(4)]
        Mr = 4
    }
}
