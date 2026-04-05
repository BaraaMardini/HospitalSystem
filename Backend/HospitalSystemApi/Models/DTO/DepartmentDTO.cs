using System.ComponentModel.DataAnnotations;

public class DepartmentDTO
{
    public int ID { get; set; }
    public string Name { get; set; }

    public DepartmentDTO() { }

    public DepartmentDTO(int iD, string name)
    {
        ID = iD;
        Name = name;
    }
}

