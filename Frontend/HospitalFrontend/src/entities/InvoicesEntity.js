export const invoicesEntity = {
    entity: "Invoicess",
    operations: {
search: {
    endpoint: "searchInvoices",
    filters: [
        { name: "AppointmentID", label: "AppointmentID", type: "int", },
        { name: "PersonName", label: "PersonName", type: "string",  source: {
                entity: "People",
                operation: "getAll",
                valueField: "name",
                displayField: "name"
            }},
        { name: "StatusName", label: "StatusName", type: "string",  source: {
                entity: "Status",
                operation: "Search",
                valueField: "statusName",
                displayField: "statusName"
            } },
],
    columns: [
        { field: "id", header: "ID" },
        { field: "appointmentID", header: "AppointmentID" },
        { field: "invoiceDate", header: "InvoiceDate" },
        { field: "personName", header: "PersonName" },
        { field: "totalAmount", header: "TotalAmount" },
        { field: "insuranceAmount", header: "InsuranceAmount" },
        { field: "patientAmount", header: "PatientAmount" },
        { field: "statusName", header: "StatusName" },
        { field: "statusDescription", header: "StatusDescription" },
    ],
},
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "appointmentID", header: "AppointmentID" },
        { field: "invoiceDate", header: "InvoiceDate" },
        { field: "personName", header: "PersonName" },
        { field: "totalAmount", header: "TotalAmount" },
        { field: "insuranceAmount", header: "InsuranceAmount" },
        { field: "patientAmount", header: "PatientAmount" },
        { field: "statusName", header: "StatusName" },
        { field: "statusDescription", header: "StatusDescription" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "appointmentID",
            label: "AppointmentID",
            type: "select",
            source: {
                entity: "Appointment",
                operation: "getAll",
                valueField: "id",
                displayField: "id"
            }
        },



        {
            name: "totalAmount",
            label: "TotalAmount",
            type: "number"
        },

    
        {
            name: "statusID",
            label: "StatusName",
            type: "select",
           source: {
                entity: "Status",
                operation: "search",
                valueField: "id",
                displayField: "statusName"
            }
        },
    ]
},
update: {
    endpoint: "{value}",
    by: "id",
    fields: [

        {
            name: "totalAmount",
            label: "TotalAmount",
            type: "number"
        },

        {
            name: "statusID",
            label: "StatusName",
            type: "select",
            source: {
                entity: "Status",
                operation: "Search",
                valueField: "id",
                displayField: "statusName"
            }
        },
    ]
},
delete: {
    endpoint: "{value}",
    by: "id"
},
    },
};

