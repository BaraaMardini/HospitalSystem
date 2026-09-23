import { createEntityStore } from "./createEntityStore";
import { specializationEntity } from "../entities/SpecializationEntity";

const useSpecializationStore = createEntityStore(specializationEntity);

export default useSpecializationStore;