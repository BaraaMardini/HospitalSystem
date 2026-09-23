export const patientEntity = {
  entity: "Patients",
  operations: {
    search: {
      endpoint: "searchPatient",
      filters: [
        { name: "PersonName", label: "PersonName", type: "string" },
        { name: "PersonID", label: "PersonID", type: "int" },
      ],
      columns: [
        { field: "id", header: "ID" },
        { field: "personName", header: "PersonName" },
        { field: "personID", header: "PersonID" },
        { field: "bloodType", header: "BloodType" },
        { field: "gender", header: "Gender" },
        { field: "age", header: "Age" },
      ],
    },
    getAll: {
      endpoint: "all",
      columns: [
        { field: "id", header: "ID" },
        { field: "personName", header: "PersonName" },
        { field: "personID", header: "PersonID" },
        { field: "bloodType", header: "BloodType" },
        { field: "gender", header: "Gender" },
        { field: "age", header: "Age" },
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
            entity: "People",
            operation: "getAll",
            valueField: "id",
            displayField: "name",
          },
        },

        {
          name: "bloodType",
          label: "BloodType",
          type: "text",
        },
      ],
    },
    update: {
      endpoint: "{value}",
      by: "id",
      fields: [
        {
          name: "personID",
          label: "PersonName",
          type: "select",
          source: {
            entity: "People",
            operation: "getAll",
            valueField: "id",
            displayField: "name",
          },
        },

        {
          name: "bloodType",
          label: "BloodType",
          type: "text",
        },
      ],
    },
    delete: {
      endpoint: "{value}",
      by: "id",
    },
  },
};
