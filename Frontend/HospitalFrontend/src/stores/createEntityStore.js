import { create } from "zustand";
import { createEntityApi } from "../api/createEntityApi";

export function createEntityStore(entityConfig) {

  const api =
    createEntityApi(entityConfig);

  return create((set) => ({

    getAllState: {
      data: [],
      message: "",
      errorCode: 0,
      loading: false,
    },

    searchState: { 
      data: [],
      message: "",
      errorCode: 0,
      loading: false,
    },

    getOneState: {
      data: null,
      message: "",
      errorCode: 0,
      loading: false,
    },

    addState: {
      data: null,
      message: "",
      errorCode: 0,
      loading: false,
    },

    updateState: {
      data: null,
      message: "",
      errorCode: 0,
      loading: false,
    },

    deleteState: {
      data: null,
      message: "",
      errorCode: 0,
      loading: false,
    },

    // ====================
    // GET ALL
    // ====================

    fetchAll: api.getAll
      ? async () => {

          set((state) => ({
            getAllState: {
              ...state.getAllState,
              loading: true,
            },
          }));

          const result =
            await api.getAll();

          set({
            getAllState: {
              data: result.data,
              message: result.message,
              errorCode: result.errorCode,
              loading: false,
            },
          });

        }
      : undefined,

    // ====================
    // SEARCH
    // ====================

    search: api.search
      ? async (filters) => {

          set((state) => ({
            searchState: {
              ...state.searchState,
              loading: true,
            },
          }));

          const result =
            await api.search(filters);

    set((state) => ({

    searchState: {

        data:

            result.data.length > 0
                ? result.data
                : state.searchState.data,

        message: result.message,

        errorCode: result.errorCode,

        loading: false,

    },

}));

        }
      : undefined,

    // ====================
    // GET ONE
    // ====================

    getOne: api.getOne
      ? async (value) => {

          set((state) => ({
            getOneState: {
              ...state.getOneState,
              loading: true,
            },
          }));

          const result =
            await api.getOne(value);

          set({
            getOneState: {
              data: result.data,
              message: result.message,
              errorCode: result.errorCode,
              loading: false,
            },
          });

        }
      : undefined,

    // ====================
    // ADD
    // ====================

    add: api.add
      ? async (data) => {

          set((state) => ({
            addState: {
              ...state.addState,
              loading: true,
            },
          }));

          const result =
            await api.add(data);
            console.log("ADD RESULT:", result);

          set({
            addState: {
              data: result.data,
              message: result.message,
              errorCode: result.errorCode,
              loading: false,
            },
          });

        }
      : undefined,

    // ====================
    // UPDATE
    // ====================

    update: api.update
      ? async (
          value,
          data
        ) => {

          set((state) => ({
            updateState: {
              ...state.updateState,
              loading: true,
            },
          }));

          const result =
            await api.update(
              value,
              data
            );

          set({
            updateState: {
              data: result.data,
              message: result.message,
              errorCode: result.errorCode,
              loading: false,
            },
          });

        }
      : undefined,

    // ====================
    // DELETE
    // ====================

    remove: api.remove
      ? async (value) => {

          set((state) => ({
            deleteState: {
              ...state.deleteState,
              loading: true,
            },
          }));

          const result =
            await api.remove(value);

          set({
            deleteState: {
              data: result.data,
              message: result.message,
              errorCode: result.errorCode,
              loading: false,
            },
          });

        }
      : undefined,

  }));

}  