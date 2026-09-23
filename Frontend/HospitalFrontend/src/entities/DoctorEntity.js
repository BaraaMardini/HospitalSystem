export const doctorEntity = {
  entity: "Doctors",
  operations: {
    search: {
      endpoint: "searchDoctor",
      filters: [
        { name: "PersonName", label: "PersonName", type: "string" },
        {
          name: "DepartmentName",
          label: "DepartmentName",
          type: "string",
          source: {
            entity: "Departments",
            operation: "getAll",
            valueField: "name",
            displayField: "name",
          },
        },
        {
          name: "SpecializationName",
          label: "SpecializationName",
          type: "string",
          source: {
            entity: "Specializations",
            operation: "getAll",
            valueField: "name",
            displayField: "name",
          },
        },
      ],
      columns: [
        { field: "id", header: "ID" },
        { field: "personName", header: "PersonName" },
        { field: "peopleID", header: "PeopleID" },
        { field: "departmentName", header: "DepartmentName" },
        { field: "specializationName", header: "SpecializationName" },
        { field: "qualification", header: "Qualification" },
        { field: "doctorLevel", header: "DoctorLevel" },
        { field: "salary", header: "Salary" },
      ],
    },
    getAll: {
      endpoint: "all",
      columns: [
        { field: "id", header: "ID" },
        { field: "personName", header: "PersonName" },
        { field: "peopleID", header: "PeopleID" },
        { field: "departmentName", header: "DepartmentName" },
        { field: "specializationName", header: "SpecializationName" },
        { field: "qualification", header: "Qualification" },
        { field: "doctorLevel", header: "DoctorLevel" },
        { field: "salary", header: "Salary" },
      ],
    },
    add: {
      endpoint: "",
      fields: [
        {
          name: "personID",
          label: "PersonName",
          type: "select",
          source: {
            entity: "Peoples",
            operation: "getAll",
            valueField: "id",
            displayField: "name",
          },
        },

        {
          name: "specializationID",
          label: "SpecializationName",
          type: "select",
          source: {
            entity: "specializations",
            operation: "getAll",
            valueField: "id",
            displayField: "name",
          },
        },

        {
          name: "departmentID",
          label: "DepartmentName",
          type: "select",
          source: {
            entity: "Departments",
            operation: "getAll",
            valueField: "id",
            displayField: "name",
          },
        },

        {
          name: "qualification",
          label: "Qualification",
          type: "text",
        },

        {
          name: "doctorLevel",
          label: "DoctorLevel",
          type: "text",
        },

        {
          name: "salary",
          label: "Salary",
          type: "number",
        },
      ],
    },
    update: {
      endpoint: "{value}",
      by: "id",
      fields: [
        {
          name: "specializationID",
          label: "SpecializationName",
          type: "select",
          source: {
            entity: "Specializations",
            operation: "getAll",
            valueField: "id",
            displayField: "name",
          },
        },

        {
          name: "departmentID",
          label: "DepartmentName",

          type: "select",
          source: {
            entity: "Departments",
            operation: "getAll",
            valueField: "id",
            displayField: "name",
          },
        },

        {
          name: "qualification",
          label: "Qualification",
          type: "text",
        },

        {
          name: "doctorLevel",
          label: "DoctorLevel",
          type: "text",
        },

        {
          name: "salary",
          label: "Salary",
          type: "number",
        },
      ],
    },
    delete: {
      endpoint: "{value}",
      by: "id",
    },
  },
};
