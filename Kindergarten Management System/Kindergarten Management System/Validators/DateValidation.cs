using System;
using System.ComponentModel.DataAnnotations;
public class DateValidation : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime todayDate = Convert.ToDateTime(value);
        return todayDate <= DateTime.Now;
    }
}

public class EmployeeDateValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime todayDate = Convert.ToDateTime(value);
        return todayDate >= (DateTime.Now.AddYears(-65));

    }
}

public class EmployeeYearValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime todayDate = Convert.ToDateTime(value);
        return todayDate <= (DateTime.Now.AddYears(-18));

    }
}

public class StudentDateValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime todayDate = Convert.ToDateTime(value);
        return todayDate >= DateTime.Now.AddYears(-5);

    }
}

public class StudentMonthValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime todayDate = Convert.ToDateTime(value);
        return todayDate <= DateTime.Now.AddMonths(-5);

    }
}
