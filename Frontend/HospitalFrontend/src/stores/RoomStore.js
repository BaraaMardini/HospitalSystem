import { createEntityStore } from "./createEntityStore";
import { roomEntity } from "../entities/RoomEntity";

const useRoomStore = createEntityStore(roomEntity);

export default useRoomStore;