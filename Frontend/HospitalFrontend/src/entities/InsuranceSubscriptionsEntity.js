export const insuranceSubscriptionsEntity = {
    entity: "InsuranceSubscriptionss",
    operations: {
search: {
    endpoint: "searchInsuranceSubscriptions",
    filters: [
        { name: "PersonName", label: "PersonName", type: "select",         source: {
                entity: "People",
                operation: "getAll",
                valueField: "name",
                displayField: "name"
            } },
        { name: "CompanyName", label: "CompanyName", type: "select",          source: {
                entity: "Insurance",
                operation: "getAll",
                valueField: "id",
                displayField: "companyName"
            } },
        { name: "DurationMonths", label: "DurationMonths", type: "int", },
        { name: "EndDate", label: "EndDate", type: "DateTime", },
],
    columns: [
        { field: "id", header: "ID" },
        { field: "personName", header: "PersonName" },
        { field: "price", header: "Price" },
        { field: "companyName", header: "CompanyName" },
        { field: "coveragePercentage", header: "CoveragePercentage" },
        { field: "durationMonths", header: "DurationMonths" },
        { field: "startDate", header: "StartDate" },
        { field: "endDate", header: "EndDate" },
    ],
},
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "personName", header: "PersonName" },
        { field: "price", header: "Price" },
        { field: "companyName", header: "CompanyName" },
        { field: "coveragePercentage", header: "CoveragePercentage" },
        { field: "durationMonths", header: "DurationMonths" },
        { field: "startDate", header: "StartDate" },
        { field: "endDate", header: "EndDate" },
    ],
},
add: {
    endpoint: "",
    fields: [

        {
            name: "insuranceID",
            label: "companyName",
            type: "select",
            source: {
                entity: "Insurance",
                operation: "getAll",
                valueField: "id",
                displayField: "companyName"
            }
        },

        {
            name: "personID",
            label: "PersonName",
            type: "select",
            source: {
                entity: "People",
                operation: "getAll",
                valueField: "id",
                displayField: "name"
            }
        },

        {
            name: "startDate",
            label: "StartDate",
            type: "datetime-local"
        },

 
    ]
},
delete: {
    endpoint: "{value}",
    by: "id"
},
    },
};

