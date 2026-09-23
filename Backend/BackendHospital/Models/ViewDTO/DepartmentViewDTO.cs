using System.ComponentModel.DataAnnotations;

public class DepartmentViewDTO
{
    public int ID { get; set; }
    public string Name { get; set; }

    public DepartmentViewDTO() { }

    public DepartmentViewDTO(int iD, string name)
    {
        ID = iD;
        Name = name;
    }
}

