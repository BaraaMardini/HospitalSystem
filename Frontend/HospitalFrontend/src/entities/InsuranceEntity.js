export const insuranceEntity = {
    entity: "Insurances",
    operations: {
search: {
    endpoint: "searchInsurance",
    filters: [
        { name: "CompanyName", label: "CompanyName", type: "string", },
        { name: "CoveragePercentage", label: "CoveragePercentage", type: "int", },
        { name: "DefaultDurationMonths", label: "DefaultDurationMonths", type: "int", },
],
    columns: [
        { field: "id", header: "ID" },
        { field: "companyName", header: "CompanyName" },
        { field: "policyNumber", header: "PolicyNumber" },
        { field: "coveragePercentage", header: "CoveragePercentage" },
        { field: "defaultDurationMonths", header: "DefaultDurationMonths" },
        { field: "notes", header: "Notes" },
        { field: "price", header: "Price" },
    ],
},
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "companyName", header: "CompanyName" },
        { field: "policyNumber", header: "PolicyNumber" },
        { field: "coveragePercentage", header: "CoveragePercentage" },
        { field: "defaultDurationMonths", header: "DefaultDurationMonths" },
        { field: "notes", header: "Notes" },
        { field: "price", header: "Price" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "companyName",
            label: "CompanyName",
            type: "text"
        },

        {
            name: "policyNumber",
            label: "PolicyNumber",
            type: "text"
        },

        {
            name: "coveragePercentage",
            label: "CoveragePercentage",
            type: "number"
        },

        {
            name: "defaultDurationMonths",
            label: "DefaultDurationMonths",
            type: "number"
        },

        {
            name: "notes",
            label: "Notes",
            type: "text"
        },

        {
            name: "price",
            label: "Price",
            type: "number"
        },
    ]
},
update: {
    endpoint: "{value}",
    by: "id",
    fields: [

        {
            name: "companyName",
            label: "CompanyName",
            type: "text"
        },

        {
            name: "policyNumber",
            label: "PolicyNumber",
            type: "text"
        },

        {
            name: "coveragePercentage",
            label: "CoveragePercentage",
            type: "number"
        },

        {
            name: "defaultDurationMonths",
            label: "DefaultDurationMonths",
            type: "number"
        },

        {
            name: "notes",
            label: "Notes",
            type: "text"
        },

        {
            name: "price",
            label: "Price",
            type: "number"
        },
    ]
},
delete: {
    endpoint: "{value}",
    by: "id"
},
    },
};

