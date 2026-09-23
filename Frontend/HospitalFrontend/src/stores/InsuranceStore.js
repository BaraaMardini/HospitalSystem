import { createEntityStore } from "./createEntityStore";
import { insuranceEntity } from "../entities/InsuranceEntity";

const useInsuranceStore = createEntityStore(insuranceEntity);
export default useInsuranceStore;