import { createEntityStore } from "./createEntityStore";
import { roomTypeEntity } from "../entities/RoomTypeEntity";

const useRoomTypeStore = createEntityStore(roomTypeEntity);

export default useRoomTypeStore;