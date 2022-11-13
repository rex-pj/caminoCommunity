import axios from "axios";
import { apiConfig } from "../config/api-config";
import { getAuthenticationToken } from "./AuthLogic";

export default class BaseService {
  constructor() {
    axios.defaults.baseURL = apiConfig.camino_api;
    const { authenticationToken } = getAuthenticationToken();
    if (authenticationToken) {
      axios.defaults.headers.common["Authorization"] = authenticationToken;
    }
  }
}
