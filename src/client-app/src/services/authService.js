import { apiConfig } from "../config/api-config";
import axios from "axios";
import BaseService from "./BaseService";

const AuthService = class extends BaseService {
  login = async (request) => {
    return axios
      .post(apiConfig.paths.authentications.post.postLogin, request)
      .then((response) => {
        return response;
      })
      .catch((error) => {
        console.log(error);
      });
  };
};

export default AuthService;
