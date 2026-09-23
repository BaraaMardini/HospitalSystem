import { createEntityStore } from "./createEntityStore";
import { medicalHistoryEntity } from "../entities/MedicalHistoryEntity";

const useMedicalHistoryStore = createEntityStore(medicalHistoryEntity);

export default useMedicalHistoryStore;