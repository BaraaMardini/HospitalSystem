export const medicalHistoryEntity = {
    entity: "MedicalHistorys",
    operations: {
search: {
    endpoint: "searchMedicalHistory",
    filters: [
        { name: "PersonName", label: "PersonName", type: "string", },
        { name: "PatientID", label: "PatientID", type: "int", },
        { name: "DiagnosisDate", label: "DiagnosisDate", type: "DateTime", },
],
    columns: [
        { field: "id", header: "ID" },
        { field: "personName", header: "PersonName" },
        { field: "patientID", header: "PatientID" },
        { field: "conditionName", header: "ConditionName" },
        { field: "diagnosisDate", header: "DiagnosisDate" },
        { field: "notes", header: "Notes" },
    ],
},
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "personName", header: "PersonName" },
        { field: "patientID", header: "PatientID" },
        { field: "conditionName", header: "ConditionName" },
        { field: "diagnosisDate", header: "DiagnosisDate" },
        { field: "notes", header: "Notes" },
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
                displayField: "personName"
            }
        },

        {
            name: "conditionName",
            label: "ConditionName",
            type: "text"
        },

     

        {
            name: "notes",
            label: "Notes",
            type: "text"
        },
    ]
},
update: {
    endpoint: "{value}",
    by: "id",
    fields: [

        {
            name: "conditionName",
            label: "ConditionName",
            type: "text"
        },

        {
            name: "notes",
            label: "Notes",
            type: "text"
        },
    ]
},
delete: {
    endpoint: "{value}",
    by: "id"
},
    },
};

