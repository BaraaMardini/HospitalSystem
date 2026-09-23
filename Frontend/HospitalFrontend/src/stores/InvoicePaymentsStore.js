import { createEntityStore } from "./createEntityStore";
import { invoicePaymentsEntity } from "../entities/InvoicePaymentsEntity";

const useInvoicePaymentsStore = createEntityStore(invoicePaymentsEntity);
export default useInvoicePaymentsStore;