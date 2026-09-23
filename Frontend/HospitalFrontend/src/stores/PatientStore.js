import { createEntityStore } from "./createEntityStore";
import { patientEntity } from "../entities/PatientEntity";

const usePatientStore = createEntityStore(patientEntity);

export default usePatientStore;