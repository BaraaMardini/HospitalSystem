import { createEntityStore } from "./createEntityStore";
import { appointmentEntity } from "../entities/AppointmentEntity";

const useAppointmentStore = createEntityStore(appointmentEntity);
export default useAppointmentStore;