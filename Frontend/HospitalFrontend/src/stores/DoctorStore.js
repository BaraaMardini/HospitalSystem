import { createEntityStore } from "./createEntityStore";
import { doctorEntity } from "../entities/DoctorEntity";

const useDoctorStore = createEntityStore(doctorEntity);

export default useDoctorStore;