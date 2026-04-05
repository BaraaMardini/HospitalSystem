using System.ComponentModel.DataAnnotations;

public class DepartmentUpdateDTO
{
 [Required]
    public int  ID { get; set; }
    public string?  Name { get; set; }

    public DepartmentUpdateDTO() { }

    public DepartmentUpdateDTO(int iD, string name)
    {
        ID = iD;
        Name = name;
    }
}

