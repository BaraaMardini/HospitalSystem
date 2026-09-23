import { API_URL } from "./apiConfig";
import { apiFetch } from "./httpClient";

export function createEntityApi(entityConfig) {
  const baseUrl = `${API_URL}/${entityConfig.entity}`;
  const operations = entityConfig.operations;
  const api = {};

  if (operations.getAll) {
    api.getAll = async () => {
      const response = await apiFetch(`${baseUrl}/${operations.getAll.endpoint}`);
      return response.json();
    };
  }

  if (operations.search) {
    api.search = async (filters) => {
      const params = new URLSearchParams(filters);
      const response = await apiFetch(`${baseUrl}/${operations.search.endpoint}?${params}`);
      return response.json();
    };
  }

  if (operations.getOne) {
    api.getOne = async (value) => {
      const endpoint = operations.getOne.endpoint.replace("{value}", value);
      const response = await apiFetch(`${baseUrl}/${endpoint}`);
      return response.json();
    };
  }

  if (operations.add) {
    api.add = async (data) => {
      const response = await apiFetch(`${baseUrl}/${operations.add.endpoint}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });
      return response.json();
    };
  }

  if (operations.update) {
    api.update = async (value, data) => {
      const endpoint = operations.update.endpoint.replace("{value}", value);
      const response = await apiFetch(`${baseUrl}/${endpoint}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });
      return response.json();
    };
  }

  if (operations.delete) {
    api.remove = async (value) => {
      const endpoint = operations.delete.endpoint.replace("{value}", value);
      const response = await apiFetch(`${baseUrl}/${endpoint}`, { method: "DELETE" });
      return response.json();
    };
  }

  return api;
}


