import { apiConfig } from "../config/api-config";
import axios from "axios";
import BaseService from "./BaseService";

const AuthService = class extends BaseService {
  register = async (request) => {
    return axios
      .post(apiConfig.paths.users.post.postRegister, request)
      .then((response) => {
        return response;
      })
      .catch((error) => {
        console.log(error);
      });
  };
};

export default AuthService;
