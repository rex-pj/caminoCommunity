import { apiConfig } from "../config/api-config";
import axios from "axios";
import BaseService from "./baseService";

const UserService = class extends BaseService {
  register = async (request) => {
    return axios.post(apiConfig.paths.users.post.postRegister, request);
  };

  partialUpdate = async (request) => {
    return axios.patch(apiConfig.paths.users.patch.patchUser, request);
  };

  updateIdentifiers = async (request) => {
    return axios.put(apiConfig.paths.users.put.putUserIdentifiers, request);
  };
};

export default UserService;
