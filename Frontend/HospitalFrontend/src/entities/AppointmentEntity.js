export const appointmentEntity = {
  entity: "Appointments",
  operations: {
    search: {
      endpoint: "searchAppointment",
      filters: [
        {
          name: "DoctorName",
          label: "DoctorName",
          type: "select",
          source: {
            entity: "Doctor",
            operation: "getAll",
            valueField: "personName",
            displayField: "perosnName",
          },
        },
        {
          name: "PatientName",
          label: "PatientName",
          type: "select",
          source: {
            entity: "Patient",
            operation: "getAll",
            valueField: "personName",
            displayField: "personName",
          },
        },
        {
          name: "RoomNumber",
          label: "RoomNumber",
          type: "select",
          source: {
            entity: "Room",
            operation: "getAll",
            valueField: "roomNumber",
            displayField: "roomNumber",
          },
        },
        { name: "StartDate", label: "StartDate", type: "DateTime" },
        { name: "EndDate", label: "EndDate", type: "DateTime" },
      ],
      columns: [
        { field: "id", header: "ID" },
        { field: "doctorName", header: "DoctorName" },
        { field: "patientName", header: "PatientName" },
        { field: "statusName", header: "StatusName" },
        { field: "roomNumber", header: "RoomNumber" },
        { field: "startDate", header: "StartDate" },
        { field: "endDate", header: "EndDate" },
        { field: "notes", header: "Notes" },
        { field: "createdAt", header: "CreatedAt" },
      ],
    },
    getAll: {
      endpoint: "all",
      columns: [
        { field: "id", header: "ID" },
        { field: "doctorName", header: "DoctorName" },
        { field: "patientName", header: "PatientName" },
        { field: "statusName", header: "StatusName" },
        { field: "roomNumber", header: "RoomNumber" },
        { field: "startDate", header: "StartDate" },
        { field: "endDate", header: "EndDate" },
        { field: "notes", header: "Notes" },
        { field: "createdAt", header: "CreatedAt" },
      ],
    },
    add: {
      endpoint: "",
      fields: [
        {
          name: "patientID",
          label: "PatientName",
          type: "select",
          source: {
            entity: "Patient",
            operation: "getAll",
            valueField: "id",
            displayField: "personName",
          },
        },

        {
          name: "doctorID",
          label: "DoctorName",
          type: "select",
          source: {
            entity: "Doctor",
            operation: "getAll",
            valueField: "id",
            displayField: "personName",
          },
        },

        {
          name: "startDate",
          label: "StartDate",
          type: "datetime-local",
        },

        {
          name: "notes",
          label: "Notes",
          type: "text",
        },

    

        {
          name: "statusID",
          label: "StatusName",
          type: "select",
          source: {
            entity: "Status",
            operation: "getAll",
            valueField: "id",
            displayField: "statusName",
          },
        },

        {
          name: "endDate",
          label: "EndDate",
          type: "datetime-local",
        },

        {
          name: "roomID",
          label: "RoomNumber",
          type: "select",
          source: {
            entity: "Room",
            operation: "getAll",
            valueField: "id",
            displayField: "roomNumber",
          },
        },
      ],
    },
    update: {
      endpoint: "{value}",
      by: "id",
      fields: [
        {
          name: "doctorID",
          label: "DoctorName",
          type: "select",
          source: {
            entity: "Doctor",
            operation: "getAll",
            valueField: "id",
            displayField: "personName",
          },
        },

        {
          name: "startDate",
          label: "StartDate",
          type: "datetime-local",
        },

        {
          name: "notes",
          label: "Notes",
          type: "text",
        },

        {
          name: "statusID",
          label: "StatusName",
          type: "select",
          source: {
            entity: "Status",
            operation: "getAll",
            valueField: "id",
            displayField: "statusName",
          },
        },

        {
          name: "endDate",
          label: "EndDate",
          type: "datetime-local",
        },

        {
          name: "roomID",
          label: "RoomNumber",
          type: "select",
          source: {
            entity: "Room",
            operation: "getAll",
            valueField: "id",
            displayField: "roomNumber",
          },
        },
      ],
    },
    delete: {
      endpoint: "{value}",
      by: "id",
    },
  },
};
