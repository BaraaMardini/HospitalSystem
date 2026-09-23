import { createEntityStore } from "./createEntityStore";
import { insuranceSubscriptionsEntity } from "../entities/InsuranceSubscriptionsEntity";

const useInsuranceSubscriptionsStore = createEntityStore(insuranceSubscriptionsEntity);
export default useInsuranceSubscriptionsStore;