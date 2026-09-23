import { createEntityStore } from "./createEntityStore";
import { invoicesEntity } from "../entities/InvoicesEntity";

const useInvoicesStore = createEntityStore(invoicesEntity);
export default useInvoicesStore;