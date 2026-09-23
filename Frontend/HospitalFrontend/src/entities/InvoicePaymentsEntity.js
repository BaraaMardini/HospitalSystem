export const invoicePaymentsEntity = {
  entity: "InvoicePaymentss",
  operations: {
    search: {
      endpoint: "searchInvoicePayments",
      filters: [
        {
          name: "InvoiceID",
          label: "InvoiceID",
          type: "int",
          source: {
            entity: "Invoices",
            operation: "getAll",
            valueField: "id",
            displayField: "id",
          },
        },
        {
          name: "PersonName",
          label: "PersonName",
          type: "string",
          source: {
            entity: "People",
            operation: "getAll",
            valueField: "name",
            displayField: "name",
          },
        },
      ],
      columns: [
        { field: "id", header: "ID" },
        { field: "invoiceID", header: "InvoiceID" },
        { field: "personName", header: "PersonName" },
        { field: "paymentDate", header: "PaymentDate" },
        { field: "amountPaid", header: "AmountPaid" },
        { field: "notes", header: "Notes" },
        { field: "appointmentID", header: "AppointmentID" },
        { field: "remainingAmount", header: "RemainingAmount" },
      ],
    },
    getAll: {
      endpoint: "all",
      columns: [
        { field: "id", header: "ID" },
        { field: "invoiceID", header: "InvoiceID" },
        { field: "personName", header: "PersonName" },
        { field: "paymentDate", header: "PaymentDate" },
        { field: "amountPaid", header: "AmountPaid" },
        { field: "notes", header: "Notes" },
        { field: "appointmentID", header: "AppointmentID" },
        { field: "remainingAmount", header: "RemainingAmount" },
      ],
    },
    add: {
      endpoint: "",
      fields: [
        {
          name: "invoiceID",
          label: "InvoiceID",
          type: "select",
          source: {
            entity: "Invoices",
            operation: "getAll",
            valueField: "id",
            displayField: "id",
          },
        },

      
        {
          name: "amountPaid",
          label: "AmountPaid",
          type: "number",
        },

        {
          name: "notes",
          label: "Notes",
          type: "text",
        },
      ],
    },
    update: {
      endpoint: "{value}",
      by: "id",
      fields: [
        {
          name: "notes",
          label: "Notes",
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
